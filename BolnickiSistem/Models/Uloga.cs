namespace BolnickiSistem.Models;

public class Uloga
{
    public int UlogaId { get; set; }

    public string Naziv { get; set; } = string.Empty;

    public ICollection<Korisnik> Korisnici { get; set; }
        = new List<Korisnik>();
}