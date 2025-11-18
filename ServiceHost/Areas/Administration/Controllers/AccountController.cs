using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.DTOs.Account.Role;
using EShop.Domain.DTOs.Account.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceHost.PresentationExtensions;

namespace ServiceHost.Areas.Administration.Controllers
{
    [Authorize("UserManagement", Roles = Roles.Administrator)]
    public class AccountController : AdminBaseController
    {
        #region Ctor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region User

        #region Filter User

        [HttpGet("FilterUsers")]
        public async Task<IActionResult> FilterUsers(FilterUserDto user)
        {
            var users = await _userService.FilterUsers(user);

            users.Roles = await _userService.GetRoles();
            ViewBag.Roles = users.Roles;

            return View(users);
        }

        #endregion

        #region Edit User

        [HttpGet("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(long userId)
        {
            var user = await _userService.GetUserForEdit(userId);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.roles = await _userService.GetRoles();

            return View(user);
        }

        [HttpPost("EditUser/{userId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserDto user)
        {
            if (ModelState.IsValid)
            {
                var editorName = await _userService.GetUserFullNameById(User.GetUserId());

                var result = await _userService.EditUser(user, editorName);

                switch (result)
                {
                    case EditUserResult.Success:
                        TempData[SuccessMessage] = "پروفایل کاربر موردنظر با موفقیت ویرایش شد.";
                        return RedirectToAction("FilterUsers", "Account", new { area = "Administration" });
                    case EditUserResult.UserNotFound:
                        TempData[WarningMessage] = "کاربر موردنظر یافت نشد.";
                        break;
                    case EditUserResult.Error:
                        TempData[ErrorMessage] = "در فرایند ویرایش کاربر خطایی رخ داد، لطفا بعدا تلاش کنید";
                        break;
                }
            }

            ViewBag.roles = await _userService.GetRoles();
            return View(user);
        }

        #endregion

        #endregion

        #region Role

        #region Filter Roles

        [HttpGet("FilterRoles")]
        public async Task<IActionResult> FilterRoles(FilterRoleDto roles)
        {
            var allRoles = await _userService.FilterRoles(roles);
            return View(allRoles);
        }

        #endregion

        #region Create Role

        [HttpGet("CreateRole")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost("CreateRole"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDto role)
        {
            if (ModelState.IsValid)
            {
                var creatorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _userService.CreateRole(role, creatorName);

                switch (result)
                {
                    case CreateRoleResult.Success:
                        TempData[SuccessMessage] = "نقش جدید با موفقیت ایجاد شد.";
                        return RedirectToAction("", "", new { area = "Administration" });
                    case CreateRoleResult.Error:
                        TempData[ErrorMessage] = "در فرایند ایجاد نقش جدید خطایی رخ داد، لطفا بعدا تلاش کنید.";
                        break;
                }
            }
            return View();
        }

        #endregion

        #region Edit Role

        [HttpGet("EditRole/{roleId}")]
        public async Task<IActionResult> EditRole(long roleId)
        {
            var role = await _userService.GetRoleForEdit(roleId);
            return View(role);
        }

        [HttpPost("EditRole/{roleId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(EditRoleDto role)
        {
            if (ModelState.IsValid)
            {
                var editorName = await _userService.GetUserFullNameById(User.GetUserId());
                var result = await _userService.EditRole(role, editorName);

                switch (result)
                {
                    case EditRoleResult.Success:
                        TempData[SuccessMessage] = "نقش موردنظر با ویرایش ایجاد شد.";
                        return RedirectToAction("FilterRoles", "Account", new { area = "Administration" });
                    case EditRoleResult.NotFound:
                        TempData[WarningMessage] = "نقش موردنظر یافت نشد";
                        break;
                    case EditRoleResult.Error:
                        TempData[ErrorMessage] = "در فرایند ویرایش نقش موردنظر خطایی رخ داد، لطفا بعدا تلاش کنید.";
                        break;
                }
            }
            return View(role);
        }

        #endregion

        #endregion

        #region Account

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
