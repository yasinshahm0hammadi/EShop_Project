using EShop.Domain.DTOs.Product;
using EShop.Domain.DTOs.Product.ProductCategory;
using EShop.Domain.DTOs.Product.ProductColor;
using EShop.Domain.DTOs.Product.ProductFeature;
using EShop.Domain.DTOs.Product.ProductGallery;
using EShop.Domain.Entities.Product;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.Services.Interface
{
    public interface IProductService : IAsyncDisposable
    {
        #region Product

        Task<FilterProductDto> FilterProducts(FilterProductDto filterProduct);
        Task<FilterProductDto> FilterProductsInAdminPanel(FilterProductDto product);
        Task<CreateProductResult> CreateProduct(CreateProductDto product, string? creatorName);
        Task<EditProductDto> GetProductForEdit(long id);
        Task<EditProductResult> EditProduct(EditProductDto product, string? modifierName);
        Task<List<Product>> GetProductsWithMaximumView(int take);
        Task<List<Product>> GetLatestArrivalProducts(int take);
        Task<bool> ActivateProduct(long id, string? modifierName);
        Task<bool> DeActivateProduct(long id, string? modifierName);

        #endregion

        #region Product Category

        Task<FilterProductCategoriesDto> FilterProductCategories(FilterProductCategoriesDto productCategory);
        Task<List<ProductCategory>> GetAllActiveProductCategories();
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryDto productCategor, string? creatorName);
        Task<EditProductCategoryDto> GetProductCategoryForEdit(long id);
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryDto productCategory, string? modifierName);
        Task<bool> ActivateProductCategory(long id, string? modifierName);
        Task<bool> DeActivateProductCategory(long id, string? modifierName);

        #endregion

        #region Product Color

        Task<List<FilterProductColorDto>> FilterProductColors(long productId);
        Task<CreateProductColorResult> CreateProductColor(CreateProductColorDto color, long productId, string? creatorName);
        Task<EditProductColorDto> GetProductColorForEdit(long colorId);
        Task<EditProductColorResult> EditProductColor(EditProductColorDto color, long colorId, string? modifierName);

        #endregion

        #region Product Feature

        Task<List<FilterProductFeatureDto>> GetAllProductFeaturesInAdminPanel(long productId);
        Task<CreateProductFeatureResult> CreateProductFeature(CreateProductFeatureDto feature, long productId, string? creatorName);

        #endregion

        #region Product Gallery

        Task<List<ProductGallery>> GetAllProductGalleries(long productId);
        Task<CreateOrEditProductGalleryResult> CreateProductGallery(CreateOrEditProductGalleryDto gallery, long productId, IFormFile galleryImage, string? creatorName);
        Task<CreateOrEditProductGalleryDto> GetProductGalleryForEdit(long galleryId);
        Task<CreateOrEditProductGalleryResult> EditProductGallery(CreateOrEditProductGalleryDto gallery, long  galleryId, IFormFile galleryImage, string? modifierName);

        #endregion
    }
}
