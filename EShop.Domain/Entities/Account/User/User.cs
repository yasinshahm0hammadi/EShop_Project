using System.ComponentModel.DataAnnotations;
using EShop.Domain.Entities.Common;
using EShop.Domain.Entities.Contact;

namespace EShop.Domain.Entities.Account.User;

public class User : BaseEntity
{
    #region Properties

    public long RoleId { get; set; }

    [Display(Name = "نام")]
    [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string FirstName { get; set; }

    [Display(Name = "نام خانوادگی")]
    [MaxLength(250, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string LastName { get; set; }

    [Display(Name = "تلفن همراه")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    [MinLength(11, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "فقط اعداد مجاز می باشد")]
    public required string Mobile { get; set; }

    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string MobileActiveCode { get; set; }

    [Display(Name = "موبایل فعال / غیرفعال")]
    public bool IsMobileActive { get; set; }

    [Display(Name = "ایمیل")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمی باشد")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Display(Name = "کد فعالسازی ایمیل")]
    [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? EmailActiveCode { get; set; }

    [Display(Name = "ایمیل فعال / غیرفعال")]
    public bool? IsEmailActive { get; set; }

    [Display(Name = "تائید دو مرحله ای فعال / غیرفعال")]
    public bool? TwoFactorAuthentication { get; set; }

    [Display(Name = "کلمه ی عبور")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد")]
    public string Password { get; set; }

    [Display(Name = "نمک")]
    [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
    public string Salt { get; set; }

    [Display(Name = "تصویر آواتار")]
    [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
    public string? AvatarPath { get; set; }

    [Display(Name = "بلاک شده / نشده")]
    public bool IsBlocked { get; set; }


    #endregion

    public void Edit(string firstName, string lastName, string? email, string mobile)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Mobile = mobile;
    }

    #region Relations

    public Role.Role Role { get; set; }
    public ICollection<ContactUs> Contacts { get; set; }

    #endregion
}