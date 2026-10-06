namespace BolnickiSistem.Models;

public class Terapija
{
    public int TerapijaId { get; set; }

    public int PregledId { get; set; }

    public string NazivLeka { get; set; } = string.Empty;

    public string Doziranje { get; set; } = string.Empty;

    public string Ucestalost { get; set; } = string.Empty;

    public DateTime DatumPocetka { get; set; }

    public DateTime? DatumZavrsetka { get; set; }

    public string? Uputstvo { get; set; }

    public Pregled? Pregled { get; set; }
}