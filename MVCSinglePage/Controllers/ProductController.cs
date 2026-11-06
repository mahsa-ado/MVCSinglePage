using MVCSinglePage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCSinglePage.ApplicationServices.Dtos.ProductDtos;
using MVCSinglePage.ApplicationServices.Services.Contracts;

public class ProductController : Controller
{
    private readonly IProductApplicationService _productApplicationService;
    #region [- ctor -]
    public ProductController(IProductApplicationService productApplicationService)
    {
        _productApplicationService = productApplicationService;
    }
    #endregion

    #region [- Detail() -]
    [HttpGet]
    public async Task<IActionResult> Detail(GetProductByIdDto getProductByIdDto)
    {
        var result = await _productApplicationService.GetProductDetail(getProductByIdDto);

        if (result == null)
        {
            return BadRequest(new{message = "The information could not be processed."});
        }

        return Json(result);
    }
    #endregion

    #region [- Edit() -]

    #region [- Post() -]
    [HttpPost]
    public async Task<IActionResult> Edit([FromBody]PutProductDto updateProductDto)
    {
        if (ModelState.IsValid)
        {
            await _productApplicationService.Put(updateProductDto);
            return Json(new {success=true,message= "Operation successful" });
        }
        else
        {
            return Json(new { success = false, message = "Operation failed" });

        }
    }
    #endregion

    #endregion

    #region [- Delete() -]

    #region [- Post() -]
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody]DeleteProductDto deleteProductDto)
    {
        if (deleteProductDto == null || deleteProductDto.Id == Guid.Empty)
        {
            return BadRequest(new { success = false, message = "Invalid product id" });
        }

        if (ModelState.IsValid)
        {
            var product = await _productApplicationService.GetProductDelete(
                new GetProductByIdDto { Id = deleteProductDto.Id });
            if (product == null)
            {
                return NotFound(new { success = false, message = "Product was not found" });
            }

            await _productApplicationService.Delete(product);

            return Json(new { success = true, message = "Product deleted successfully" });
        }
        else
        {
            return BadRequest(new { success = false, message = "Validation failed" });
        }
    }
    #endregion

    #endregion

    #region [- Create() -]

    #region [- Post() -]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]PostProductDto postProductDto)
    {
        if (ModelState.IsValid)
        {
            await _productApplicationService.Post(postProductDto);
            return Json(new { success = true, message = "Validation successful" });
        }
        else
        {
            return Json(new { success = false, message = "Validation failed" });
        }
    }
    #endregion
    #endregion

    #region [- Index() -]
    public async Task<IActionResult> Index()
    {
        var result = await _productApplicationService.GetAllProducts();
        return View();
            
    }
    #endregion

    #region [- GetAll() -]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _productApplicationService.GetAllProducts();
        return Json(result);
    } 
    #endregion
}
