namespace DapperProject.Dtos.VetDtos
{
    public class GetVetByIdDto
    {
        public int VetId { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
