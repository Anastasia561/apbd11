namespace apbd11.Model.Dto;

public class PatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly Birthdate { get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; }
}