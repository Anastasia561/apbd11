using apbd11.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken token)
    {
        if (!await _patientService.CheckPatientExists(id, token))
        {
            return NotFound($"Patient with id - {id} not found");
        }

        return Ok(await _patientService.GetPatient(id, token));
    }
}