using BolnickiSistem.Data;
using BolnickiSistem.Models;
using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BolnickiSistem.Services;

public class AutentifikacioniServis
{
    private readonly BolnicaContext _kontekst;

    public AutentifikacioniServis(BolnicaContext kontekst)
    {
        _kontekst = kontekst;
    }

    public async Task<Korisnik?> PrijaviKorisnikaAsync(
        string korisnickoIme,
        string lozinka)
    {
        Korisnik? korisnik =
            await _kontekst.Korisnici
                .Include(k => k.Uloga)
                .FirstOrDefaultAsync(k =>
                    k.KorisnickoIme == korisnickoIme);

        if (korisnik == null)
            return null;

        if (!korisnik.Aktivan)
            return null;

        bool ispravnaLozinka = ProveriLozinku(lozinka, korisnik.LozinkaHash);

        if (!ispravnaLozinka)
            return null;

        return korisnik;
    }


    private bool ProveriLozinku(string lozinka, string sacuvaniHash)
    {
        string[] delovi = sacuvaniHash.Split('$');

        if (delovi.Length != 6)
            return false;

        if (delovi[0] != "argon2id")
            return false;

        if (!int.TryParse(delovi[1], out int memorija))
            return false;

        if (!int.TryParse(delovi[2], out int iteracije))
            return false;

        if (!int.TryParse(delovi[3], out int paralelizam))
            return false;

        byte[] salt;

        byte[] ocekivaniHash;

        try
        {
            salt = Convert.FromBase64String(delovi[4]);

            ocekivaniHash = Convert.FromBase64String(delovi[5]);
        }
        catch (FormatException)
        {
            return false;
        }

        byte[] bajtoviLozinke = Encoding.UTF8.GetBytes(lozinka);

        using Argon2id argon2 =
            new Argon2id(bajtoviLozinke)
            {
                Salt = salt,
                MemorySize = memorija,
                Iterations = iteracije,
                DegreeOfParallelism = paralelizam
            };

        byte[] dobijeniHash = argon2.GetBytes(ocekivaniHash.Length);

        return CryptographicOperations.FixedTimeEquals(dobijeniHash, ocekivaniHash);
    }
}