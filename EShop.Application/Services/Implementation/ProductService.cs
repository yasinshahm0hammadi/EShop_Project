using EShop.Application.Extensions;
using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.DTOs.Paging;
using EShop.Domain.DTOs.Product;
using EShop.Domain.DTOs.Product.ProductCategory;
using EShop.Domain.DTOs.Product.ProductColor;
using EShop.Domain.DTOs.Product.ProductFeature;
using EShop.Domain.DTOs.Product.ProductGallery;
using EShop.Domain.Entities.Product;
using EShop.Domain.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EShop.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        #region Ctor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategoryRepository;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedCategoryRepository;
        private readonly IGenericRepository<ProductColor> _productColorRepository;
        private readonly IGenericRepository<ProductFeature> _productFeatureRepository;
        private readonly IGenericRepository<ProductGallery> _productGalleryRepository;

        public ProductService(IGenericRepository<Product> productRepository,
            IGenericRepository<ProductCategory> productCategoryRepository,
            IGenericRepository<ProductSelectedCategory> productSelectedCategoryRepository,
            IGenericRepository<ProductColor> productColorRepository,
            IGenericRepository<ProductFeature> productFeatureRepository,
            IGenericRepository<ProductGallery> productGalleryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _productSelectedCategoryRepository = productSelectedCategoryRepository;
            _productColorRepository = productColorRepository;
            _productFeatureRepository = productFeatureRepository;
            _productGalleryRepository = productGalleryRepository;
        }

        #endregion

        #region Product

        public async Task<FilterProductDto> FilterProducts(FilterProductDto filterProduct)
        {
            var query = _productRepository
                .GetQuery()
                .Where(q => q.IsPublished)
                .Include(q => q.ProductColors)
                .Include(q => q.PruductFeatures)
                .Include(q => q.ProductGalleries)
                .AsQueryable();

            if (query != null)
            {
                #region Display Order

                switch (filterProduct.OrderBy)
                {
                    case FilterProductOrderBy.CreateDateDescending:
                        query = query.OrderByDescending(q => q.CreatedAt);
                        break;
                    case FilterProductOrderBy.CreateDateAscending:
                        query = query.OrderBy(q => q.CreatedAt);
                        break;
                    case FilterProductOrderBy.PriceAscending:
                        query = query.OrderBy(q => q.Price);
                        break;
                    case FilterProductOrderBy.PriceDescending:
                        query = query.OrderByDescending(q => q.Price);
                        break;
                    case FilterProductOrderBy.ViewCountDescending:
                        query = query.OrderByDescending(q => q.ViewCount);
                        break;
                    case FilterProductOrderBy.SellCountDescending:
                        query = query.OrderByDescending(q => q.SellCount);
                        break;
                    default:
                        return new FilterProductDto();
                }

                #endregion

                #region Filter By Price

                var expenciveProduct = await query.OrderByDescending(q => q.Price).FirstOrDefaultAsync();
                var cheapProduct = await query.OrderBy(q => q.Price).FirstOrDefaultAsync();

                if (expenciveProduct is not null || cheapProduct is not null)
                {
                    filterProduct.FilterMaxPrice = expenciveProduct.Price;
                    filterProduct.FilterMinPrice = cheapProduct.Price;
                }

                if (filterProduct.SelectedMinPrice is not null || filterProduct.SelectedMaxPrice is not null)
                {
                    query = query.Where(q => q.Price <= filterProduct.SelectedMaxPrice);
                    query = query.Where(q => q.Price >= filterProduct.SelectedMinPrice);
                }


                #endregion

                #region Pagination

                var productCount = await query.CountAsync();
                filterProduct.ProductsCount  = productCount;

                var pager = Pager.Build(filterProduct.PageId, productCount, filterProduct.TakeEntity,
                    filterProduct.HowManyShowPageAfterAndBefore);

                var allEntities = await query.Paging(pager).ToListAsync();

                #endregion

                return filterProduct.SetPaging(pager).SetProduct(allEntities);
            }
            else
            {
                return null;
            }
        }
        public async Task<FilterProductDto> FilterProductsInAdminPanel(FilterProductDto product)
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
                        query = query.OrderByDescending(x => x.CreatedAt);
                        break;
                    case FilterProductOrderBy.PriceAscending:
                        query = query.OrderBy(x => x.Price);
                        break;
                    case FilterProductOrderBy.PriceDescending:
                        query = query.OrderByDescending(x => x.Price);
                        break;
                    case FilterProductOrderBy.ViewCountDescending:
                        query = query.OrderByDescending(x => x.ViewCount);
                        break;
                    case FilterProductOrderBy.SellCountDescending:
                        query = query.OrderByDescending(x => x.SellCount);
                        break;
                    case FilterProductOrderBy.CreateDateAscending:
                        query = query.OrderBy(x => x.CreatedAt);
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
        public async Task<CreateProductResult> CreateProduct(CreateProductDto product, string? creatorName)
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
                    await AddProductSelectedCategories(newProduct.Id, product.SelectedCategories, creatorName);
                    await _productSelectedCategoryRepository.SaveChanges();
                }

                await _productRepository.AddEntity(newProduct, creatorName);
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
        public async Task<EditProductResult> EditProduct(EditProductDto product, string? modifierName)
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

                        await AddProductSelectedCategories(product.Id, product.SelectedCategories, modifierName);
                        await _productCategoryRepository.SaveChanges();
                    }

                    _productRepository.EditEntity(existingProduct, modifierName);
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
                .Where(x => x.IsPublished && x.IsActive)
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
                .Where(x => x.IsActive && !x.IsPublished)
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
        public async Task<bool> ActivateProduct(long id, string? modifierName)
        {
            try
            {
                var product = await _productRepository
               .GetQuery()
               .SingleOrDefaultAsync(x => x.Id == id);

                if (product != null)
                {
                    product.IsActive = true;
                    product.IsPublished = false;

                    _productRepository.EditEntity(product, modifierName);
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
        public async Task<bool> DeActivateProduct(long id, string? modifierName)
        {
            try
            {
                var product = await _productRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                if (product != null)
                {
                    product.IsActive = false;
                    product.IsPublished = true;

                    _productRepository.EditEntity(product, modifierName);
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
                    query = query.Where(x => EF.Functions.Like(x.Title, $"%{productCategory.Title}%")).OrderByDescending(x => x.CreatedAt);
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
                .Where(x => x.IsActive && !x.IsPublished)
                .ToListAsync();
        }
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto productCategory, string? creatorName)
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

                await _productCategoryRepository.AddEntity(newProductCategory, creatorName);
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
        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto productCategory, string? modifierName)
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

                    _productCategoryRepository.EditEntity(existingProductCategory, modifierName);
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
        public async Task<bool> ActivateProductCategory(long id, string? modifierName)
        {
            var productCategory = await _productCategoryRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (productCategory != null)
            {
                productCategory.IsActive = true;
                productCategory.IsPublished = false;

                _productCategoryRepository.EditEntity(productCategory, modifierName);
                await _productCategoryRepository.SaveChanges();

                return true;
            }

            return false;
        }
        public async Task<bool> DeActivateProductCategory(long id, string? modifierName)
        {
            var productCategory = await _productCategoryRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (productCategory != null)
            {
                productCategory.IsActive = false;
                productCategory.IsPublished = true;

                _productCategoryRepository.EditEntity(productCategory, modifierName);
                await _productCategoryRepository.SaveChanges();

                return true;
            }

            return false;
        }

        #region Add / Remove Product Category

        public async Task AddProductSelectedCategories(long productId, List<long> productSelectedCategoriesId, string? creatorName)
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

                await _productSelectedCategoryRepository.AddRangeEntity(productSelectedCategories, creatorName);
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

                _productSelectedCategoryRepository.DeleteEntities(productSelectedCategories);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        #endregion

        #endregion

        #region Product Color

        public async Task<List<FilterProductColorDto>> FilterProductColors(long productId)
        {
            try
            {
                return await _productColorRepository
                    .GetQuery()
                    .Include(x => x.Product)
                    .Where(x => x.ProductId == productId)
                    .Select(x => new FilterProductColorDto
                    {
                        Id = x.ProductId,
                        ColorName = x.ColorName,
                        ColorCode = x.ColorCode,
                        Price = x.Price,
                        CreateDate = x.CreatedAt.ToString(),
                        IsActive = x.IsPublished,
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<FilterProductColorDto>();
            }
        }
        public async Task<CreateProductColorResult> CreateProductColor(CreateProductColorDto color, long productId, string? creatorName)
        {
            try
            {
                var product = _productRepository.GetEntityById(productId);

                if (product == null)
                {
                    return CreateProductColorResult.ProductNotFound;
                }

                foreach (var productColor in color.ProductColors)
                {
                    var isDuplicatedColorTitle = await _productColorRepository
                        .GetQuery()
                        .AnyAsync(x => x.ColorName == productColor.ColorName);

                    if (isDuplicatedColorTitle)
                    {
                        return CreateProductColorResult.DuplicateColor;
                    }
                }

                await AddProductColors(productId, color.ProductColors, creatorName);
                await _productColorRepository.SaveChanges();

                return CreateProductColorResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);


                return CreateProductColorResult.Error;
            }
        }
        public async Task<EditProductColorDto> GetProductColorForEdit(long colorId)
        {
            try
            {
                var productColor = await _productColorRepository.GetEntityById(colorId);

                if (productColor == null)
                {
                    return null;
                }

                return new EditProductColorDto
                {
                    Id = productColor.Id,
                    ColorName = productColor.ColorName,
                    ColorCode = productColor.ColorCode,
                    Price = productColor.Price
                };
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new EditProductColorDto();
            }
        }
        public async Task<EditProductColorResult> EditProductColor(EditProductColorDto color, long colorId, string? modifierName)
        {
            try
            {
                var existingColor = await _productColorRepository
                    .GetQuery()
                    .SingleOrDefaultAsync(x => x.Id == colorId);

                if (existingColor is null)
                {
                    return EditProductColorResult.ColorNotFound;
                }

                var isDuplicatedColorTitle = await _productColorRepository
                    .GetQuery()
                    .AnyAsync(x => x.Id == colorId);

                if (isDuplicatedColorTitle)
                {
                    return EditProductColorResult.DuplicateColor;
                }

                existingColor.ColorName = color.ColorName;
                existingColor.ColorCode = color.ColorCode;
                existingColor.Price = color.Price;

                _productColorRepository.EditEntity(existingColor, modifierName);
                await _productColorRepository.SaveChanges();

                return EditProductColorResult.ColorNotFound;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return EditProductColorResult.Error;
            }
        }

        #region Add or Remove Product Colors

        public async Task AddProductColors(long productId, List<CreateProductColorDto> productColors, string? creatorName)
        {
            var productSelectedColors = new List<ProductColor>();

            foreach (var productColor in productColors)
            {
                if (productSelectedColors.All(x => x.ColorName == productColor.ColorName))
                {
                    productSelectedColors.Add(new ProductColor
                    {
                        ProductId = productId,
                        ColorName = productColor.ColorName,
                        ColorCode = productColor.ColorCode,
                        Price = productColor.Price,
                    });
                }
            }

            await _productColorRepository.AddRangeEntity(productSelectedColors, creatorName);
        }

        #endregion

        #endregion

        #region Product Features

        public async Task<List<FilterProductFeatureDto>> GetAllProductFeaturesInAdminPanel(long productId)
        {
            try
            {
                return await _productFeatureRepository
               .GetQuery()
               .Include(x => x.Product)
               .Where(x => x.ProductId == productId)
               .OrderByDescending(x => x.CreatedAt)
               .Select(x => new FilterProductFeatureDto
               {
                   Id = x.Id,
                   ProductId = productId,
                   FeatureTitle = x.FeatureTitle,
                   FeatureValue = x.FeatureValue,
                   CreatedAt = x.CreatedAt.ToString()
               }).ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<FilterProductFeatureDto>();
            }
        }
        public async Task<CreateProductFeatureResult> CreateProductFeature(CreateProductFeatureDto feature, long productId, string? modifierName)
        {
            try
            {
                var product = await _productRepository.GetEntityById(productId);

                if (product is null)
                {
                    return CreateProductFeatureResult.ProductNotFound;
                }

                await AddProductFeatures(productId, feature.ProductFeatures, modifierName);
                await _productFeatureRepository.SaveChanges();

                return CreateProductFeatureResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return CreateProductFeatureResult.Error;
            }
        }

        #region Add or Remove Product Features

        public async Task AddProductFeatures(long productId, List<CreateProductFeatureDto> features, string? creatorName)
        {
            try
            {
                var productSelectedFeature = new List<ProductFeature>();

                foreach (var feature in features)
                {
                    if (productSelectedFeature.All(x => x.FeatureTitle != feature.FeatureTitle))
                    {
                        productSelectedFeature.Add(new ProductFeature
                        {
                            ProductId = productId,
                            FeatureTitle = feature.FeatureTitle,
                            FeatureValue = feature.FeatureValue,
                        });
                    }
                }

                await _productFeatureRepository.AddRangeEntity(productSelectedFeature, creatorName);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        #endregion

        #endregion

        #region Product Gallery

        public async Task<List<ProductGallery>> GetAllProductGalleries(long productId)
        {
            try
            {
                return await _productGalleryRepository
                    .GetQuery()
                    .Where(x => x.ProductId == productId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<ProductGallery>();
            }
        }

        public async Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDto gallery, long productId, IFormFile galleryImage, string? creatorName)
        {
            try
            {
                var product = await _productRepository.GetEntityById(productId);

                if (product == null)
                {
                    return CreateOrEditProductGalleryResult.ProductNotFound;
                }

                if (galleryImage == null || !galleryImage.IsImage())
                {
                    return CreateOrEditProductGalleryResult.ImageIsNullOrInvalid;
                }

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(galleryImage.FileName);
                galleryImage.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100, PathExtension.ProductGalleryThumbServer);

                var newGallery = new ProductGallery
                {
                    ProductId = productId,
                    ImageName = imageName,
                    DisplayPriority = gallery.DisplayPriority
                };

                await _productGalleryRepository.AddEntity(newGallery, creatorName);
                await _productGalleryRepository.SaveChanges();

                return CreateOrEditProductGalleryResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return CreateOrEditProductGalleryResult.Error;
            }

        }

        public async Task<CreateOrEditProductGalleryDto> GetProductGalleryForEdit(long galleryId)
        {
            var gallery = await _productGalleryRepository
                .GetQuery()
                .Include(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == galleryId);

            if (gallery == null)
            {
                return null;
            }

            return new CreateOrEditProductGalleryDto
            {
                Image = gallery.ImageName,
                DisplayPriority = gallery.DisplayPriority
            };
        }

        public async Task<CreateOrEditProductGalleryResult> EditProductGallery(CreateOrEditProductGalleryDto gallery, long galleryId, IFormFile galleryImage, string? modifierName)
        {
            try
            {
                var existingGallery = await _productGalleryRepository
                .GetQuery()
                .Include(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == galleryId);

                if (existingGallery == null)
                {
                    return CreateOrEditProductGalleryResult.ProductNotFound;
                }

                if (galleryImage == null || !galleryImage.IsImage())
                {
                    return CreateOrEditProductGalleryResult.ImageIsNullOrInvalid;
                }

                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(galleryImage.FileName);
                galleryImage.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer,
                    100, 100, PathExtension.ProductGalleryThumbServer, existingGallery.ImageName);

                existingGallery.ImageName = imageName;
                existingGallery.DisplayPriority = gallery.DisplayPriority;

                _productGalleryRepository.EditEntity(existingGallery, modifierName);

                return CreateOrEditProductGalleryResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return CreateOrEditProductGalleryResult.Error;
            }
        }

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
