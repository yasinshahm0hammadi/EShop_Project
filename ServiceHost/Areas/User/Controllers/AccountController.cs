using EShop.Application.Services.Interface;
using EShop.Domain.DTOs.Account.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region Constructor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Get User Profile

        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userInfo = await _userService.GetUserById(User.GetUserId());
            ViewBag.AvatarImage = userInfo.AvatarPath ?? string.Empty;
            var userProfile = await _userService.GetUserProfile(User.GetUserId());
            return View(userProfile);
        }

        #endregion

        #region Update User Profile

        [HttpGet("update-user-profile")]
        public async Task<IActionResult> EditUserProfile()
        {
            var user = await _userService.GetUserProfileForEdit(User.GetUserId());

            if (user == null)
            {
                return RedirectToAction("UserDashboard", "Home");
            }

            var userInfo = await _userService.GetUserById(User.GetUserId());
            ViewBag.AvatarImage = userInfo.AvatarPath ?? string.Empty;

            return View(user);
        }

        [HttpPost("update-user-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(UpdateUserProfileDto profile, IFormFile? avatar)
        {
            if (ModelState.IsValid)
            {
                var modifierName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _userService.EditUserProfile(profile, User.GetUserId(), avatar, modifierName);

                switch (result)
                {
                    case UpdateUserProfileResult.Success:
                        TempData[SuccessMessage] =
                            $"{profile.FirstName + ' ' + profile.LastName} عزیز پروفایل کاربری شما با موفقیت ویرایش شد.";
                        return RedirectToAction("GetUserProfile", "Account");
                    case UpdateUserProfileResult.Error:
                        TempData[ErrorMessage] =
                            "در فرایند ویرایش پروفایل کاربری شما خطایی رخ داد، لطفا مجددا تلاش کنید.";
                        break;
                    case UpdateUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با این مشخصات یافت نشد.";
                        break;
                }
            }

            return View(profile);
        }

        #endregion

        #region Change User Password

        [HttpGet("change-user-password")]
        public async Task<IActionResult> ChangeUserPassword()
        {
            return View();
        }

        [HttpPost("change-user-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserPassword(ChangeUserPasswordDto changePassword)
        {
            if (ModelState.IsValid)
            {
                var modifierName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _userService.ChangeUserPassword(changePassword, User.GetUserId(), modifierName);

                var userFullName = await _userService.GetUserFullNameById(User.GetUserId());
                var userMobile = await _userService.GetUserMobileById(User.GetUserId());

                switch (result)
                {
                    case ChangeUserPasswordResult.Success:
                        TempData[SuccessMessage] =
                            $"{userFullName} عزیز رمز عبور شما با موفقیت تغییر یافت.";
                        TempData[InfoMessage] = "برای ورود مجدد لطفا با رمز عبور جدید وارد شوید.";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("UserLogin", "Account", new { mobile = userMobile });
                    case ChangeUserPasswordResult.NotFound:
                        TempData[ErrorMessage] =
                            "کاربری با این مشخصات یافت نشد.";
                        break;
                    case ChangeUserPasswordResult.WrongCurrentPassword:
                        TempData[ErrorMessage] = "رمز عبور فعلی ورودی نادرست می باشد.";
                        break;
                    case ChangeUserPasswordResult.CurrentPasswordSameAsNew:
                        TempData[ErrorMessage] = "رمز عبور فعلی و رمز عبور جدید یکسان هستند.";
                        TempData[InfoMessage] = "لطفا رمز عبور جدید متفاوت از رمز عبور فعلی انتخاب کنید.";
                        break;
                    case ChangeUserPasswordResult.Error:
                        TempData[ErrorMessage] =
                            "در فرایند تغییر رمز عبور شما خطایی رخ داد، لطفا مجددا تلاش کنید.";
                        break;
                }
            }

            return View(changePassword);
        }

        #endregion
    }
}
