using EShop.Application.Services.Interface;
using EShop.Domain.Entities.Site;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    #region Site Header

    public class SiteHeaderViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteHeader");
        }
    }

    #endregion

    #region Mega Menu

    public class MegaMenuViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public MegaMenuViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _productService.GetAllActiveProductCategories();
            return View("MegaMenu", categories);
        }
    }

    #endregion

    #region Site Footer

    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var siteSetting = await _siteService.GetDefaultSiteSetting();
            return View("SiteFooter", siteSetting);
        }
    }

    #endregion

    #region AboutUs > Features

    public class FeaturesViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public FeaturesViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var feature = await _siteService.GetAllFeatures();
            return View("Features", feature);
        }
    }

    #endregion

    #region AboutUs > Questions

    public class QuestionsViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public QuestionsViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var question = await _siteService.GetAllQuestions();
            return View("Questions", question);
        }
    }

    #endregion

    #region Slider

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public HomeSliderViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slides = await _siteImagesService.GetAllActiveSlides();
            return View("HomeSlider", slides);
        }
    }


    #endregion

    #region Site Banner

    public class SiteBannerViewComponent : ViewComponent
    {
        private readonly ISiteImagesService _siteImagesService;

        public SiteBannerViewComponent(ISiteImagesService siteImagesService)
        {
            _siteImagesService = siteImagesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(SiteBannerPlacement placement)
        {
            var banners = await _siteImagesService.GetSiteBannersByPlacement(placement);
            return View("SiteBanner", banners);
        }
    }

    #endregion

    #region Latest Arrival Products Section

    public class LatestArrivalProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public LatestArrivalProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestArrivalProducts = await _productService.GetLatestArrivalProducts(15);
            return View("LatestArrivalProducts", latestArrivalProducts);
        }
    }

    #endregion
}
