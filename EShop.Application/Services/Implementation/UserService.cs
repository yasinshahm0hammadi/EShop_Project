using EShop.Application.Extensions;
using EShop.Application.Services.Interface;
using EShop.Application.Utilities;
using EShop.Domain.DTOs.Account.Role;
using EShop.Domain.DTOs.Account.User;
using EShop.Domain.DTOs.Paging;
using EShop.Domain.Entities.Account.Role;
using EShop.Domain.Entities.Account.User;
using EShop.Domain.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace EShop.Application.Services.Implementation;

public class UserService : IUserService
{
    #region Constructor

    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly ISmsService _smsService;
    private readonly IRoleService _roleService;

    public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository,
        ISmsService smsService, IRoleService roleService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _smsService = smsService;
        _roleService = roleService;
    }

    #endregion

    #region Services

    #region Account

    #region User Validation
    public async Task<UserValidationResult> IsUserValidate(UserValidationDto validate)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Mobile == validate.Mobile);

            if (user != null)
            {
                if (user.IsMobileActive)
                {
                    return UserValidationResult.ExistAndActive;
                }

                await _smsService.SendVerificationSms(validate.Mobile, user.MobileActiveCode);


                return UserValidationResult.ExistAndNotActive;
            }

            return UserValidationResult.NotExists;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return UserValidationResult.Error;
        }
    }

    #endregion

    #region User Register

    public async Task<UserRegisterResult> UserRegister(UserRegisterDto register)
    {
        try
        {
            if (!await IsUserExistByMobile(register.Mobile))
            {
                var salt = PasswordManager.GenerateSalt(16);

                var newUser = new User
                {
                    Mobile = register.Mobile,
                    MobileActiveCode = new Random().Next(100000, 999999).ToString(),
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Salt = salt,
                    Password = PasswordManager.HashPassword(register.Password, salt),
                    RoleId = 2
                };

                await _smsService.SendVerificationSms(register.Mobile, newUser.MobileActiveCode);

                await _userRepository.AddEntity(newUser, null);
                await _userRepository.SaveChanges();

                return UserRegisterResult.Success;
            }

            return UserRegisterResult.MobileExists;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return UserRegisterResult.Error;
        }
    }
    public async Task<bool> IsUserExistByMobile(string mobile)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Mobile == mobile);

            if (user != null)
            {
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

    #region User Login

    public async Task<UserLoginResult> UserLogin(UserLoginDto login)
    {
        try
        {
            var user = await _userRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Mobile == login.Mobile && !x.IsBlocked && !x.IsPublished);

            if (await IsUserExistByMobile(login.Mobile))
            {
                if (!user.IsMobileActive)
                {
                    await _smsService.SendVerificationSms(login.Mobile, user.MobileActiveCode);

                    return UserLoginResult.MobileNotActivated;
                }

                if (user.Password == PasswordManager.HashPassword(login.Password, user.Salt))
                {
                    return UserLoginResult.Success;
                }

                return UserLoginResult.WrongPassword;
            }

            return UserLoginResult.UserNotFound;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return UserLoginResult.Error;
        }
    }
    public async Task<User> GetUserByMobile(string mobile)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Mobile == mobile);

            return user;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return null;
        }
    }
    public async Task<User> GetUserById(long id)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Id == id);

            return user;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return null;
        }
    }

    #endregion

    #region Activation Mobile

    public async Task<bool> ActivateMobile(ActivateMobileDto activateMobile)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Mobile == activateMobile.Mobile);

            if (user != null)
            {
                if (activateMobile.MobileActiveCode == user.MobileActiveCode)
                {
                    user.IsMobileActive = true;
                    user.MobileActiveCode = new Random().Next(100000, 9999999).ToString();
                    await _userRepository.SaveChanges();

                    return true;
                }
            }

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    #endregion

    #region Restore User Password

    public async Task<ForgotPasswordResult> RestoreUserPassword(ForgotPasswordDto forgot)
    {
        try
        {
            if (await IsUserExistByMobile(forgot.Mobile))
            {
                var user = await _userRepository
                    .GetQuery()
                    .SingleOrDefaultAsync(x => x.Mobile == forgot.Mobile);

                if (user == null)
                {
                    return ForgotPasswordResult.UserNotFound;
                }

                var newPassword = PasswordManager.CreateRandomPassword();
                user.Password = PasswordManager.HashPassword(newPassword, user.Salt);

                _userRepository.EditEntity(user, null);

                await _smsService.SendRestorePasswordSms(forgot.Mobile, newPassword);

                await _userRepository.SaveChanges();

                return ForgotPasswordResult.Success;
            }

            return ForgotPasswordResult.UserNotFound;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return ForgotPasswordResult.Error;
        }
    }

    #endregion

    #region Get User Profile

    public async Task<ReadUserProfileDto> GetUserProfile(long userId)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Id == userId && !x.IsPublished);

            if (user != null)
            {
                return new ReadUserProfileDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    Email = user.Email,
                    AvatarPath = user.AvatarPath,
                    RegisterDate = user.CreatedAt.ToString()
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return null;
        }
    }

    #endregion

    #region Update User Profile

    public async Task<UpdateUserProfileDto> GetUserProfileForEdit(long userId)
    {
        try
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user != null)
            {
                return new UpdateUserProfileDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    Email = user.Email,
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return null;
        }
    }
    public async Task<UpdateUserProfileResult> EditUserProfile(UpdateUserProfileDto profile, long userId, IFormFile? avatar, string? modifierName)
    {
        try
        {
            var user = await _userRepository.GetEntityById(userId);

            if (user == null)
            {
                return UpdateUserProfileResult.NotFound;
            }

            user.Edit(profile.FirstName, profile.LastName, profile.Email, profile.Mobile);

            if (avatar != null && avatar.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatar.FileName);

                avatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer,
                    100, 100, PathExtension.UserAvatarThumbServer, user.AvatarPath);

                user.AvatarPath = imageName;
            }

            _userRepository.EditEntity(user, modifierName);
            await _userRepository.SaveChanges();

            return UpdateUserProfileResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return UpdateUserProfileResult.Error;
        }
    }

    #endregion

    #region Change User Password

    public async Task<ChangeUserPasswordResult> ChangeUserPassword(ChangeUserPasswordDto changePassword, long userId, string? modifierName)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Id == userId && !x.IsBlocked && !x.IsPublished);

            if (user == null)
            {
                return ChangeUserPasswordResult.NotFound;
            }

            if (user.Password != PasswordManager.HashPassword(changePassword.CurrentPassword, user.Salt))
            {
                return ChangeUserPasswordResult.WrongCurrentPassword;
            }

            if (changePassword.NewPassword == user.Password)
            {
                return ChangeUserPasswordResult.CurrentPasswordSameAsNew;
            }

            user.Password = PasswordManager.HashPassword(changePassword.NewPassword, user.Salt);

            _userRepository.EditEntity(user, modifierName);
            await _userRepository.SaveChanges();

            return ChangeUserPasswordResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return ChangeUserPasswordResult.Error;
        }
    }
    public async Task<string> GetUserFullNameById(long userId)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Id == userId && !x.IsBlocked && !x.IsPublished);

            if (user != null)
            {
                return (user.FirstName + " " + user.LastName);
            }

            return "Not Found";
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return "Not Found";
        }
    }
    public async Task<string> GetUserMobileById(long userId)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .AsQueryable()
            .SingleOrDefaultAsync(x => x.Id == userId && !x.IsBlocked && !x.IsPublished);

            if (user != null)
            {
                return user.Mobile;
            }

            return null;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return null;
        }
    }

    #endregion

    #region User (CRUD)

    public async Task<FilterUserDto> FilterUsers(FilterUserDto user)
    {
        try
        {
            var query = _userRepository
            .GetQuery()
            .Include(x => x.Role)
            .AsQueryable();

            #region Filter

            if (user.RoleId > 0)
            {
                query = query.Where(x => x.RoleId == user.RoleId);
            }
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{user.FirstName}%"));
            }
            if (!string.IsNullOrEmpty(user.LastName))
            {
                query = query.Where(x => EF.Functions.Like(x.LastName, $"%{user.LastName}%"));
            }
            if (!string.IsNullOrEmpty(user.Mobile))
            {
                query = query.Where(x => EF.Functions.Like(x.Mobile, $"%{user.Mobile}%"));
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{user.Email}%"));
            }

            #endregion

            #region Paging

            var userCount = await query.CountAsync();

            var pager = Pager.Build(user.PageId, userCount, user.TakeEntity,
                user.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();

            #endregion

            return user.SetPaging(pager).SetUsers(allEntities);
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new FilterUserDto();
        }
    }
    public async Task<EditUserDto> GetUserForEdit(long userId)
    {
        try
        {
            var existingUser = await _userRepository
            .GetQuery()
            .Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.Id == userId);

            if (existingUser != null)
            {
                return new EditUserDto
                {
                    Id = existingUser.Id,
                    RoleId = existingUser.Role.Id,
                    FirstName = existingUser.FirstName,
                    LastName = existingUser.LastName,
                    Mobile = existingUser.Mobile,
                    Email = existingUser.Email,
                    IsMobileActivated = existingUser.IsMobileActive,
                    IsBlocked = existingUser.IsBlocked
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new EditUserDto();
        }
    }
    public async Task<EditUserResult> EditUser(EditUserDto user, string? modifierName)
    {
        try
        {
            var existingUser = await _userRepository
                .GetQuery()
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Id == user.Id);

            if (existingUser != null)
            {
                existingUser.RoleId = user.RoleId;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Mobile = user.Mobile;
                existingUser.Email = user.Email;
                existingUser.IsMobileActive = user.IsMobileActivated;
                existingUser.LastModifiedAt = DateTime.Now;


                _userRepository.EditEntity(existingUser, modifierName);
                await _userRepository.SaveChanges();

                return EditUserResult.Success;
            }

            return EditUserResult.UserNotFound;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return EditUserResult.Error;
        }
    }

    #endregion

    #endregion

    #region User Role

    public async Task<string> GetUserRole(long userId)
    {
        try
        {
            var user = await _userRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                return await _roleService.GetRoleNameByRoleId(user.RoleId);
            }

            return "Not Found";
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return "Not Found";
        }
    }
    public async Task<FilterRoleDto> FilterRoles(FilterRoleDto role)
    {
        try
        {
            var query = _roleRepository
            .GetQuery()
            .Include(x => x.Users);

            #region Filter

            if (Equals(!string.IsNullOrEmpty(role.RoleName)))
            {
                query.Where(x => EF.Functions.Like(x.RoleName, $"%{role.RoleName}%"));
            }

            #endregion

            #region Paging

            var roleCount = await query.CountAsync();

            var pager = Pager.Build(role.PageId, roleCount, role.TakeEntity,
                role.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return role.SetPaging(pager).SetRoles(allEntities);
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new FilterRoleDto();
        }
    }
    public async Task<CreateRoleResult> CreateRole(CreateRoleDto role, string? creatorName)
    {
        try
        {
            var newRole = new Role
            {
                RoleName = role.RoleName
            };

            await _roleRepository.AddEntity(newRole, creatorName);
            await _roleRepository.SaveChanges();

            return CreateRoleResult.Success;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return CreateRoleResult.Error;
        }
    }
    public async Task<EditRoleDto> GetRoleForEdit(long roleId)
    {
        try
        {
            var existingRole = await _roleRepository
            .GetQuery()
            .SingleOrDefaultAsync(x => x.Id == roleId);

            if (existingRole != null)
            {
                return new EditRoleDto
                {
                    Id = existingRole.Id,
                    RoleName = existingRole.RoleName
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new EditRoleDto();
        }
    }
    public async Task<EditRoleResult> EditRole(EditRoleDto role, string? modifier)
    {
        try
        {
            var existingRole = await _roleRepository
                .GetQuery()
                .SingleOrDefaultAsync(x => x.Id == role.Id);

            if (existingRole != null)
            {
                existingRole.RoleName = role.RoleName;
                existingRole.LastModifiedAt = DateTime.Now;

                _roleRepository.EditEntity(existingRole, modifier);
                await _roleRepository.SaveChanges();

                return EditRoleResult.Success;
            }

            return EditRoleResult.NotFound;
        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return EditRoleResult.Error;
        }
    }
    public async Task<List<Role>> GetRoles()
    {
        try
        {
            return await _roleRepository
            .GetQuery()
            .Select(x => new Role
            {
                Id = x.Id,
                RoleName = x.RoleName
            }).ToListAsync();

        }
        catch (Exception ex)
        {
            Logger.ShowError(ex);

            return new List<Role>();
        }
    }

    #endregion

    #endregion

    #region Dispose

    public async ValueTask DisposeAsync() { }

    #endregion
}

