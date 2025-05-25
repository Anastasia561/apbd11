namespace apbd11.Model.Dto;

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public List<MedicamentForPatientDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}