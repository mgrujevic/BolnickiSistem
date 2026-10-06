namespace BolnickiSistem.Models;

public class Pacijent
{
    public int PacijentId { get; set; }

    public string BrojZdravstveneKartice { get; set; } = string.Empty;

    public string JMBG { get; set; } = string.Empty;

    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public DateTime DatumRodjenja { get; set; }

    public string Pol { get; set; } = string.Empty;

    public string? Adresa { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? KrvnaGrupa { get; set; }

    public string? Alergije { get; set; }

    public string? HronicneBolesti { get; set; }

    public int? LekarId { get; set; }
    public Korisnik? Lekar { get; set; }

    public ICollection<Pregled> Pregledi { get; set; }
        = new List<Pregled>();

    public ICollection<LaboratorijskiNalaz> LaboratorijskiNalazi { get; set; }
        = new List<LaboratorijskiNalaz>();

    public ICollection<Dokument> Dokumenti { get; set; }
        = new List<Dokument>();
}