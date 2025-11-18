using EShop.Application.Services.Interface;
using EShop.Domain.DTOs.Contact;
using EShop.Domain.DTOs.Site;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Ctor

        private readonly IUserService _userService;
        private readonly ISiteService _siteService;
        private readonly IContactService _contactService;

        public HomeController(IUserService userService, ISiteService siteService, IContactService contactService)
        {
            _userService = userService;
            _siteService = siteService;
            _contactService = contactService;
        }

        #endregion

        #region Home

        [HttpGet("Home")]
        public async Task<IActionResult> AdminPanel()
        {
            return View();
        }

        #endregion

        #region Site Setting

        [HttpGet("site-setting")]
        public async Task<IActionResult> GetDefaultSiteSetting()
        {
            var setting = await _siteService.GetDefaultSiteSetting();
            return View(setting);
        }

        [HttpGet("site-setting/{settingId}")]
        public async Task<IActionResult> EditSiteSetting(long settingId)
        {
            var setting = await _siteService.GetSiteSettingForEdit(settingId);
            return View(setting);
        }

        [HttpPost("site-setting/{settingId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSiteSetting(EditSiteSettingDto setting)
        {
            var userName = await _userService.GetUserFullNameById(User.GetUserId());
            var result = await _siteService.EditSiteSetting(setting, userName);

            switch (result)
            {
                case EditSiteSettingResult.Success:
                    TempData[SuccessMessage] = "تغییرات با موفقیت اعمال شد.";
                    return RedirectToAction("GetDefaultSiteSetting", "Home", new { area = "Administration" });
                case EditSiteSettingResult.NotFound:
                    TempData[ErrorMessage] = "هیچ تنظیمات سایتی با این اطلاعات یافت نشد.";
                    break;
                case EditSiteSettingResult.Error:
                    TempData[ErrorMessage] = "در فرایند به روز رسانی تنظیمات سایت اختلالی پیش آمد، لطفا بعدا تلاش کنید.";
                    break;
            }
            return View(setting);
        }

        #endregion

        #region Contact Us

        [HttpGet("contact-messages")]
        public async Task<IActionResult> FilterContactMessages(FilterContactMessagesDto contactMessage)
        {
            var message = await _contactService.FilterContactMessages(contactMessage);
            return View(message);
        }

        #endregion

        #region About Us

        [HttpGet("AboutUs")]
        public async Task<IActionResult> GetAboutUs()
        {
            var about = await _siteService.GetAboutUs();
            return View(about);
        }

        [HttpGet("create-about-us")]
        public async Task<IActionResult> CreateAboutUs()
        {
            return View();
        }

        [HttpPost("create-about-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAboutUs(CreateAboutUsDto about)
        {
            if (ModelState.IsValid)
            {
                var creatorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _siteService.CreateAboutUs(about, creatorName);

                if (result == CreateAboutUsResult.Success)
                {
                    TempData[SuccessMessage] = "متن درباره ما جدید با موفقیت ایجاد شد.";
                    return RedirectToAction("GetAboutUs", "Home", new { area = "Administration" });
                }

                TempData[ErrorMessage] = "در فرایند ایجاد متن درباره ما جدید خطایی رخ داد، لطفا بعدا تلاش کنید.";
                return RedirectToAction("GetAboutUs", "Home", new { area = "Administration" });
            }
            return View(about);
        }

        [HttpGet("edit-about-us")]
        public async Task<IActionResult> EditAboutUs(long id)
        {
            var about = await _siteService.GetAboutUsForEdit(id);
            return View(about);
        }

        [HttpPost("edit-about-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(EditAboutUsDto about)
        {
            if (ModelState.IsValid)
            {
                var userName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _siteService.EditAboutUs(about, userName);

                switch (result)
                {
                    case EditAboutUsResult.Success:
                        TempData[SuccessMessage] = "متن درباره ما موردنظر با موفقیت ویرایش شد.";
                        return RedirectToAction("GetAboutUs", "Home", new { area = "Administration" });
                    case EditAboutUsResult.NotFound:
                        TempData[WarningMessage] = "متن درباره ما موردنظر یافت نشد.";
                        break;
                    case EditAboutUsResult.Error:
                        TempData[ErrorMessage] = "در فرایند ویرایش متن درباره ما موردنظر خطایی رخ داد، لطفا بعدا تلاش کنید.";
                        return RedirectToAction("GetAboutUs", "Home", new { area = "Administration" });
                }
            }
            return View(about);
        }



        #endregion
    }
}
