namespace DapperProject.Dtos.AppointmentDtos
{
    // Listeleme sayfasında her randevuyu göstermek için kullanılır
    // Join sorgusundan gelen tüm alanları içerir
    public class ResultAppointmentDto
    {
        public int AppointmentId { get; set; }
        public string PetName { get; set; }      // Pets tablosundan
        public string SpeciesName { get; set; }  // Species tablosundan
        public string OwnerName { get; set; }    // Owners tablosundan
        public string OwnerCity { get; set; }    // Owners tablosundan
        public string VetName { get; set; }      // Vets tablosundan
        public string AppointmentDate { get; set; }
        public string Complaint { get; set; }
        public string Status { get; set; }
        public decimal AppointmentFee { get; set; }
    }
}
