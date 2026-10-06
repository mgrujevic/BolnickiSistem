namespace BolnickiSistem.Models;

public class Korisnik
{
    public int KorisnikId { get; set; }

    public string KorisnickoIme { get; set; } = string.Empty;

    public string LozinkaHash { get; set; } = string.Empty;

    public string Ime { get; set; } = string.Empty;

    public string Prezime { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Aktivan { get; set; } = true;

    public DateTime DatumKreiranja { get; set; } = DateTime.Now;

    public int UlogaId { get; set; }

    public Uloga? Uloga { get; set; }


    public ICollection<Pregled> Pregledi { get; set; }
        = new List<Pregled>();


    public ICollection<Dokument> Dokumenti { get; set; }
        = new List<Dokument>();

    public ICollection<Pacijent> Pacijenti { get; set; }
    = new List<Pacijent>();

    public string ImePrezime => $"{Ime} {Prezime}";
}