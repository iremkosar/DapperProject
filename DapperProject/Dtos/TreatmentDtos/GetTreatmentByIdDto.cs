namespace DapperProject.Dtos.TreatmentDtos
{
    public class GetTreatmentByIdDto
    {
        public int TreatmentId { get; set; }
        public int AppointmentId { get; set; }
        public string PetName { get; set; }
        public string SpeciesName { get; set; }
        public string OwnerName { get; set; }
        public string VetName { get; set; }
        public string Diagnosis { get; set; }
        public string Notes { get; set; }
        public string TreatmentDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}