namespace apbd11.Model.Dto;

public class PrescriptionCreationRequestDto
{
    public Patient Patient { get; set; }
    public int IdDoctor { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
}