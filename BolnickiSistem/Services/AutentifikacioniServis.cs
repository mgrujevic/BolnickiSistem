using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BolnickiSistem.Data;
using BolnickiSistem.Models;

namespace BolnickiSistem.Services;

public class AutentifikacioniServis
{
    private readonly BolnicaContext _kontekst;

    public AutentifikacioniServis(BolnicaContext kontekst)
    {
        _kontekst = kontekst;
    }

    public async Task<Korisnik?> PrijaviKorisnikaAsync(string korisnickoIme, string lozinka)
    {
        string lozinkaHash = KreirajHashLozinke(lozinka);

        return await _kontekst.Korisnici
            .Include(k => k.Uloga)
            .FirstOrDefaultAsync(k =>
                k.KorisnickoIme == korisnickoIme &&
                k.LozinkaHash == lozinkaHash &&
                k.Aktivan);
    }

    private string KreirajHashLozinke(string lozinka)
    {
        using SHA256 sha256 = SHA256.Create();

        byte[] bajtovi = Encoding.UTF8.GetBytes(lozinka);
        byte[] hash = sha256.ComputeHash(bajtovi);

        return Convert.ToHexString(hash);
    }
}