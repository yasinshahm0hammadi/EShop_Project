using EShop.Domain.DTOs.Product;
using EShop.Domain.DTOs.Product.ProductCategory;
using EShop.Domain.Entities.Product;

namespace EShop.Application.Services.Interface
{
    public interface IProductService : IAsyncDisposable
    {
        #region Product

        Task<FilterProductDto> FilterProducts(FilterProductDto product);
        Task<CreateProductResult> CreateProduct(CreateProductDto product);
        Task<EditProductDto> GetProductForEdit(long id);
        Task<EditProductResult> EditProduct(EditProductDto product, string editorName);
        Task<List<Product>> GetProductsWithMaximumView(int take);
        Task<List<Product>> GetLatestArrivalProducts(int take);
        Task<bool> ActivateProduct(long id);
        Task<bool> DeActivateProduct(long id);

        #endregion

        #region Product Category

        Task<FilterProductCategoriesDto> FilterProductCategories(FilterProductCategoriesDto productCategory);
        Task<List<ProductCategory>> GetAllActiveProductCategories();
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto productCategory);
        Task<EditProductCategoryDto> GetProductCategoryForEdit(long id);
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto productCategory, string editorName);
        Task<bool> ActivateProductCategory(long id);
        Task<bool> DeActivateProductCategory(long id);

        #endregion
    }
}
