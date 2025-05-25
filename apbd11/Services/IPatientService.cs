using apbd11.Model.Dto;

namespace apbd11.Services;

public interface IPatientService
{
    public Task<PatientDto> GetPatient(int patientId, CancellationToken token);
    public Task<bool> CheckPatientExists(int patientId, CancellationToken token);
}