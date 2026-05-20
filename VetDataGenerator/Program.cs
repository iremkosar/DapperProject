using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=== Veteriner Klinik Veri Üretici ===\n");

var rnd = new Random(42);
string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vet_csv");
Directory.CreateDirectory(outputDir);

// ── YARDIMCI FONKSİYONLAR ───────────────────────────────────

string WeightedChoice(string[] items, int[] weights)
{
    int total = weights.Sum();
    int r = rnd.Next(total);
    int cum = 0;
    for (int i = 0; i < items.Length; i++) { cum += weights[i]; if (r < cum) return items[i]; }
    return items[^1];
}

string RandPhone() => $"05{rnd.Next(10, 59)}{rnd.Next(1000000, 9999999)}";

DateTime RandDate(DateTime start, DateTime end)
{
    int range = (end - start).Days;
    return start.AddDays(rnd.Next(range));
}

void WriteCsv(string fileName, IEnumerable<string> lines)
{
    string path = Path.Combine(outputDir, fileName);
    File.WriteAllLines(path, lines, new UTF8Encoding(true));
    Console.WriteLine($"✓ {fileName,-25} → {lines.Count() - 1,10:N0} satır");
}

// ── LOOKUP VERİLERİ ──────────────────────────────────────────

var cities = new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya", "Adana", "Konya", "Gaziantep", "Mersin", "Kayseri" };
var cityWeights = new[] { 30, 18, 12, 8, 7, 6, 5, 5, 5, 4 };
var maleNames = new[] { "Ahmet", "Mehmet", "Mustafa", "Ali", "Hüseyin", "Hasan", "İbrahim", "Murat", "Ömer", "Yusuf", "Emre", "Burak", "Serkan", "Tolga", "Kemal" };
var femaleNames = new[] { "Fatma", "Ayşe", "Emine", "Hatice", "Zeynep", "Elif", "Merve", "Selin", "Nur", "Gül", "Pınar", "Esra", "Deniz", "Yağmur", "Büşra" };
var surnames = new[] { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Doğan", "Arslan", "Koç", "Öztürk", "Aydın", "Yıldız", "Güneş", "Polat", "Çetin", "Kılıç", "Acar", "Yaman", "Erdoğan", "Şen", "Özkan" };
var petMaleNames = new[] { "Max", "Charlie", "Buddy", "Rocky", "Oliver", "Leo", "Milo", "Simba", "Oscar", "Loki", "Atlas", "Zeus", "Kral", "Aslan", "Kaplan", "Paşa" };
var petFemNames = new[] { "Luna", "Bella", "Lucy", "Daisy", "Nala", "Cleo", "Mia", "Zoe", "Lily", "Maya", "Pamuk", "Boncuk", "Minnoş", "Prenses", "Şeker", "İnci" };
var complaints = new[] { "Yemek yemiyor", "Kusma", "İshal", "Kilo kaybı", "Aşı yaptırma", "Rutin kontrol", "Yaralanma", "Göz akıntısı", "Kulak enfeksiyonu", "Deri sorunu", "Solunum güçlüğü", "Diş sorunu", "Topallık", "Yorgunluk", "Aşırı tüy dökülmesi", "İdrar sorunu", "Davranış değişikliği", "Ameliyat sonrası kontrol", "Kist kontrolü", "Genel muayene" };
var diagnoses = new[] { "Gastroenterit", "Üst Solunum Yolu Enfeksiyonu", "Deri Enfeksiyonu", "Otitis Externa", "Konjunktivit", "Üriner Sistem Enfeksiyonu", "Diş Taşı", "Kırık", "Yara", "Parazit Enfestasyonu", "Alerjik Reaksiyon", "Kardiyomiyopati", "Böbrek Yetmezliği", "Diyabet", "Epilepsi", "Obezite", "Anemi", "Karaciğer Hastalığı", "Pankreatit", "Tümör" };
var statuses = new[] { "Tamamlandı", "İptal", "Bekliyor", "Devam Ediyor" };
var statusW = new[] { 75, 10, 10, 5 };
var frequencies = new[] { "Günde 1 kez", "Günde 2 kez", "Günde 3 kez", "12 saatte bir", "Haftada 1 kez" };
var specialties = new[] { "Genel Pratisyen", "Cerrahi", "Dermatoloji", "Kardiyoloji", "Onkoloji", "Nöroloji", "Göz Hastalıkları", "Diş Hekimliği", "Egzotik Hayvanlar" };
var aptFees = new[] { 150, 200, 250, 300, 350, 400 };
var dosageDays = new[] { 3, 5, 7, 10, 14, 21, 30 };

// ── 1. SPECIES ───────────────────────────────────────────────
var speciesList = new List<(int Id, string Name)>
{
    (1,"Kedi"),(2,"Köpek"),(3,"Kuş"),(4,"Tavşan"),(5,"Hamster"),(6,"Balık"),(7,"Sürüngen")
};
var speciesLines = new List<string> { "SpeciesId,SpeciesName" };
speciesLines.AddRange(speciesList.Select(s => $"{s.Id},{s.Name}"));
WriteCsv("Species.csv", speciesLines);

// ── 2. BREEDS ────────────────────────────────────────────────
var breedsList = new List<(int Id, int SpeciesId, string Name)>
{
    (1,1,"İran Kedisi"),(2,1,"British Shorthair"),(3,1,"Siyam"),(4,1,"Maine Coon"),(5,1,"Scottish Fold"),(6,1,"Tekir"),(7,1,"Van Kedisi"),
    (8,2,"Golden Retriever"),(9,2,"Labrador"),(10,2,"Alman Kurdu"),(11,2,"Bulldog"),(12,2,"Chihuahua"),(13,2,"Husky"),(14,2,"Poodle"),(15,2,"Beagle"),(16,2,"Rottweiler"),
    (17,3,"Muhabbet Kuşu"),(18,3,"Sultan Papağanı"),(19,3,"Jako"),(20,3,"Kanarya"),
    (21,4,"Hollanda Tavşanı"),(22,4,"Angora Tavşanı"),(23,4,"Rex Tavşanı"),
    (24,5,"Suriye Hamsteri"),(25,5,"Cüce Hamster"),
    (26,6,"Akvaryum Balığı"),(27,6,"Japon Balığı"),
    (28,7,"Kaplumbağa"),(29,7,"Kertenkele"),
};
var breedBySpecies = breedsList.GroupBy(b => b.SpeciesId).ToDictionary(g => g.Key, g => g.Select(b => b.Id).ToList());
var breedsLines = new List<string> { "BreedId,SpeciesId,BreedName" };
breedsLines.AddRange(breedsList.Select(b => $"{b.Id},{b.SpeciesId},{b.Name}"));
WriteCsv("Breeds.csv", breedsLines);

// ── 3. VETS ──────────────────────────────────────────────────
var vetNames = new[] {
    "Dr. Ahmet Yılmaz","Dr. Elif Kaya","Dr. Mehmet Demir","Dr. Zeynep Çelik",
    "Dr. Can Şahin","Dr. Ayşe Arslan","Dr. Burak Koç","Dr. Selin Öztürk",
    "Dr. Emre Yıldız","Dr. Deniz Aydın","Dr. Fatma Doğan","Dr. Kerem Polat",
    "Dr. Merve Güneş","Dr. Okan Çetin","Dr. Pınar Kılıç","Dr. Serhat Acar",
    "Dr. Tuğba Yaman","Dr. Ufuk Erdoğan","Dr. Vildan Şen","Dr. Yusuf Özkan"
};
var vetsLines = new List<string> { "VetId,FullName,Specialty,Phone,Email" };
for (int i = 0; i < vetNames.Length; i++)
    vetsLines.Add($"{i + 1},{vetNames[i]},{specialties[rnd.Next(specialties.Length)]},{RandPhone()},vet{i + 1}@veterklinik.com");
WriteCsv("Vets.csv", vetsLines);

// ── 4. MEDICINES ─────────────────────────────────────────────
var medicineData = new[]
{
    ("Amoksisilin","Amoksisilin","45.50","Antibiyotik"),("Enrofloksasin","Enrofloksasin","62.00","Antibiyotik"),
    ("Metronidazol","Metronidazol","38.75","Antibiyotik"),("Deksametazon","Deksametazon","55.00","Antiinflamatuar"),
    ("Meloksikam","Meloksikam","78.50","Antiinflamatuar"),("Prednizolon","Prednizolon","42.00","Kortikosteroid"),
    ("Furosemid","Furosemid","35.00","Diüretik"),("Atenolol","Atenolol","68.00","Kardiyovasküler"),
    ("Fenobarbital","Fenobarbital","95.00","Antiepileptik"),("Metoklopramid","Metoklopramid","28.50","Antiemetik"),
    ("Ondansetron","Ondansetron","110.00","Antiemetik"),("Omeprazol","Omeprazol","52.00","Mide Koruyucu"),
    ("Sukralfat","Sukralfat","45.00","Mide Koruyucu"),("İvermektin","İvermektin","35.00","Antiparazitik"),
    ("Fenbendazol","Fenbendazol","42.00","Antiparazitik"),("Prazikuantel","Prazikuantel","38.00","Antiparazitik"),
    ("Ketoprofen","Ketoprofen","65.00","Ağrı Kesici"),("Tramadol","Tramadol","85.00","Ağrı Kesici"),
    ("Gabapentin","Gabapentin","72.00","Nörolojik"),("Amlodipine","Amlodipine","58.00","Kardiyovasküler"),
    ("Digoksin","Digoksin","44.00","Kardiyovasküler"),("Sotalol","Sotalol","67.00","Kardiyovasküler"),
    ("Kloramin","Kloramin","22.00","Antiseptik"),("Povidon İyot","Povidon İyot","18.50","Antiseptik"),
    ("Vitamin B Kompleks","Vitamin B","32.00","Vitamin"),("Vitamin C","Vitamin C","25.00","Vitamin"),
    ("Omega-3","Omega-3","48.00","Besin Desteği"),("Probiyotik","Probiyotik","55.00","Sindirim"),
    ("Laktuloz","Laktuloz","38.00","Sindirim"),("Silymarin","Silymarin","62.00","Karaciğer Koruyucu"),
};
var medLines = new List<string> { "MedicineId,MedicineName,ActiveIngredient,UnitPrice,Category" };
for (int i = 0; i < medicineData.Length; i++)
{
    var m = medicineData[i];
    medLines.Add($"{i + 1},{m.Item1},{m.Item2},{m.Item3},{m.Item4}");
}
WriteCsv("Medicines.csv", medLines);

// ── 5. OWNERS ────────────────────────────────────────────────
Console.Write("Owners üretiliyor...   ");
int N_OWNERS = 50_000;
var ownerLines = new List<string> { "OwnerId,FullName,Gender,Phone,Email,City,RegisteredAt" };
var ownerGenders = new string[N_OWNERS];
for (int i = 0; i < N_OWNERS; i++)
{
    string gender = rnd.Next(2) == 0 ? "E" : "K";
    ownerGenders[i] = gender;
    string fn = gender == "E" ? maleNames[rnd.Next(maleNames.Length)] : femaleNames[rnd.Next(femaleNames.Length)];
    string ln = surnames[rnd.Next(surnames.Length)];
    string city = WeightedChoice(cities, cityWeights);
    DateTime regDate = RandDate(new DateTime(2018, 1, 1), new DateTime(2024, 12, 31));
    ownerLines.Add($"{i + 1},{fn} {ln},{gender},{RandPhone()},sahip{i + 1}@mail.com,{city},{regDate:yyyy-MM-dd}");
}
WriteCsv("Owners.csv", ownerLines);

// ── 6. PETS ──────────────────────────────────────────────────
Console.Write("Pets üretiliyor...     ");
int N_PETS = 80_000;
var speciesIds = new[] { 1, 2, 3, 4, 5, 6, 7 };
var speciesProbs = new[] { 38, 42, 8, 5, 3, 2, 2 };
var petLines = new List<string> { "PetId,OwnerId,SpeciesId,BreedId,Name,Gender,BirthDate,IsNeutered,Weight" };
var petSpeciesArr = new int[N_PETS];
for (int i = 0; i < N_PETS; i++)
{
    int spId = int.Parse(WeightedChoice(speciesIds.Select(x => x.ToString()).ToArray(), speciesProbs));
    petSpeciesArr[i] = spId;
    int breedId = breedBySpecies[spId][rnd.Next(breedBySpecies[spId].Count)];
    string pg = rnd.Next(2) == 0 ? "Erkek" : "Dişi";
    string pn = pg == "Erkek" ? petMaleNames[rnd.Next(petMaleNames.Length)] : petFemNames[rnd.Next(petFemNames.Length)];
    DateTime bd = RandDate(new DateTime(2010, 1, 1), new DateTime(2024, 1, 1));
    int neutered = rnd.Next(100) < 55 ? 1 : 0;
    double wt = Math.Round(0.2 + rnd.NextDouble() * 44.8, 1);
    petLines.Add($"{i + 1},{rnd.Next(1, N_OWNERS + 1)},{spId},{breedId},{pn},{pg},{bd:yyyy-MM-dd},{neutered},{wt}");
}
WriteCsv("Pets.csv", petLines);

// ── 7. APPOINTMENTS ──────────────────────────────────────────
Console.Write("Appointments üretiliyor...");
int N_APT = 400_000;
var aptLines = new List<string> { "AppointmentId,PetId,VetId,AppointmentDate,Complaint,Status,AppointmentFee" };
var aptStatuses = new string[N_APT];
var aptDates = new DateTime[N_APT];
for (int i = 0; i < N_APT; i++)
{
    DateTime aptDate = RandDate(new DateTime(2020, 1, 1), new DateTime(2024, 12, 31));
    aptDate = aptDate.AddHours(rnd.Next(8, 18)).AddMinutes(rnd.Next(4) * 15);
    string status = WeightedChoice(statuses, statusW);
    aptStatuses[i] = status;
    aptDates[i] = aptDate;
    int fee = aptFees[rnd.Next(aptFees.Length)];
    aptLines.Add($"{i + 1},{rnd.Next(1, N_PETS + 1)},{rnd.Next(1, vetNames.Length + 1)},{aptDate:yyyy-MM-dd HH:mm},{complaints[rnd.Next(complaints.Length)]},{status},{fee}");
}
WriteCsv("Appointments.csv", aptLines);

// ── 8. TREATMENTS ────────────────────────────────────────────
Console.Write("Treatments üretiliyor...");
var treatLines = new List<string> { "TreatmentId,AppointmentId,Diagnosis,Notes,TreatmentDate,TotalCost" };
int treatId = 1;
var treatmentIds = new List<int>();
for (int i = 0; i < N_APT; i++)
{
    if (aptStatuses[i] != "Tamamlandı") continue;
    double cost = Math.Round(100 + rnd.NextDouble() * 2400, 2);
    treatLines.Add($"{treatId},{i + 1},{diagnoses[rnd.Next(diagnoses.Length)]},\"Tedavi notu {treatId}: Hasta stabil durumda.\",{aptDates[i]:yyyy-MM-dd HH:mm},{cost}");
    treatmentIds.Add(treatId);
    treatId++;
}
WriteCsv("Treatments.csv", treatLines);

// ── 9. PRESCRIPTIONS ─────────────────────────────────────────
Console.Write("Prescriptions üretiliyor...");
var presLines = new List<string> { "PrescriptionId,TreatmentId,MedicineId,DosageDays,Frequency,Quantity" };
int presId = 1;
int[] nMedWeights = { 30, 40, 20, 10 };
foreach (int tid in treatmentIds)
{
    int nMeds = int.Parse(WeightedChoice(new[] { "1", "2", "3", "4" }, nMedWeights));
    var chosen = Enumerable.Range(1, medicineData.Length).OrderBy(_ => rnd.Next()).Take(nMeds);
    foreach (int mid in chosen)
    {
        presLines.Add($"{presId},{tid},{mid},{dosageDays[rnd.Next(dosageDays.Length)]},{frequencies[rnd.Next(frequencies.Length)]},{rnd.Next(1, 4)}");
        presId++;
    }
}
WriteCsv("Prescriptions.csv", presLines);

// ── 10. VACCINES ─────────────────────────────────────────────
Console.Write("Vaccines üretiliyor...  ");
var vaccinesBySpecies = new Dictionary<int, string[]>
{
    { 1, new[]{"Kuduz","FPV-FHV-FCV (Üçlü)","FeLV (Lösemi)","Klamidya"} },
    { 2, new[]{"Kuduz","DHPP (Dörtlü)","Leptospira","Kennel Cough","Bordetella"} },
    { 3, new[]{"Polioma Virüsü","Newcastle"} },
    { 4, new[]{"Kuduz","Mixomatozis","VHD"} },
    { 7, new[]{"Salmonella"} },
};
var vacLines = new List<string> { "VaccineId,PetId,VetId,VaccineName,VaccineDate,NextDueDate,BatchNo" };
int vacId = 1;
for (int i = 0; i < N_PETS; i++)
{
    int sp = petSpeciesArr[i];
    if (!vaccinesBySpecies.ContainsKey(sp)) continue;
    var vaxList = vaccinesBySpecies[sp];
    int nVax = rnd.Next(1, Math.Min(4, vaxList.Length) + 1);
    var chosen = vaxList.OrderBy(_ => rnd.Next()).Take(nVax);
    foreach (string vname in chosen)
    {
        DateTime vdate = RandDate(new DateTime(2018, 1, 1), new DateTime(2024, 12, 31));
        DateTime nextDue = vdate.AddYears(1);
        string batch = $"BAT{rnd.Next(10000, 99999)}";
        vacLines.Add($"{vacId},{i + 1},{rnd.Next(1, vetNames.Length + 1)},{vname},{vdate:yyyy-MM-dd},{nextDue:yyyy-MM-dd},{batch}");
        vacId++;
    }
}
WriteCsv("Vaccines.csv", vacLines);

// ── ÖZET ─────────────────────────────────────────────────────
long total = new[]{ "Species","Breeds","Vets","Medicines","Owners","Pets",
                    "Appointments","Treatments","Prescriptions","Vaccines" }
    .Sum(f => File.ReadLines(Path.Combine(outputDir, f + ".csv")).Count() - 1);

Console.WriteLine("\n" + new string('═', 44));
Console.WriteLine($"  TOPLAM : {total,15:N0} satır");
Console.WriteLine($"  Klasör : {outputDir}");
Console.WriteLine(new string('═', 44));
Console.WriteLine("\nBitti! CSV dosyaları oluşturuldu.");
Console.ReadKey();

