
using MVCSinglePage.ApplicationServices.Dtos.PersonDtos;
using MVCSinglePage.ApplicationServices.Services.Contracts;
using MVCSinglePage.Models;
using MVCSinglePage.Models.DomainModels.PersonAggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PersonController : Controller
{

    private readonly IPersonApplicationService _personApplicationService;

    #region [-ctor-]
    public PersonController(IPersonApplicationService personApplicationService)
    {
        _personApplicationService = personApplicationService;

    }
    #endregion

    #region [- Detail() -]
    [HttpGet]
    public async Task<IActionResult> Detail(GetPersonByIdDto getPersonDtoById)
    {
        if (getPersonDtoById == null)
        {
            return NotFound();
        }
        var result = await _personApplicationService.GetPersonDetail(getPersonDtoById);
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
    public async Task<IActionResult> Edit(GetPersonByIdDto getPersonDtoById)
    {
        if (getPersonDtoById == null)
        {
            return NotFound();
        }
        var result = await _personApplicationService.GetPersonById(getPersonDtoById);
        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }
    #endregion
    #region [- Post() -]
    [HttpPost]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromBody] PutPersonDto putPersonDto)
    {
        if (ModelState.IsValid)
        {
            await _personApplicationService.Put(putPersonDto);
            return Json(new { success = true, message = "Operation successful" });
        }
        else
        {
            return Json(new { success = false, message = "Operation failed" });
        }
    }
    #endregion
    #endregion

    #region [- Delete() -]
    #region [- Get() -]
    public async Task<IActionResult> Delete(GetPersonByIdDto getPersonDtoById)
    {
        if (getPersonDtoById == null)
        {
            return NotFound();
        }

        var person = await _personApplicationService.GetPersonDelete(getPersonDtoById);

        if (person == null)
        {
            return NotFound();
        }
        return View(person);
    }

    #endregion
    #region [- Post() -]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(DeletePersonDto deletePersonDto)
    {
        if (ModelState.IsValid)
        {
            await _personApplicationService.Delete(deletePersonDto);
            return RedirectToAction(nameof(Index));

        }
        else
        {
            return View(deletePersonDto);
        }
    }
    #endregion
    #endregion

    #region [- Index() -]
    public async Task<IActionResult> Index()
    {
        var result = await _personApplicationService.GetAllPerson();
        return View(result);
    }
    #endregion

    #region [- create() -]

    #region [- Get() -]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    #endregion
    #region [- Post() -]
    [HttpPost]
    public async Task<IActionResult> create([FromBody] PostPersonDto postPersonDto)

    {
        if (ModelState.IsValid)
        {
            await _personApplicationService.Post(postPersonDto);
            return Json(new { success = true, message = "Operation successful" });
        }
        else
        {
            return Json(new { success = false, message = "Validation failed" });
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetPersonDetails([FromQuery] GetPersonByIdDto getPersonByIdDto)
    {
        var result = await _personApplicationService.GetPersonDetail(getPersonByIdDto);
        return result == null ? NotFound() : Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePerson([FromBody] DeletePersonDto deletePersonDto)
    {
        if (deletePersonDto == null || deletePersonDto.Id == Guid.Empty)
        {
            return BadRequest(new { success = false, message = "Invalid person id" });
        }

        await _personApplicationService.Delete(deletePersonDto);
        return Json(new { success = true, message = "Operation successful" });
    }
    #endregion

    #endregion

    #region [- GetAll() -]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _personApplicationService.GetAllPerson();
        return Json(result);
    }

    #endregion
}
