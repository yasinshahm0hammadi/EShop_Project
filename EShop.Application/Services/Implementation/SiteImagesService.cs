using EShop.Application.Extensions;
using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.Entities.Site;
using EShop.Domain.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using EShop.Domain.DTOs.Site.Banner;
using EShop.Domain.DTOs.Site.Silder;
using System.Net.Http.Headers;

namespace EShop.Application.Services.Implementation
{
    public class SiteImagesService : ISiteImagesService
    {
        #region Ctor

        private readonly IGenericRepository<Slider> _sliderRepository;
        private readonly IGenericRepository<SiteBanner> _siteBannerRepository;

        public SiteImagesService(IGenericRepository<Slider> sliderRepository, IGenericRepository<SiteBanner> siteBannerRepository)
        {
            _sliderRepository = sliderRepository;
            _siteBannerRepository = siteBannerRepository;
        }

        #endregion

        #region Silder

        public async Task<List<Slider>> GetAllSlides()
        {
            try
            {
                return await _sliderRepository
                .GetQuery()
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<Slider>();
            }
        }
        public async Task<List<Slider>> GetAllActiveSlides()
        {
            try
            {
                return await _sliderRepository
                .GetQuery()
                .Where(x => x.IsActive && !x.IsDelete)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<Slider>();
            }
        }
        public async Task<CreateSliderResult> CreateSlide(CreateSliderDto slide, IFormFile slideImage, IFormFile? slideMobileImage)
        {
            try
            {
                string imageName = null;
                if (slideImage != null)
                {
                    if (slideImage.IsImage())
                    {
                        imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(slideImage.FileName);
                        slideImage.AddImageToServer(imageName, PathExtension.SliderOriginServer,
                            100, 100, PathExtension.SliderThumbServer);
                    }
                }

                string mobileImageName = null;
                if (slideMobileImage != null)
                {
                    if (slideMobileImage.IsImage())
                    {
                        mobileImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(slideMobileImage.FileName);
                        slideImage.AddImageToServer(mobileImageName, PathExtension.MobileSliderOriginServer,
                            100, 100, PathExtension.MobileSliderThumbServer);
                    }
                }

                var newSilde = new Slider
                {
                    ImageName = imageName,
                    MobileImageName = mobileImageName,
                    Link = slide.Link,
                    Description = slide.Description,
                    IsActive = slide.IsActive,
                    IsDelete = false,
                    CreateDate = DateTime.Now
                };

                await _sliderRepository.AddEntity(newSilde);
                await _sliderRepository.SaveChanges();

                return CreateSliderResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return CreateSliderResult.Error;
            }
        }
        public async Task<EditSliderDto> GetSlideForEdit(long id)
        {
            try
            {
                var slide = await _sliderRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                if (slide == null) return null;

                return new EditSliderDto
                {
                    Id = slide.Id,
                    Link = slide.Link,
                    Description = slide.Description,
                    IsActive = slide.IsActive,
                };
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new EditSliderDto();
            }
        }
        public async Task<EditSliderResult> EditSlide(EditSliderDto slide, IFormFile slideImage, IFormFile? slideMobileImage, string editorName)
        {
            try
            {
                var Slide = _sliderRepository
                    .GetQuery()
                    .SingleOrDefault(x => x.Id == slide.Id);

                if (slideImage != null)
                {
                    if (slideImage.IsImage())
                    {
                        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(slideImage.FileName);
                        slideImage.AddImageToServer(imageName, PathExtension.SliderOriginServer,
                            100, 100, PathExtension.SliderThumbServer);

                        Slide.ImageName = imageName;
                    }
                }

                if (slideMobileImage != null)
                {
                    if (slideMobileImage.IsImage())
                    {
                        var mobileImageName = Guid.NewGuid().ToString("N") + Path.GetExtension(slideMobileImage.FileName);
                        slideImage.AddImageToServer(mobileImageName, PathExtension.MobileSliderOriginServer,
                            100, 100, PathExtension.MobileSliderThumbServer);

                        Slide.MobileImageName = mobileImageName;
                    }
                }

                Slide.Link = slide.Link;
                Slide.Description = slide.Description;
                Slide.IsActive = slide.IsActive;
                Slide.LastUpdateDate = DateTime.Now;
                
                

                _sliderRepository.EditEntityByEditor(Slide, editorName);
                await _sliderRepository.SaveChanges();

                return EditSliderResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return EditSliderResult.Error;
            }
        }
        public async Task<bool> ActivateSlide(long id)
        {
            try
            {
                var slide = _sliderRepository
                .GetQuery()
                .SingleOrDefault(x => x.Id == id);

                slide.IsActive = true;
                slide.IsDelete = false;
                slide.LastUpdateDate = DateTime.Now;

                _sliderRepository.EditEntity(slide);
                await _sliderRepository.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return false;
            }
        }
        public async Task<bool> DeActivateSlide(long id)
        {
            try
            {
                var slide = _sliderRepository
                .GetQuery()
                .SingleOrDefault(x => x.Id == id);

                slide.IsActive = false;
                slide.IsDelete = true;
                slide.LastUpdateDate = DateTime.Now;

                _sliderRepository.EditEntity(slide);
                await _sliderRepository.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return false;
            }
        }

        #endregion

        #region Site Banners

        public async Task<List<SiteBanner>> GetSiteBannersByPlacement(SiteBannerPlacement placement)
        {
            try
            {
                return await _siteBannerRepository
                .GetQuery()
                .Where(x => x.Placement == placement && !x.IsDelete)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<SiteBanner>();
            }
        }
        public async Task<List<SiteBanner>> GetAllBanners()
        {
            try
            {
                return await _siteBannerRepository
                .GetQuery()
                .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new List<SiteBanner>();
            }
        }
        public async Task<CreateSiteBannerResult> CreateSiteBanner(CreateSiteBannerDto banner, IFormFile bannerImage)
        {
            try
            {
                string BannerImage = null;
                if (bannerImage != null)
                {
                    if (bannerImage.IsImage())
                    {
                        BannerImage = Guid.NewGuid().ToString("N") + Path.GetExtension(bannerImage.FileName);
                        bannerImage.AddImageToServer(BannerImage, PathExtension.SiteBannerOriginServer,
                            100, 100, PathExtension.SiteBannerThumbServer);
                    }
                }

                var newSiteBanner = new SiteBanner
                {
                    ImageName = BannerImage,
                    Placement = banner.Placement,
                    Link = banner.Link,
                    Description = banner.Description,
                    GridColumnSize = banner.GridColumnSize
                };

                await _siteBannerRepository.AddEntity(newSiteBanner);
                await _siteBannerRepository.SaveChanges();

                return CreateSiteBannerResult.Success;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return CreateSiteBannerResult.Error;
            }
        }
        public async Task<EditSiteBannerDto> GetSiteBannerForEdit(long id)
        {
            try
            {
                var existingSiteBanner = await _siteBannerRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                if (existingSiteBanner != null)
                {
                    return new EditSiteBannerDto
                    {
                        Id = existingSiteBanner.Id,
                        Placement = existingSiteBanner.Placement,
                        Description = existingSiteBanner.Description,
                        Link = existingSiteBanner.Link,
                        GridColumnSize = existingSiteBanner.GridColumnSize
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return new EditSiteBannerDto();
            }
        }
        public async Task<EditSiteBannerResult> EditSiteBanner(EditSiteBannerDto banner, IFormFile bannerImage, string editorName)
        {
            try
            {
                var existingSiteBanner = await _siteBannerRepository.GetQuery()
                    .SingleOrDefaultAsync(x => x.Id == banner.Id);

                if (existingSiteBanner != null)
                {
                    if (bannerImage != null)
                    {
                        if (bannerImage.IsImage())
                        {
                            var BannerImage = Guid.NewGuid().ToString("N") + Path.GetExtension(bannerImage.FileName);
                            bannerImage.AddImageToServer(BannerImage, PathExtension.SiteBannerOriginServer,
                                100, 100, PathExtension.SiteBannerThumbServer);

                            existingSiteBanner.ImageName = BannerImage;
                        }
                    }

                    existingSiteBanner.Placement = banner.Placement;
                    existingSiteBanner.Description = banner.Description;
                    existingSiteBanner.Link = banner.Link;
                    existingSiteBanner.GridColumnSize = banner.GridColumnSize;
                    existingSiteBanner.LastUpdateDate = DateTime.Now;

                    _siteBannerRepository.EditEntityByEditor(existingSiteBanner, editorName);
                    await _siteBannerRepository.SaveChanges();

                    return EditSiteBannerResult.Success;
                }

                return EditSiteBannerResult.NotFound;
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);

                return EditSiteBannerResult.Error;
            }
        }
        public async Task<bool> ActivateSiteBanner(long id)
        {
            try
            {
                var siteBanner = await _siteBannerRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                if (siteBanner != null)
                {
                    siteBanner.IsDelete = false;

                    _siteBannerRepository.EditEntity(siteBanner);
                    await _siteBannerRepository.SaveChanges();

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
        public async Task<bool> DeActivateSiteBanner(long id)
        {
            try
            {
                var siteBanner = await _siteBannerRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == id);

                if (siteBanner != null)
                {
                    siteBanner.IsDelete = true;

                    _siteBannerRepository.EditEntity(siteBanner);
                    await _siteBannerRepository.SaveChanges();

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

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (_sliderRepository != null)
            {
                await _sliderRepository.DisposeAsync();
            }
        }

        #endregion
    }
}
