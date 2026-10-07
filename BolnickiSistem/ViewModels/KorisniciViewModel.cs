using BolnickiSistem.Data;
using BolnickiSistem.Models;
using Konscious.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace BolnickiSistem.ViewModels;

public class KorisniciViewModel : INotifyPropertyChanged
{
    private readonly BolnicaContext _kontekst;

    private Korisnik? _izabraniKorisnik;
    private string _korisnickoIme = string.Empty;
    private string _lozinka = string.Empty;
    private string _ime = string.Empty;
    private string _prezime = string.Empty;
    private string _email = string.Empty;
    private int _izabranaUlogaId = 2;
    private bool _aktivan = true;
    private string _poruka = string.Empty;

    public ObservableCollection<Korisnik> Korisnici { get; } = new();

    public ObservableCollection<Uloga> Uloge { get; } = new();

    public Korisnik? IzabraniKorisnik
    {
        get => _izabraniKorisnik;
        set
        {
            _izabraniKorisnik = value;
            OnPropertyChanged();

            if (value != null)
            {
                KorisnickoIme = value.KorisnickoIme;
                Ime = value.Ime;
                Prezime = value.Prezime;
                Email = value.Email;
                IzabranaUlogaId = value.UlogaId;
                Aktivan = value.Aktivan;
                Lozinka = string.Empty;
            }
        }
    }

    public string KorisnickoIme
    {
        get => _korisnickoIme;
        set
        {
            _korisnickoIme = value;
            OnPropertyChanged();
        }
    }

    public string Lozinka
    {
        get => _lozinka;
        set
        {
            _lozinka = value;
            OnPropertyChanged();
        }
    }

    public string Ime
    {
        get => _ime;
        set
        {
            _ime = value;
            OnPropertyChanged();
        }
    }

    public string Prezime
    {
        get => _prezime;
        set
        {
            _prezime = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public int IzabranaUlogaId
    {
        get => _izabranaUlogaId;
        set
        {
            _izabranaUlogaId = value;
            OnPropertyChanged();
        }
    }

    public bool Aktivan
    {
        get => _aktivan;
        set
        {
            _aktivan = value;
            OnPropertyChanged();
        }
    }

    public string Poruka
    {
        get => _poruka;
        set
        {
            _poruka = value;
            OnPropertyChanged();
        }
    }

    public ICommand NoviKorisnikCommand { get; }
    public ICommand SacuvajCommand { get; }
    public ICommand ObrisiCommand { get; }
    public ICommand AktivirajDeaktivirajCommand { get; }

    public KorisniciViewModel(BolnicaContext kontekst)
    {
        _kontekst = kontekst;

        NoviKorisnikCommand = new RelayCommand(_ => NoviKorisnik());
        SacuvajCommand = new RelayCommand(async _ => await SacuvajAsync());
        ObrisiCommand = new RelayCommand(async _ => await ObrisiAsync());
        AktivirajDeaktivirajCommand = new RelayCommand(async _ => await AktivirajDeaktivirajAsync());

        _ = UcitajPodatkeAsync();
    }

    private async Task UcitajPodatkeAsync()
    {
        var korisnici = await _kontekst.Korisnici
            .Include(k => k.Uloga)
            .OrderBy(k => k.Prezime)
            .ThenBy(k => k.Ime)
            .ToListAsync();

        Korisnici.Clear();

        foreach (var korisnik in korisnici)
            Korisnici.Add(korisnik);

        var uloge = await _kontekst.Uloge
            .OrderBy(u => u.Naziv)
            .ToListAsync();

        Uloge.Clear();

        foreach (var uloga in uloge)
            Uloge.Add(uloga);
    }

    private void NoviKorisnik()
    {
        IzabraniKorisnik = null;

        KorisnickoIme = string.Empty;
        Lozinka = string.Empty;
        Ime = string.Empty;
        Prezime = string.Empty;
        Email = string.Empty;
        IzabranaUlogaId = 2;
        Aktivan = true;
        Poruka = "Unesite podatke novog korisnika.";
    }

    private async Task SacuvajAsync()
    {
        Poruka = string.Empty;

        if (string.IsNullOrWhiteSpace(KorisnickoIme) ||
            string.IsNullOrWhiteSpace(Ime) ||
            string.IsNullOrWhiteSpace(Prezime) ||
            string.IsNullOrWhiteSpace(Email))
        {
            Poruka = "Popunite sva obavezna polja.";
            return;
        }

        if (IzabraniKorisnik == null)
        {
            if (string.IsNullOrWhiteSpace(Lozinka))
            {
                Poruka = "Za novog korisnika morate uneti lozinku.";
                return;
            }

            bool postojiKorisnickoIme = await _kontekst.Korisnici
                .AnyAsync(k => k.KorisnickoIme == KorisnickoIme);

            if (postojiKorisnickoIme)
            {
                Poruka = "Korisničko ime već postoji.";
                return;
            }

            var noviKorisnik = new Korisnik
            {
                KorisnickoIme = KorisnickoIme.Trim(),
                LozinkaHash = KreirajHashLozinke(Lozinka),
                Ime = Ime.Trim(),
                Prezime = Prezime.Trim(),
                Email = Email.Trim(),
                UlogaId = IzabranaUlogaId,
                Aktivan = Aktivan,
                DatumKreiranja = DateTime.Now
            };

            _kontekst.Korisnici.Add(noviKorisnik);
        }
        else
        {
            bool postojiDrugoKorisnickoIme = await _kontekst.Korisnici
                .AnyAsync(k =>
                    k.KorisnickoIme == KorisnickoIme &&
                    k.KorisnikId != IzabraniKorisnik.KorisnikId);

            if (postojiDrugoKorisnickoIme)
            {
                Poruka = "Korisničko ime već postoji.";
                return;
            }

            IzabraniKorisnik.KorisnickoIme = KorisnickoIme.Trim();
            IzabraniKorisnik.Ime = Ime.Trim();
            IzabraniKorisnik.Prezime = Prezime.Trim();
            IzabraniKorisnik.Email = Email.Trim();
            IzabraniKorisnik.UlogaId = IzabranaUlogaId;
            IzabraniKorisnik.Aktivan = Aktivan;

            if (!string.IsNullOrWhiteSpace(Lozinka))
            {
                IzabraniKorisnik.LozinkaHash =
                    KreirajHashLozinke(Lozinka);
            }
        }

        await _kontekst.SaveChangesAsync();

        await UcitajPodatkeAsync();

        Poruka = "Podaci su uspešno sačuvani.";
    }

    private async Task ObrisiAsync()
    {
        if (IzabraniKorisnik == null)
        {
            Poruka = "Izaberite korisnika za brisanje.";
            return;
        }

        bool imaPreglede = await _kontekst.Pregledi
            .AnyAsync(p => p.LekarId == IzabraniKorisnik.KorisnikId);

        bool imaDokumente = await _kontekst.Dokumenti
            .AnyAsync(d => d.DodaoKorisnikId == IzabraniKorisnik.KorisnikId);

        if (imaPreglede || imaDokumente)
        {
            Poruka = "Korisnik se ne može obrisati jer je povezan sa podacima sistema. Deaktivirajte ga umesto brisanja.";
            return;
        }

        _kontekst.Korisnici.Remove(IzabraniKorisnik);

        await _kontekst.SaveChangesAsync();

        IzabraniKorisnik = null;

        await UcitajPodatkeAsync();

        Poruka = "Korisnik je obrisan.";
    }

    private async Task AktivirajDeaktivirajAsync()
    {
        if (IzabraniKorisnik == null)
        {
            Poruka = "Izaberite korisnika.";
            return;
        }

        if (IzabraniKorisnik.UlogaId == 1)
        {
            Poruka = "Administrator se ne može deaktivirati.";
            return;
        }

        bool novoStanje = !IzabraniKorisnik.Aktivan;

        IzabraniKorisnik.Aktivan = novoStanje;

        await _kontekst.SaveChangesAsync();

        await UcitajPodatkeAsync();

        Poruka = novoStanje
            ? "Korisnik je aktiviran."
            : "Korisnik je deaktiviran.";
    }

    private string KreirajHashLozinke(string lozinka)
    {
        const int duzinaSalta = 16;
        const int duzinaHasha = 32;

        const int memorija = 19456;
        const int iteracije = 2;
        const int paralelizam = 1;

        byte[] bajtoviLozinke = System.Text.Encoding.UTF8.GetBytes(lozinka);

        byte[] salt = RandomNumberGenerator.GetBytes(duzinaSalta);

        using Argon2id argon2 =
            new Argon2id(bajtoviLozinke)
            {
                Salt = salt,
                MemorySize = memorija,
                Iterations = iteracije,
                DegreeOfParallelism = paralelizam
            };

        byte[] hash = argon2.GetBytes(duzinaHasha);

        string saltBase64 = Convert.ToBase64String(salt);

        string hashBase64 = Convert.ToBase64String(hash);

        return $"argon2id${memorija}${iteracije}${paralelizam}${saltBase64}${hashBase64}";
    }




    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? imeSvojstva = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(imeSvojstva));
    }
}