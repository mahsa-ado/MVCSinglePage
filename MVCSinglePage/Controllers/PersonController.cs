
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

    #region [- GetPersonDetails() -]
    [HttpGet]
    public async Task<IActionResult> GetPersonDetails([FromQuery] GetPersonByIdDto getPersonByIdDto)
    {
        var result = await _personApplicationService.GetPersonDetail(getPersonByIdDto);
        if (result == null)
        {
            return BadRequest(new{ message = "The submitted information could not be processed."});
        }
        else
        {
            return Json(result);
        }
    }
    #endregion

    #region [- Edit() -]

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
    #region [- Post() -]
    [HttpPost]
    public async Task<IActionResult> Delete([FromBody]DeletePersonDto deletePersonDto)
    {
        if (deletePersonDto == null || deletePersonDto.Id == Guid.Empty)
        {
            return BadRequest(new { success = false, message = "Invalid person id" });
        }
        if (ModelState.IsValid)
        {
            var person = await _personApplicationService.GetPersonDelete(
                new GetPersonByIdDto { Id = deletePersonDto.Id });
            if (person == null)
            {
                return NotFound(new { success = false, message = "Person was not found" });
            }

            await _personApplicationService.Delete(person);
            return Json(new { success = true, message = "Person deleted successfully" });

        }
        else
        {
            return BadRequest(new { success = false, message = "Validation failed" });
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
