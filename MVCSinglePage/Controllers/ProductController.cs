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
            return NotFound();
        }

        return View(result);
    }
    #endregion

    #region [- Edit() -]

    #region [- Get() -]
    [HttpGet]
    public async Task<IActionResult> Edit(GetProductByIdDto getProductDtoById)
    {
        var product = await _productApplicationService.GetProductById(getProductDtoById);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    #endregion

    #region [- Post() -]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PutProductDto updateProductDto)
    {
        if (ModelState.IsValid)
        {
            await _productApplicationService.Put(updateProductDto);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(updateProductDto);
        }
    }
    #endregion

    #endregion

    #region [- Delete() -]

    #region [- Get() -]
    [HttpGet]
    public async Task<IActionResult> Delete(GetProductByIdDto getProductDtoById)
    {
        var product = await _productApplicationService.GetProductDelete(getProductDtoById);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);

    }
    #endregion

    #region [- Post() -]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeleteProductDto deleteProductDto)
    {
        if (ModelState.IsValid)
        {
            await _productApplicationService.Delete(deleteProductDto);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(deleteProductDto);
        }
    }
    #endregion

    #endregion

    #region [- Create() -]

    #region [- Get() -]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    #endregion

    #region [- Post() -]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostProductDto postProductDto)
    {
        if (ModelState.IsValid)
        {
            await _productApplicationService.Post(postProductDto);
            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View(postProductDto);
        }
    }
    #endregion
    #endregion

    #region [- Index() -]
    public async Task<IActionResult> Index()
    {
        var result = await _productApplicationService.GetAllProducts();
        return View(result);
    }
    #endregion

}
