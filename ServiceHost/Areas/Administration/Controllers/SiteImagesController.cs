using EShop.Application.Services.Interface;
using EShop.Domain.DTOs.Site.Banner;
using EShop.Domain.DTOs.Site.Silder;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class SiteImagesController : AdminBaseController
    {
        #region Ctor

        private readonly ISiteImagesService _siteImagesService;
        private readonly IUserService _userService;

        public SiteImagesController(ISiteImagesService siteImagesService, IUserService userService)
        {
            _siteImagesService = siteImagesService;
            _userService = userService;
        }

        #endregion

        #region Slider

        #region Slides List

        [HttpGet("Slides")]
        public async Task<IActionResult> Slides()
        {
            var slides = await _siteImagesService.GetAllSlides();
            return View(slides);
        }

        #endregion

        #region Create Slide

        [HttpGet("CreateSlide")]
        public IActionResult CreateSlide()
        {
            return View();
        }

        [HttpPost("CreateSlide"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlide(CreateSliderDto slide, IFormFile? slideImage, IFormFile? slideMobileImage)
        {
            if (ModelState.IsValid)
            {
                var creatorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _siteImagesService.CreateSlide(slide, slideImage, slideMobileImage, creatorName);

                if (result == CreateSliderResult.Success)
                {
                    TempData[SuccessMessage] = "اسلاید جدید با موفقیت ایجاد شد";
                    return RedirectToAction("Slides", "SiteImages", new { area = "Administration" });
                }
                else
                {
                    TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
                }
            }
            return View();
        }

        #endregion

        #region Edit Slide

        [HttpGet("EditSlide/{slideId}")]
        public async Task<IActionResult> EditSlide(long slideId)
        {
            var slide = await _siteImagesService.GetSlideForEdit(slideId);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost("EditSlide/{slideId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlide(EditSliderDto slide, IFormFile? slideImage, IFormFile? slideMobileImage)
        {
            if (ModelState.IsValid)
            {
                var editorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _siteImagesService.EditSlide(slide, slideImage, slideMobileImage, editorName);

                switch (result)
                {
                    case EditSliderResult.Success:
                        TempData[SuccessMessage] = "اسلاید موردنظر با موفقیت ویرایش شد.";
                        return RedirectToAction("Slides", "SiteImages", new { area = "Administration" });
                    case EditSliderResult.Error:
                        TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
                        break;
                    case EditSliderResult.NotFound:
                        TempData[WarningMessage] = "متاسفانه اسلایدی با این مشخصات یافت نشد.";
                        break;
                }
            }

            return View(slide);
        }



        #endregion

        #region Activate / DeActivate Slide

        [HttpGet("ActivateSlide/{slideId}")]
        public async Task<IActionResult> ActivateSlide(long slideId)
        {
            var modifierName = await _userService.GetUserFullNameById(User.GetUserId());
            var result = await _siteImagesService.ActivateSlide(slideId, modifierName);
            if (result)
            {
                TempData[SuccessMessage] = "اسلاید موردنظر با موفقیت فعال شد.";
            }
            else
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
            }
            return RedirectToAction("Slides", "SiteImages", new { area = "Administration" });
        }

        [HttpGet("DeActivateSlide/{slideId}")]
        public async Task<IActionResult> DeActivateSlide(long slideId)
        {
            var modifierName = await _userService.GetUserFullNameById(User.GetUserId());
            var result = await _siteImagesService.DeActivateSlide(slideId, modifierName);
            if (result)
            {
                TempData[SuccessMessage] = "اسلاید موردنظر با موفقیت غیرفعال شد.";
            }
            else
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
            }
            return RedirectToAction("Slides", "SiteImages", new { area = "Administration" });
        }

        #endregion

        #endregion

        #region Site Banners

        #region Site Banners List

        [HttpGet("SiteBanners")]
        public async Task<IActionResult> SiteBanners()
        {
            var siteBanners = await _siteImagesService.GetAllBanners();
            return View(siteBanners);
        }

        #endregion

        #region Create Site Banner

        [HttpGet("CreateSiteBanner")]
        public IActionResult CreateSiteBanner()
        {
            return View();
        }

        [HttpPost("CreateSiteBanner"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSiteBanner(CreateSiteBannerDto banner, IFormFile? bannerImage)
        {
            if (ModelState.IsValid)
            {
                var creatorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _siteImagesService.CreateSiteBanner(banner, bannerImage, creatorName);

                if (result == CreateSiteBannerResult.Success)
                {
                    TempData[SuccessMessage] = "بنر جدید با موفقیت ایجاد شد";
                    return RedirectToAction("SiteBanners", "SiteImages", new { area = "Administration" });
                }
                else
                {
                    TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
                }
            }
            return View();
        }

        #endregion

        #region Edit Site Banner

        [HttpGet("EditSiteBanner/{bannerId}")]
        public async Task<IActionResult> EditSiteBanner(long bannerId)
        {
            var siteBanner = await _siteImagesService.GetSiteBannerForEdit(bannerId);
            if (siteBanner == null) return NotFound();
            return View(siteBanner);
        }

        [HttpPost("EditSiteBanner/{bannerId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSiteBanner(EditSiteBannerDto banner, IFormFile? bannerImage)
        {
            if (ModelState.IsValid)
            {
                var editorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _siteImagesService.EditSiteBanner(banner, bannerImage, editorName);

                switch (result)
                {
                    case EditSiteBannerResult.Success:
                        TempData[SuccessMessage] = "بنر موردنظر با موفقیت ویرایش شد.";
                        return RedirectToAction("SiteBanners", "SiteImages", new { area = "Administration" });
                    case EditSiteBannerResult.Error:
                        TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
                        break;
                    case EditSiteBannerResult.NotFound:
                        TempData[WarningMessage] = "متاسفانه بنری با این مشخصات یافت نشد.";
                        break;
                }
            }

            return View(banner);
        }

        #endregion

        #region Activate / DeActivate Site Banner

        [HttpGet("ActivateSitBanner/{bannerId}")]
        public async Task<IActionResult> ActivateSitBanner(long bannerId)
        {
            var modifierName = await _userService.GetUserFullNameById(User.GetUserId());
            var result = await _siteImagesService.ActivateSiteBanner(bannerId, modifierName);
            if (result)
            {
                TempData[SuccessMessage] = "بنر موردنظر با موفقیت فعال شد.";
            }
            else
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
            }
            return RedirectToAction("SiteBanners", "SiteImages", new { area = "Administration" });
        }

        [HttpGet("DeActivateSiteBanner/{bannerId}")]
        public async Task<IActionResult> DeActivateSiteBanner(long bannerId)
        {
            var modifierName = await _userService.GetUserFullNameById(User.GetUserId());
            var result = await _siteImagesService.DeActivateSiteBanner(bannerId, modifierName);
            if (result)
            {
                TempData[SuccessMessage] = "بنر موردنظر با موفقیت غیرفعال شد.";
            }
            else
            {
                TempData[ErrorMessage] = "عملیات با خطا مواجه شد، لطفا مجددا تلاش کنید";
            }
            return RedirectToAction("SiteBanners", "SiteImages", new { area = "Administration" });
        }

        #endregion

        #endregion
    }
}
