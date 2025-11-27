using System.ComponentModel.DataAnnotations;
using EShop.Domain.DTOs.Paging;

namespace EShop.Domain.DTOs.Product
{
    public class FilterProductDto : BasePaging
    {
        #region Constructor

        public FilterProductDto()
        {
            OrderBy = FilterProductOrderBy.CreateDateDescending;
        }

        #endregion

        #region Properties

        public long ProductId { get; set; }
        public string Search { get; set; }
        public string ProductTitle { get; set; }
        public string ProductCode { get; set; }
        public string ProductBrand { get; set; }
        public string Category { get; set; }
        public int FilterMinPrice { get; set; }
        public int FilterMaxPrice { get; set; }
        public int? SelectedMinPrice { get; set; }
        public int? SelectedMaxPrice { get; set; }
        public int MobileSelectedMinPrice { get; set; }
        public int MobileSelectedMaxPrice { get; set; }
        public List<Entities.Product.Product> Products { get; set; }
        public FilterProductState ProductState { get; set; }
        public FilterProductOrderBy OrderBy { get; set; }
        public FilterProductOrder ProductOrder { get; set; }
        public List<long> SelectedProductCategories { get; set; }
        public List<Entities.Product.ProductCategory> ProductCategories { get; set; }

        #endregion

        #region Methods

        public FilterProductDto SetProduct(List<Entities.Product.Product> products)
        {
            Products = products;
            return this;
        }

        public FilterProductDto SetPaging(BasePaging paging)
        {
            PageId = paging.PageId;
            AllEntitiesCount = paging.AllEntitiesCount;
            StartPage = paging.StartPage;
            EndPage = paging.EndPage;
            HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            TakeEntity = paging.TakeEntity;
            SkipEntity = paging.SkipEntity;
            PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }


    public enum FilterProductState
    {
        [Display(Name = "همه")]
        All,

        [Display(Name = "فعال")]
        Active,

        [Display(Name = "غیرفعال")]
        NotActive
    }

    public enum FilterProductOrder
    {
        CreateDateDescending,
        CreateDateAscending,
    }

    public enum FilterProductOrderBy 
    {
        [Display(Name = "جدیدترین")]
        CreateDateDescending,

        [Display(Name = "قدیمی ترین")]
        CreateDateAscending,

        [Display(Name = "ارزانترین")]
        PriceAscending,

        [Display(Name = "گرانترین")]
        PriceDescending,

        [Display(Name = "پر بازدیدترین")]
        ViewCountDescending,

        [Display(Name = "پر فروشترین")]
        SellCountDescending,
    }
}
