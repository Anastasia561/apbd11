using apbd11.Data;
using apbd11.Model;
using apbd11.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly DatabaseContext _context;

    public PrescriptionService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task CreatePrescriptionAsync(PrescriptionCreationRequestDto dto, CancellationToken token)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(token);

        try
        {
            if (!await PatientExists(dto.Patient, token))
            {
                await CreatePatient(dto.Patient, token);
            }

            var prescription = await CreateOnlyPrescriptionAsync(dto, token);
            await AddMedicamentsToPrescription(dto.Medicaments, prescription, token);
            await transaction.CommitAsync(token);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(token);
            throw;
        }
    }

    private async Task<Prescription> CreateOnlyPrescriptionAsync(PrescriptionCreationRequestDto dto,
        CancellationToken token)
    {
        var prescription = new Prescription
        {
            Patient = await _context.Patients.FirstAsync(p =>
                    p.FirstName == dto.Patient.FirstName
                    && p.LastName == dto.Patient.LastName
                    && p.Birthdate == dto.Patient.Birthdate
                , token),
            Doctor = await _context.Doctors.FirstAsync(d => d.IdDoctor == dto.IdDoctor, token),
            Date = dto.Date,
            DueDate = dto.DueDate
        };

        await _context.Prescriptions.AddAsync(prescription, token);
        await _context.SaveChangesAsync(token);
        return prescription;
    }

    private async Task CreatePatient(Patient patient, CancellationToken token)
    {
        await _context.Patients.AddAsync(new Patient()
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.Birthdate
        }, token);
        await _context.SaveChangesAsync(token);
    }

    private async Task<bool> PatientExists(Patient patient, CancellationToken token)
    {
        return await _context.Patients.AnyAsync(p => p.IdPatient == patient.IdPatient, token);
    }

    private async Task AddMedicamentsToPrescription(List<MedicamentDto> medicaments, Prescription p,
        CancellationToken token)
    {
        foreach (var medicamentDto in medicaments)
        {
            var medicament = await _context.Medicaments
                .FirstAsync(m => m.IdMedicament == medicamentDto.IdMedicament, token);

            var prescriptionMedicament = new PrescriptionMedicament
            {
                Prescription = p,
                Medicament = medicament,
                Dose = medicamentDto.Dose,
                Details = medicamentDto.Description
            };

            await _context.PrescriptionMedicaments.AddAsync(prescriptionMedicament, token);
        }

        await _context.SaveChangesAsync(token);
    }
}