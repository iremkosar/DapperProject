namespace DapperProject.Dtos.VaccineDtos
{
    public class ResultVaccineDto
    {
        public int VaccineId { get; set; }
        public string PetName { get; set; }
        public string SpeciesName { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string VetName { get; set; }
        public string VaccineName { get; set; }
        public string VaccineDate { get; set; }
        public string NextDueDate { get; set; }
        public string BatchNo { get; set; }
    }
}
