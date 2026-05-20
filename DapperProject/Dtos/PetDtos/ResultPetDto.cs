namespace DapperProject.Dtos.PetDtos
{
    public class ResultPetDto
    {
        public int PetId { get; set; }
        public string Name { get; set; }
        public string SpeciesName { get; set; }  // Species tablosundan
        public string BreedName { get; set; }    // Breeds tablosundan
        public string OwnerName { get; set; }    // Owners tablosundan
        public string Gender { get; set; }
        public string BirthDate { get; set; }
        public bool IsNeutered { get; set; }
        public decimal Weight { get; set; }
    }
}
