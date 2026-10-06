namespace BolnickiSistem.Models;

public class LaboratorijskiNalaz
{
    public int LaboratorijskiNalazId { get; set; }


    public int PacijentId { get; set; }

    public Pacijent? Pacijent { get; set; }

    public int? PregledId { get; set; }

    public Pregled? Pregled { get; set; }


    public string NazivAnalize { get; set; } = string.Empty;

    public string Rezultat { get; set; } = string.Empty;

    public string? Jedinica { get; set; }

    public string? ReferentnaVrednost { get; set; }

    public DateTime DatumAnalize { get; set; }

    public string? Napomena { get; set; }
}