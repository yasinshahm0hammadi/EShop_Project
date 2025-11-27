using EShop.Application.Services.Interface;
using EShop.Domain.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    public class ProductController : SiteBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        [HttpGet("products/{category}")]
        public async Task<IActionResult> FilterProducts(FilterProductDto filterProduct)
        {
            var products = await _productService.FilterProducts(filterProduct);

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            return View(products);
        }
    }
}
