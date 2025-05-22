using apbd11.Model;

namespace apbd11.Services;

public interface IMedicamentService
{
    public Task<bool> CheckMedicamentExistsAsync(int idMedicament, CancellationToken token);
}