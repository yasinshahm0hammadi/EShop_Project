using EShop.Application.Extensions;
using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.DTOs.Paging;
using EShop.Domain.DTOs.Product;
using EShop.Domain.DTOs.Product.ProductCategory;
using EShop.Domain.Entities.Product;
using EShop.Domain.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        #region Ctor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategoryRepository, IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSelectedCategoryRepository = productSelectedCategoryRepository;
        }

        #endregion

        #region Product

        public async Task<FilterProductDto> FilterProducts(FilterProductDto product)
        {
            try
            {
                var query = _productRepository
                .GetQuery()
                .AsQueryable();

                #region Product State

                switch (product.ProductState)
                {
                    case FilterProductState.All:
                        break;
                    case FilterProductState.Active:
                        query = query.Where(x => x.IsActive);
                        break;
                    case FilterProductState.NotActive:
                        query = query.Where(x => !x.IsActive);
                        break;
                }

                #endregion

                #region Product Order

                switch (product.OrderBy)
                {
                    case FilterProductOrderBy.CreateDateDescending:
                        query = query.OrderByDescending(x => x.CreateDate);
                        break;
                    case FilterProductOrderBy.PriceAscending:
                        query = query.OrderBy(x => x.Price);
                        break;
                    case FilterProductOrderBy.PriceDescending:
                        query = query.OrderByDescending(x => x.Price);
                        break;
                    case FilterProductOrderBy.ViewDescending:
                        query = query.OrderByDescending(x => x.ViewCount);
                        break;
                    case FilterProductOrderBy.SellDescending:
                        query = query.OrderByDescending(x => x.SellCount);
                        break;
                    case FilterProductOrderBy.SellAscending:
                        query = query.OrderBy(x => x.SellCount);
                        break;
                    case FilterProductOrderBy.CreateDateAscending:
                        query = query.OrderBy(x => x.CreateDate);
                        break;
                }

                #endregion

                #region Filter Products

                if (!string.IsNullOrWhiteSpace(product.ProductTitle))
                {
                    query = query.Where(x => EF.Functions.Like(x.Title, $"%{product.ProductTitle}%"));
                }

                #endregion

                #region Product Paging

                var productCount = await query.CountAsync();

                var pager = Pager.Build(product.PageId, productCount, product.TakeEntity,
                    product.HowManyShowPageAfterAndBefore);

                var allEntities = await query.Paging(pager).ToListAsync();

                #endregion

                return product.SetPaging(pager).SetProduct(allEntities);
            }
            catch (Exception ex)
            { 
                Logger.ShowError(ex);

                return new FilterProductDto();
            }
        }
        public async Task<CreateProductResult> CreateProduct(CreateProductDto product)
        {
            try
            {
                string productImageName = null;
                if (product.Image != null)
                {
                    if (product.Image.IsImage())
                    {
                        productImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(product.Image.FileName);
                        product.Image.AddImageToServer(productImageName, PathExtension.ProductOriginServer,
                            100, 100, PathExtension.ProductThumbServer);
                    }
                    else
                    {
                        return CreateProductResult.ImageErrorType;
                    }
                }

                var newProduct = new Product
                {
                    Title = product.Title,
                    Code = new Random().Next(100000, 999999).ToString(),
                    ShortDescription = product.ShortDescription,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    Image = productImageName,
                    Price = product.Price,
                    SellCount = 0,
                    ViewCount = 0
                };

                if (product.SelectedCategories is not null)
                {
                    await AddProductSelectedCategories(newProduct.Id, product.SelectedCategories);
                    await _productSelectedCategoryRepository.SaveChanges();
                }

                await _productRepository.AddEntity(newProduct);
                await _productRepository.SaveChanges();

                return CreateProductResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return CreateProductResult.Error;
            }
        }
        public async Task<EditProductDto> GetProductForEdit(long id)
        {
            try
            {
                var existingProduct = await _productRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                var selectedCategories = await _productSelectedCategoryRepository
                    .GetQuery()
                    .Where(x => x.ProductId == id)
                    .Select(x => x.ProductCategoryId)
                    .ToListAsync();

                if (existingProduct != null)
                {
                    return new EditProductDto
                    {
                        Id = id,
                        Title = existingProduct.Title,
                        ShortDescription = existingProduct.ShortDescription,
                        Description = existingProduct.Description,
                        Price = existingProduct.Price,
                        ImageFileName = existingProduct.Image,
                        SelectedCategories = selectedCategories,
                        IsActive = existingProduct.IsActive,
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new EditProductDto();
            }
        }
        public async Task<EditProductResult> EditProduct(EditProductDto product, string editorName)
        {
            try
            {
                var existingProduct = await _productRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == product.Id);

                if (existingProduct != null)
                {
                    if (product.Image is not null)
                    {
                        if (product.Image.IsImage())
                        {
                            var productImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(product.Image.FileName);
                            product.Image.AddImageToServer(productImageName, PathExtension.ProductOriginServer,
                                100, 100, PathExtension.ProductThumbServer);

                            existingProduct.Image = productImageName;
                        }
                        else
                        {
                            return EditProductResult.ImageErrorType;
                        }
                    }

                    existingProduct.Id = product.Id;
                    existingProduct.Title = product.Title;
                    existingProduct.ShortDescription = product.ShortDescription;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.IsActive = product.IsActive;


                    if (product.SelectedCategories is not null)
                    {
                        await RemoveProductSelectedCategories(product.Id);

                        await AddProductSelectedCategories(product.Id, product.SelectedCategories);
                        await _productCategoryRepository.SaveChanges();
                    }

                    _productRepository.EditEntityByEditor(existingProduct, editorName);
                    await _productRepository.SaveChanges();

                    return EditProductResult.Success;
                }

                return EditProductResult.NotFound;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return EditProductResult.Error;
            }
        }
        public async Task<List<Product>> GetProductsWithMaximumView(int take)
        {
            try
            {
                var maxViewedProducts = await _productRepository
                .GetQuery()
                .Where(x => x.IsDelete && x.IsActive)
                .OrderByDescending(x => x.ViewCount)
                .Skip(0)
                .Take(take)
                .Distinct()
                .ToListAsync();

                if (maxViewedProducts.Count > take)
                {
                    maxViewedProducts.Skip(take - 1).Take(1).ToList();
                }

                return maxViewedProducts;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<Product>();
            }
        }
        public async Task<List<Product>> GetLatestArrivalProducts(int take)
        {
            try
            {
                var latestArrivalProducts = await _productRepository
                .GetQuery()
                .Where(x => x.IsActive && !x.IsDelete)
                .OrderByDescending(x => x.ViewCount)
                .Skip(0)
                .Take(take)
                .Distinct()
                .ToListAsync();

                if (latestArrivalProducts.Count > take)
                {
                    latestArrivalProducts.Skip(14).Take(1).ToList();
                }

                return latestArrivalProducts;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<Product>();
            }
        }
        public async Task<bool> ActivateProduct(long id)
        {
            try
            {
                var product = await _productRepository
               .GetQuery()
               .SingleOrDefaultAsync(x => x.Id == id);

                if (product != null)
                {
                    product.IsActive = true;
                    product.IsDelete = false;

                    _productRepository.EditEntity(product);
                    await _productRepository.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return false;
            }
        }
        public async Task<bool> DeActivateProduct(long id)
        {
            try
            {
                var product = await _productRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                if (product != null)
                {
                    product.IsActive = false;
                    product.IsDelete = true;

                    _productRepository.EditEntity(product);
                    await _productRepository.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return false;
            }
        }

        #endregion

        #region Product Category

        public async Task<FilterProductCategoriesDto> FilterProductCategories(FilterProductCategoriesDto productCategory)
        {
            try
            {
                var query = _productCategoryRepository
            .GetQuery()
            .Where(x => x.ParentId == productCategory.ParentId)
            .Include(x => x.ProductSelectedCategories)
            .AsQueryable();

                #region Filter

                if (!string.IsNullOrWhiteSpace(productCategory.Title))
                {
                    query = query.Where(x => EF.Functions.Like(x.Title, $"%{productCategory.Title}%")).OrderByDescending(x => x.CreateDate);
                }

                #endregion

                #region Paging

                var productCategoryCount = await query.CountAsync();

                var pager = Pager.Build(productCategory.PageId, productCategoryCount, productCategory.TakeEntity,
                    productCategory.HowManyShowPageAfterAndBefore);

                var allEntities = await query.Paging(pager).ToListAsync();

                #endregion

                return productCategory.SetPaging(pager).SetProductCategory(allEntities);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new FilterProductCategoriesDto();
            }
        }
        public async Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return await _productCategoryRepository
                .GetQuery()
                .Where(x => x.IsActive && !x.IsDelete)
                .ToListAsync();
        }
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto productCategory)
        {
            try
            {
                string imageName = null;
                if (productCategory.Image is not null)
                {
                    if (productCategory.Image.IsImage())
                    {
                        imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productCategory.Image.FileName);
                        productCategory.Image.AddImageToServer(imageName, PathExtension.ProductCategoryOriginServer,
                            100, 100, PathExtension.ProductCategoryThumbServer);
                    }
                    else
                    {
                        return CreateProductCategoryResult.ImageErrorType;
                    }
                }

                var newProductCategory = new ProductCategory
                {
                    Title = productCategory.Title,
                    UrlName = productCategory.Title.Replace(" ", "-"),
                    Image = imageName,
                    Icon = productCategory.Icon,
                    ParentId = productCategory.ParentId ?? null,
                    IsActive = productCategory.IsActive
                };

                await _productCategoryRepository.AddEntity(newProductCategory);
                await _productCategoryRepository.SaveChanges();

                return CreateProductCategoryResult.Success;
            }
            catch (Exception)
            {
                return CreateProductCategoryResult.Error;
            }
        }
        public async Task<EditProductCategoryDto> GetProductCategoryForEdit(long id)
        {
            var existingProductCategory = await _productCategoryRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (existingProductCategory is not null)
            {
                return new EditProductCategoryDto
                {
                    Id = existingProductCategory.Id,
                    Title = existingProductCategory.Title,
                    ExistingImage = existingProductCategory.Image,
                    IsActive = existingProductCategory.IsActive,
                    Icon = existingProductCategory.Icon,
                    ParentId = existingProductCategory.ParentId ?? null
                };
            }

            return null;
        }
        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto productCategory, string editorName)
        {
            try
            {
                var existingProductCategory = await _productCategoryRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == productCategory.Id);

                if (existingProductCategory is not null)
                {
                    if (productCategory.Image is not null)
                    {
                        if (productCategory.Image.IsImage())
                        {
                            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productCategory.Image.FileName);
                            productCategory.Image.AddImageToServer(imageName, PathExtension.ProductCategoryOriginServer,
                                100, 100, PathExtension.ProductCategoryThumbServer);

                            existingProductCategory.Image = imageName;
                        }
                        else
                        {
                            return EditProductCategoryResult.ImageErrorType;
                        }
                    }

                    existingProductCategory.Title = productCategory.Title;
                    existingProductCategory.UrlName = productCategory.Title.Replace(" ", "-");
                    existingProductCategory.Icon = productCategory.Icon;
                    existingProductCategory.ParentId = productCategory.Id;
                    existingProductCategory.IsActive = productCategory.IsActive;

                    _productCategoryRepository.EditEntityByEditor(existingProductCategory, editorName);
                    await _productCategoryRepository.SaveChanges();

                    return EditProductCategoryResult.Success;
                }

                return EditProductCategoryResult.NotFound;
            }
            catch (Exception)
            {
                return EditProductCategoryResult.Error;
            }
        }
        public async Task<bool> ActivateProductCategory(long id)
        {
            var productCategory = await _productCategoryRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (productCategory != null)
            {
                productCategory.IsActive = true;
                productCategory.IsDelete = false;

                _productCategoryRepository.EditEntity(productCategory);
                await _productCategoryRepository.SaveChanges();

                return true;
            }

            return false;
        }
        public async Task<bool> DeActivateProductCategory(long id)
        {
            var productCategory = await _productCategoryRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (productCategory != null)
            {
                productCategory.IsActive = false;
                productCategory.IsDelete = true;

                _productCategoryRepository.EditEntity(productCategory);
                await _productCategoryRepository.SaveChanges();

                return true;
            }

            return false;
        }

        #region Add / Remove Product Category

        public async Task AddProductSelectedCategories(long productId, List<long> productSelectedCategoriesId)
        {
            try
            {
                var productSelectedCategories = new List<ProductSelectedCategory>();

                foreach (var categoryId in productSelectedCategoriesId)
                {
                    productSelectedCategories.Add(new ProductSelectedCategory
                    {
                        ProductCategoryId = categoryId,
                        ProductId = productId
                    });
                }

                await _productSelectedCategoryRepository.AddRangeEntity(productSelectedCategories);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }
        public async Task RemoveProductSelectedCategories(long productId)
        {
            try
            {
                var productSelectedCategories = await _productSelectedCategoryRepository
                .GetQuery()
                .Where(x => x.Id == productId)
                .ToListAsync();

                _productSelectedCategoryRepository.DeletePermanentEntities(productSelectedCategories);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        #endregion

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_productRepository != null)
            {
                await _productRepository.DisposeAsync();
            }
        }

        #endregion
    }
}
