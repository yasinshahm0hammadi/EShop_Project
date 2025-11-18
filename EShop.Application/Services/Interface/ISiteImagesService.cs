using EShop.Domain.DTOs.Site.Banner;
using EShop.Domain.DTOs.Site.Silder;
using EShop.Domain.Entities.Site;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.Services.Interface
{
    public interface ISiteImagesService : IAsyncDisposable
    {
        #region Slider

        Task<List<Slider>> GetAllSlides();
        Task<List<Slider>> GetAllActiveSlides();
        Task<CreateSliderResult> CreateSlide(CreateSliderDto slide, IFormFile slideImage, IFormFile slideMobileImage, string? creatorName);
        Task<EditSliderDto> GetSlideForEdit(long id);
        Task<EditSliderResult> EditSlide(EditSliderDto silde, IFormFile slideImage, IFormFile slideMobileImage, string? editorName);
        Task<bool> ActivateSlide(long id, string? modifierName);
        Task<bool> DeActivateSlide(long id, string? modifierName);

        #endregion

        #region Site Banners

        Task<List<SiteBanner>> GetSiteBannersByPlacement(SiteBannerPlacement placement);
        Task<List<SiteBanner>> GetAllBanners();
        Task<CreateSiteBannerResult> CreateSiteBanner(CreateSiteBannerDto banner, IFormFile bannerImage, string? creatorName);
        Task<EditSiteBannerDto> GetSiteBannerForEdit(long id);
        Task<EditSiteBannerResult> EditSiteBanner(EditSiteBannerDto banner, IFormFile bannerImage, string? editorName);
        Task<bool> ActivateSiteBanner(long id, string? modifierName);
        Task<bool> DeActivateSiteBanner(long id, string? modifierName);

        #endregion
    }
}
