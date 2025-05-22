using apbd11.Model.Dto;
using apbd11.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd11.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;
    private readonly IMedicamentService _medicamentService;

    public PrescriptionsController(IPrescriptionService prescriptionService,
        IMedicamentService medicamentService)
    {
        _prescriptionService = prescriptionService;
        _medicamentService = medicamentService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePrescription(PrescriptionCreationRequestDto dto, CancellationToken token)
    {
        if (dto.Medicaments.Count > 10) return BadRequest("Medicaments list is too large");
        if (dto.DueDate <= dto.Date) return BadRequest("Due date is before the prescription date");
        foreach (var m in dto.Medicaments)
        {
            if (!await _medicamentService.CheckMedicamentExistsAsync(m.IdMedicament, token))
            {
                return BadRequest($"Medicament with id - {m.IdMedicament} is missing");
            }
        }

        await _prescriptionService.CreatePrescriptionAsync(dto, token);

        return Ok("Prescription created");
    }
}