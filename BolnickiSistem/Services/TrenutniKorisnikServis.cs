using BolnickiSistem.Models;

namespace BolnickiSistem.Services;

public class TrenutniKorisnikServis
{
    public Korisnik? Korisnik { get; private set; }

    public void PostaviKorisnika(Korisnik korisnik)
    {
        Korisnik = korisnik;
    }

    public void OdjaviKorisnika()
    {
        Korisnik = null;
    }

    public int KorisnikId => Korisnik?.KorisnikId ?? throw new InvalidOperationException("Trenutni korisnik nije prijavljen.");
}