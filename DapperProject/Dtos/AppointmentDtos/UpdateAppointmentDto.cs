namespace DapperProject.Dtos.AppointmentDtos
{
   
    public class UpdateAppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public string AppointmentDate { get; set; }
        public string Complaint { get; set; }
        public string Status { get; set; }
        public decimal AppointmentFee { get; set; }
    }
}
