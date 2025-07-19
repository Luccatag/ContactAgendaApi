using Microsoft.AspNetCore.Mvc;
using ContactAgendaApi.Repositories;
using ContactAgendaApi.Models;
using System.Collections.Generic;

namespace ContactAgendaApi.Controllers;

// Controller to demonstrate Dapper usage
[ApiController]
[Route("api/[controller]")]
public class DapperContactsController : ControllerBase
{
    private readonly DapperContactRepository _dapperRepo;

    public DapperContactsController(DapperContactRepository dapperRepo)
    {
        _dapperRepo = dapperRepo;
    }

    // GET: api/dappercontacts
    [HttpGet]
    public ActionResult<IEnumerable<Contact>> GetAll()
    {
        var contacts = _dapperRepo.GetAllContacts();
        return Ok(contacts);
    }
}