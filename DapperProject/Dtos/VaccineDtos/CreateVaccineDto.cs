namespace DapperProject.Dtos.VaccineDtos
{
    public class CreateVaccineDto
    {
        public int PetId { get; set; }
        public int VetId { get; set; }
        public string VaccineName { get; set; }
        public string VaccineDate { get; set; }
        public string NextDueDate { get; set; }
        public string BatchNo { get; set; }
    }
}
