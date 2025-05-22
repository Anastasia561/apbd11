using apbd11.Model.Dto;

namespace apbd11.Services;

public interface IPrescriptionService
{
    public Task CreatePrescriptionAsync(PrescriptionCreationRequestDto dto, CancellationToken token);
}