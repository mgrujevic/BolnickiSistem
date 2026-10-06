namespace BolnickiSistem.Models;

public class Pregled
{
    public int PregledId { get; set; }

    public int PacijentId { get; set; }

    public Pacijent? Pacijent { get; set; }


    public int LekarId { get; set; }

    public Korisnik? Lekar { get; set; }


    public DateTime DatumPregleda { get; set; }

    public string RazlogDolaska { get; set; } = string.Empty;

    public string? Simptomi { get; set; }

    public string? Nalaz { get; set; }

    public string? Preporuka { get; set; }

    public DateTime? DatumKontrole { get; set; }


    public ICollection<Dijagnoza> Dijagnoze { get; set; }
        = new List<Dijagnoza>();


    public ICollection<Terapija> Terapije { get; set; }
        = new List<Terapija>();


    public ICollection<LaboratorijskiNalaz> LaboratorijskiNalazi { get; set; }
        = new List<LaboratorijskiNalaz>();


    public ICollection<Dokument> Dokumenti { get; set; }
        = new List<Dokument>();
}