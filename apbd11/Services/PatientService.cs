using apbd11.Data;
using apbd11.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Services;

public class PatientService : IPatientService
{
    private readonly DatabaseContext _context;

    public PatientService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<PatientDto> GetPatient(int patientId, CancellationToken token)
    {
        var medicamentList = await _context.Medicaments
            .Join(_context.PrescriptionMedicaments,
                m => m.IdMedicament,
                pm => pm.IdMedicament,
                (m, pm) => new
                {
                    pm.IdPrescription,
                    m.IdMedicament,
                    m.Name,
                    pm.Dose,
                    m.Description
                })
            .ToListAsync(token);

        var prescriptionsRaw = await _context.Prescriptions
            .Where(p => p.IdPatient == patientId)
            .Select(p => new PrescriptionDto
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new DoctorDto
                {
                    IdDoctor = p.Doctor.IdDoctor,
                    FirstName = p.Doctor.FirstName
                },
                Medicaments = new List<MedicamentForPatientDto>()
            })
            .OrderBy(p => p.DueDate)
            .ToListAsync(token);

        foreach (var prescription in prescriptionsRaw)
        {
            var meds = medicamentList
                .Where(m => m.IdPrescription == prescription.IdPrescription)
                .Select(m => new MedicamentForPatientDto
                {
                    IdMedicament = m.IdMedicament,
                    Name = m.Name,
                    Description = m.Description,
                    Dose = m.Dose
                }).ToList();

            prescription.Medicaments = meds;
        }

        var patient = await _context.Patients.Where(p => p.IdPatient == patientId)
            .Select(p => new PatientDto()
            {
                IdPatient = p.IdPatient,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Birthdate = p.Birthdate
            }).FirstAsync(token);

        patient.Prescriptions = prescriptionsRaw;

        return patient;
    }

    public async Task<bool> CheckPatientExists(int patientId, CancellationToken token)
    {
        return await _context.Patients.AnyAsync(p => p.IdPatient == patientId, token);
    }
}