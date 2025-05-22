using apbd11.Data;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Services;

public class MedicamentService : IMedicamentService
{
    private readonly DatabaseContext _context;

    public MedicamentService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckMedicamentExistsAsync(int idMedicament, CancellationToken token)
    {
        return await _context.Medicaments.AnyAsync(x => x.IdMedicament == idMedicament, token);
    }
}