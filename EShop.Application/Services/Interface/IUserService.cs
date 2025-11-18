using EShop.Domain.DTOs.Account.Role;
using EShop.Domain.DTOs.Account.User;
using EShop.Domain.DTOs.Site;
using EShop.Domain.Entities.Account.Role;
using EShop.Domain.Entities.Account.User;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.Services.Interface;

public interface IUserService : IAsyncDisposable
{
    #region Account (User)

    #region User Validation

    Task<UserValidationResult> IsUserValidate(UserValidationDto validate);

    #endregion

    #region User register

    Task<UserRegisterResult> UserRegister(UserRegisterDto register);
    Task<bool> IsUserExistByMobile(string mobile);

    #endregion

    #region User Login

    Task<UserLoginResult> UserLogin(UserLoginDto login);
    Task<User> GetUserByMobile(string mobile);
    Task<User> GetUserById(long id);

    #endregion

    #region Activation Mobile

    Task<bool> ActivateMobile(ActivateMobileDto activateMobile);

    #endregion

    #region Restore User Password

    Task<ForgotPasswordResult> RestoreUserPassword(ForgotPasswordDto forgot);

    #endregion

    #region User Role

    Task<string> GetUserRole(long userId);

    #endregion

    #region Update User Profile

    Task<UpdateUserProfileDto> GetUserProfileForEdit(long userId);
    Task<UpdateUserProfileResult> EditUserProfile(UpdateUserProfileDto profile, long userId, IFormFile avatar, string? modifierName);

    #endregion

    #region Get User Profile

    Task<ReadUserProfileDto> GetUserProfile(long userId);

    #endregion

    #region Change User Password

    Task<ChangeUserPasswordResult> ChangeUserPassword(ChangeUserPasswordDto changePassword, long userId, string? modifierName);
    Task<string> GetUserFullNameById(long userId);
    Task<string> GetUserMobileById(long userId);

    #endregion

    #region User (CRUD)

    Task<FilterUserDto> FilterUsers(FilterUserDto user);
    Task<EditUserDto> GetUserForEdit(long userId);
    Task<EditUserResult> EditUser(EditUserDto user, string? modifierName);

    #endregion



    #endregion

    #region Role

    Task<FilterRoleDto> FilterRoles(FilterRoleDto role);
    Task<CreateRoleResult> CreateRole(CreateRoleDto role, string? creatorName);
    Task<EditRoleDto> GetRoleForEdit(long roleId);
    Task<EditRoleResult> EditRole(EditRoleDto role, string? modifierName);
    Task<List<Role>> GetRoles();

    #endregion
}