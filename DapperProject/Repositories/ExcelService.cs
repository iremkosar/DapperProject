using ClosedXML.Excel;
using DapperProject.Dtos.AppointmentDtos;
using DapperProject.Dtos.OwnerDtos;
using DapperProject.Dtos.PetDtos;
using DapperProject.Dtos.VetDtos;

namespace DapperProject.Repositories
{
    public class ExcelService
    {
        public byte[] ExportAppointments(List<ResultAppointmentDto> data)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Randevular");

            // Başlıklar
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Hayvan";
            ws.Cell(1, 3).Value = "Tür";
            ws.Cell(1, 4).Value = "Sahip";
            ws.Cell(1, 5).Value = "Şehir";
            ws.Cell(1, 6).Value = "Veteriner";
            ws.Cell(1, 7).Value = "Tarih";
            ws.Cell(1, 8).Value = "Şikayet";
            ws.Cell(1, 9).Value = "Durum";
            ws.Cell(1, 10).Value = "Ücret";

            // Başlık stili
            var header = ws.Range(1, 1, 1, 10);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a3e");
            header.Style.Font.FontColor = XLColor.White;

            // Veriler
            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var d = data[i];
                ws.Cell(row, 1).Value = d.AppointmentId;
                ws.Cell(row, 2).Value = d.PetName;
                ws.Cell(row, 3).Value = d.SpeciesName;
                ws.Cell(row, 4).Value = d.OwnerName;
                ws.Cell(row, 5).Value = d.OwnerCity;
                ws.Cell(row, 6).Value = d.VetName;
                ws.Cell(row, 7).Value = d.AppointmentDate;
                ws.Cell(row, 8).Value = d.Complaint;
                ws.Cell(row, 9).Value = d.Status;
                ws.Cell(row, 10).Value = (double)d.AppointmentFee;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public byte[] ExportOwners(List<ResultOwnerDto> data)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Sahipler");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Ad Soyad";
            ws.Cell(1, 3).Value = "Cinsiyet";
            ws.Cell(1, 4).Value = "Telefon";
            ws.Cell(1, 5).Value = "E-posta";
            ws.Cell(1, 6).Value = "Şehir";
            ws.Cell(1, 7).Value = "Kayıt Tarihi";

            var header = ws.Range(1, 1, 1, 7);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a3e");
            header.Style.Font.FontColor = XLColor.White;

            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var d = data[i];
                ws.Cell(row, 1).Value = d.OwnerId;
                ws.Cell(row, 2).Value = d.FullName;
                ws.Cell(row, 3).Value = d.Gender == "E" ? "Erkek" : "Kadın";
                ws.Cell(row, 4).Value = d.Phone;
                ws.Cell(row, 5).Value = d.Email;
                ws.Cell(row, 6).Value = d.City;
                ws.Cell(row, 7).Value = d.RegisteredAt;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public byte[] ExportPets(List<ResultPetDto> data)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Hayvanlar");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Ad";
            ws.Cell(1, 3).Value = "Tür";
            ws.Cell(1, 4).Value = "Irk";
            ws.Cell(1, 5).Value = "Sahip";
            ws.Cell(1, 6).Value = "Cinsiyet";
            ws.Cell(1, 7).Value = "Doğum Tarihi";
            ws.Cell(1, 8).Value = "Kısırlaştırıldı";
            ws.Cell(1, 9).Value = "Ağırlık";

            var header = ws.Range(1, 1, 1, 9);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a3e");
            header.Style.Font.FontColor = XLColor.White;

            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var d = data[i];
                ws.Cell(row, 1).Value = d.PetId;
                ws.Cell(row, 2).Value = d.Name;
                ws.Cell(row, 3).Value = d.SpeciesName;
                ws.Cell(row, 4).Value = d.BreedName;
                ws.Cell(row, 5).Value = d.OwnerName;
                ws.Cell(row, 6).Value = d.Gender;
                ws.Cell(row, 7).Value = d.BirthDate;
                ws.Cell(row, 8).Value = d.IsNeutered ? "Evet" : "Hayır";
                ws.Cell(row, 9).Value = (double)d.Weight;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public byte[] ExportVets(List<ResultVetDto> data)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Veterinerler");

            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Ad Soyad";
            ws.Cell(1, 3).Value = "Uzmanlık";
            ws.Cell(1, 4).Value = "Telefon";
            ws.Cell(1, 5).Value = "E-posta";

            var header = ws.Range(1, 1, 1, 5);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a3e");
            header.Style.Font.FontColor = XLColor.White;

            for (int i = 0; i < data.Count; i++)
            {
                var row = i + 2;
                var d = data[i];
                ws.Cell(row, 1).Value = d.VetId;
                ws.Cell(row, 2).Value = d.FullName;
                ws.Cell(row, 3).Value = d.Specialty;
                ws.Cell(row, 4).Value = d.Phone;
                ws.Cell(row, 5).Value = d.Email;
            }

            ws.Columns().AdjustToContents();

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
    }
}