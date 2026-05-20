namespace DapperProject.Dtos.PetDtos
{
    public class PetDetailDto
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string SpeciesName { get; set; }
        public string BreedName { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public bool IsNeutered { get; set; }
        public decimal Weight { get; set; }
        public int TotalAppointments { get; set; }
        public int TotalVaccines { get; set; }
        public int TotalTreatments { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class PetAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string VetName { get; set; }
        public string AppointmentDate { get; set; }
        public string Complaint { get; set; }
        public string Status { get; set; }
        public decimal AppointmentFee { get; set; }
    }

    public class PetVaccineDto
    {
        public int VaccineId { get; set; }
        public string VaccineName { get; set; }
        public string VetName { get; set; }
        public string VaccineDate { get; set; }
        public string NextDueDate { get; set; }
        public string BatchNo { get; set; }
    }
}