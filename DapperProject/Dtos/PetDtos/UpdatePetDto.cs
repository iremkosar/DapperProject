namespace DapperProject.Dtos.PetDtos
{
    public class UpdatePetDto
    {
        public int PetId { get; set; }
        public int OwnerId { get; set; }
        public int SpeciesId { get; set; }
        public int BreedId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public bool IsNeutered { get; set; }
        public decimal Weight { get; set; }
    }
}
