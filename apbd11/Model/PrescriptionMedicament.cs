using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace apbd11.Model;

[Table("Prescription_Medicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    [ForeignKey(nameof(Medicament))] public int IdMedicament { get; set; }
    [ForeignKey(nameof(Prescription))] public int IdPrescription { get; set; }
    [Range(0, int.MaxValue)] public int? Dose { get; set; }
    [Required] [MaxLength(100)] public string Details { get; set; }
    public Medicament Medicament { get; set; }
    public Prescription Prescription { get; set; }
}