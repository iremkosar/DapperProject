namespace DapperProject.Dtos.OwnerDtos
{
    public class OwnerDetailDto
    {
        public int OwnerId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string RegisteredAt { get; set; }
        public int TotalPets { get; set; }
        public int TotalAppointments { get; set; }
        public decimal TotalSpent { get; set; }
    }

    public class OwnerPetDto
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string SpeciesName { get; set; }
        public string BreedName { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public bool IsNeutered { get; set; }
        public decimal Weight { get; set; }
        public int AppointmentCount { get; set; }
    }

    public class OwnerAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PetName { get; set; }
        public string VetName { get; set; }
        public string AppointmentDate { get; set; }
        public string Complaint { get; set; }
        public string Status { get; set; }
        public decimal AppointmentFee { get; set; }
    }
}