namespace BolnickiSistem.Models;

public class Dijagnoza
{
    public int DijagnozaId { get; set; }

    public int PregledId { get; set; }

    public string Sifra { get; set; } = string.Empty;

    public string Naziv { get; set; } = string.Empty;

    public string? Opis { get; set; }

    public Pregled? Pregled { get; set; }
}