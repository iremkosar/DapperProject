namespace DapperProject.Dtos.TreatmentDtos
{
    public class ResultPrescriptionDto
    {
        public int PrescriptionId { get; set; }
        public string MedicineName { get; set; }
        public string ActiveIngredient { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int DosageDays { get; set; }
        public string Frequency { get; set; }
        public int Quantity { get; set; }
    }
}