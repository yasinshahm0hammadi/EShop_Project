using System.ComponentModel.DataAnnotations;

namespace EShop.Domain.DTOs.Product.ProductGallery
{
    public class CreateOrEditProductGalleryDto
    {
        [Display(Name = "الویت نمایش")]
        public int? DisplayPriority { get; set; }

        [Display(Name = "تصویر گالری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string? Image { get; set; }
    }

    public enum CreateOrEditProductGalleryResult
    {
        Success,
        Error,
        NotForUserProduct,
        ImageIsNullOrInvalid,
        ProductNotFound
    }
}
