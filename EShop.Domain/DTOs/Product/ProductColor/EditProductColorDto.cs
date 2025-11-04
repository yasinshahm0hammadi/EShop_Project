using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Domain.DTOs.Product.ProductColor
{
    public class EditProductColorDto : CreateProductColorDto
    {
        public long Id { get; set; }
    }

    public enum EditProductColorResult
    {
        Error,
        ColorNotFound,
        DuplicateColor,
        Success,
    }
}
