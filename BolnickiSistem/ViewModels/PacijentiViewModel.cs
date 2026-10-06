using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using BolnickiSistem.Data;
using BolnickiSistem.Models;

namespace BolnickiSistem.ViewModels;

public class PacijentiViewModel : INotifyPropertyChanged
{
    private readonly BolnicaContext _kontekst;

    private Pacijent? _izabraniPacijent;

    private string _brojZdravstveneKartice = string.Empty;
    private string _jmbg = string.Empty;
    private string _ime = string.Empty;
    private string _prezime = string.Empty;
    private DateTime? _datumRodjenja;
    private string _pol = string.Empty;
    private string _adresa = string.Empty;
    private string _telefon = string.Empty;
    private string _email = string.Empty;
    private string _krvnaGrupa = string.Empty;
    private string _alergije = string.Empty;
    private string _hronicneBolesti = string.Empty;
    private string _pretraga = string.Empty;
    private string _poruka = string.Empty;

    public ObservableCollection<Pacijent> Pacijenti { get; } = new();

    public ObservableCollection<Korisnik> Lekari { get; }
        = new();

    public ObservableCollection<string> Polovi { get; } = new()
    {
        "Muški",
        "Ženski"
    };

    public ObservableCollection<string> KrvneGrupe { get; } = new()
    {
        "A+",
        "A-",
        "B+",
        "B-",
        "AB+",
        "AB-",
        "0+",
        "0-",
        "null"
    };


    public Pacijent? IzabraniPacijent
    {
        get => _izabraniPacijent;

        set
        {
            _izabraniPacijent = value;

            if (value != null)
            {
                UcitajPodatkePacijenta(value);
            }

            OnPropertyChanged();
        }
    }

    private int? _izabraniLekarId;

    public int? IzabraniLekarId
    {
        get => _izabraniLekarId;
        set
        {
            _izabraniLekarId = value;
            OnPropertyChanged();
        }
    }


    public string BrojZdravstveneKartice
    {
        get => _brojZdravstveneKartice;
        set
        {
            _brojZdravstveneKartice = value;
            OnPropertyChanged();
        }
    }


    public string JMBG
    {
        get => _jmbg;
        set
        {
            _jmbg = value;
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


    public DateTime? DatumRodjenja
    {
        get => _datumRodjenja;
        set
        {
            _datumRodjenja = value;
            OnPropertyChanged();
        }
    }


    public string Pol
    {
        get => _pol;
        set
        {
            _pol = value;
            OnPropertyChanged();
        }
    }


    public string Adresa
    {
        get => _adresa;
        set
        {
            _adresa = value;
            OnPropertyChanged();
        }
    }


    public string Telefon
    {
        get => _telefon;
        set
        {
            _telefon = value;
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


    public string KrvnaGrupa
    {
        get => _krvnaGrupa;
        set
        {
            _krvnaGrupa = value;
            OnPropertyChanged();
        }
    }


    public string Alergije
    {
        get => _alergije;
        set
        {
            _alergije = value;
            OnPropertyChanged();
        }
    }


    public string HronicneBolesti
    {
        get => _hronicneBolesti;
        set
        {
            _hronicneBolesti = value;
            OnPropertyChanged();
        }
    }


    public string Pretraga
    {
        get => _pretraga;
        set
        {
            _pretraga = value;
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


    public RelayCommand NoviPacijentCommand { get; }

    public RelayCommand SacuvajCommand { get; }

    public RelayCommand ObrisiCommand { get; }

    public RelayCommand PretraziCommand { get; }
        

    public PacijentiViewModel(BolnicaContext kontekst)
    {
        _kontekst = kontekst;

        NoviPacijentCommand = new RelayCommand(_ => NoviPacijent());

        SacuvajCommand = new RelayCommand(async _ => await SacuvajAsync());

        ObrisiCommand = new RelayCommand(async _ => await ObrisiAsync());

        PretraziCommand = new RelayCommand(async _ => await UcitajPacijenteAsync());

        _ = UcitajPacijenteAsync();
    }


    private async Task UcitajPacijenteAsync()
    {
        try
        {
            _izabraniPacijent = null;
            OnPropertyChanged(nameof(IzabraniPacijent));

            IQueryable<Pacijent> upit = _kontekst.Pacijenti;

            if (!string.IsNullOrWhiteSpace(Pretraga))
            {
                string pretraga = Pretraga.Trim();

                upit = upit.Where(p =>
                    p.Ime.Contains(pretraga) ||
                    p.Prezime.Contains(pretraga) ||
                    p.JMBG.Contains(pretraga) ||
                    p.BrojZdravstveneKartice.Contains(pretraga));
            }

            List<Pacijent> pacijenti = await upit
                .OrderBy(p => p.Prezime)
                .ThenBy(p => p.Ime)
                .ToListAsync();

            Pacijenti.Clear();

            foreach (Pacijent pacijent in pacijenti)
            {
                Pacijenti.Add(pacijent);
            }

            var lekari = await _kontekst.Korisnici
                .Where(k => k.UlogaId == 2 && k.Aktivan)
                .OrderBy(k => k.Prezime)
                .ThenBy(k => k.Ime)
                .ToListAsync(); 

            Lekari.Clear();

            foreach (var lekar in lekari)
            {
                Lekari.Add(lekar);
            }
        }
        catch (Exception)
        {
            Poruka = "Došlo je do greške prilikom učitavanja pacijenata.";
        }
    }


    private void UcitajPodatkePacijenta(Pacijent pacijent)
    {
        BrojZdravstveneKartice = pacijent.BrojZdravstveneKartice;
        JMBG = pacijent.JMBG;
        Ime = pacijent.Ime;
        Prezime = pacijent.Prezime;
        DatumRodjenja = pacijent.DatumRodjenja;
        Pol = pacijent.Pol;
        Adresa = pacijent.Adresa ?? string.Empty;
        Telefon = pacijent.Telefon ?? string.Empty;
        Email = pacijent.Email ?? string.Empty;
        KrvnaGrupa = pacijent.KrvnaGrupa ?? string.Empty;
        Alergije = pacijent.Alergije ?? string.Empty;
        HronicneBolesti = pacijent.HronicneBolesti ?? string.Empty;
        IzabraniLekarId = pacijent.LekarId;

        Poruka = string.Empty;
    }


    private void NoviPacijent()
    {
        OcistiFormu();

        Poruka = "Unesite podatke za novog pacijenta.";
    }


    private async Task SacuvajAsync()
    {
        Poruka = string.Empty;

        string brojKartice = BrojZdravstveneKartice?.Trim() ?? string.Empty;
        string jmbg = JMBG?.Trim() ?? string.Empty;
        string ime = Ime?.Trim() ?? string.Empty;
        string prezime = Prezime?.Trim() ?? string.Empty;
        string pol = Pol?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(brojKartice))
        {
            Poruka = "Nedostaje broj zdravstvene kartice.";
            return;
        }

        if (string.IsNullOrWhiteSpace(jmbg))
        {
            Poruka = "Nedostaje JMBG.";
            return;
        }

        if (string.IsNullOrWhiteSpace(ime))
        {
            Poruka = "Nedostaje ime.";
            return;
        }

        if (string.IsNullOrWhiteSpace(prezime))
        {
            Poruka = "Nedostaje prezime.";
            return;
        }

        if (DatumRodjenja == null)
        {
            Poruka = "Nedostaje datum rođenja.";
            return;
        }

        if (string.IsNullOrWhiteSpace(pol))
        {
            Poruka = "Nedostaje pol.";
            return;
        }

        if (jmbg.Length != 13 || !jmbg.All(char.IsDigit))
        {
            Poruka = "JMBG mora sadržati tačno 13 cifara.";
            return;
        }

        if (DatumRodjenja.Value.Date > DateTime.Today)
        {
            Poruka = "Datum rođenja ne može biti u budućnosti.";
            return;
        }

        int izabraniPacijentId = _izabraniPacijent?.PacijentId ?? 0;

        bool postojiJmbg =
            await _kontekst.Pacijenti.AnyAsync(p =>
                p.JMBG == jmbg &&
                p.PacijentId != izabraniPacijentId);

        if (postojiJmbg)
        {
            Poruka = "Pacijent sa ovim JMBG-om već postoji.";
            return;
        }

        bool postojiBrojKartice =
            await _kontekst.Pacijenti.AnyAsync(p =>
                p.BrojZdravstveneKartice == brojKartice &&
                p.PacijentId != izabraniPacijentId);

        if (postojiBrojKartice)
        {
            Poruka = "Pacijent sa ovim brojem zdravstvene kartice već postoji.";
            return;
        }

        try
        {
            if (_izabraniPacijent == null)
            {
                Pacijent pacijent = new Pacijent
                {
                    BrojZdravstveneKartice = brojKartice,
                    JMBG = jmbg,
                    Ime = ime,
                    Prezime = prezime,
                    DatumRodjenja = DatumRodjenja.Value,
                    Pol = pol,

                    Adresa = string.IsNullOrWhiteSpace(Adresa)
                        ? null
                        : Adresa.Trim(),

                    Telefon = string.IsNullOrWhiteSpace(Telefon)
                        ? null
                        : Telefon.Trim(),

                    Email = string.IsNullOrWhiteSpace(Email)
                        ? null
                        : Email.Trim(),

                    KrvnaGrupa = string.IsNullOrWhiteSpace(KrvnaGrupa)
                        ? null
                        : KrvnaGrupa,

                    Alergije = string.IsNullOrWhiteSpace(Alergije)
                        ? null
                        : Alergije.Trim(),

                    HronicneBolesti = string.IsNullOrWhiteSpace(HronicneBolesti)
                        ? null
                        : HronicneBolesti.Trim(),

                    LekarId = IzabraniLekarId
                };

                _kontekst.Pacijenti.Add(pacijent);

                int brojIzmena = await _kontekst.SaveChangesAsync();

                if (brojIzmena > 0)
                {
                    Poruka = "Pacijent je uspešno dodat.";
                }
                else
                {
                    Poruka = "Pacijent nije dodat u bazu.";
                    return;
                }
            }
            else
            {
                _izabraniPacijent.BrojZdravstveneKartice = brojKartice;
                _izabraniPacijent.JMBG = jmbg;
                _izabraniPacijent.Ime = ime;
                _izabraniPacijent.Prezime = prezime;
                _izabraniPacijent.DatumRodjenja = DatumRodjenja.Value;
                _izabraniPacijent.Pol = pol;
                _izabraniPacijent.LekarId = IzabraniLekarId;


                _izabraniPacijent.Adresa =
                    string.IsNullOrWhiteSpace(Adresa)
                        ? null
                        : Adresa.Trim();

                _izabraniPacijent.Telefon =
                    string.IsNullOrWhiteSpace(Telefon)
                        ? null
                        : Telefon.Trim();

                _izabraniPacijent.Email =
                    string.IsNullOrWhiteSpace(Email)
                        ? null
                        : Email.Trim();

                _izabraniPacijent.KrvnaGrupa =
                    string.IsNullOrWhiteSpace(KrvnaGrupa)
                        ? null
                        : KrvnaGrupa;

                _izabraniPacijent.Alergije =
                    string.IsNullOrWhiteSpace(Alergije)
                        ? null
                        : Alergije.Trim();

                _izabraniPacijent.HronicneBolesti =
                    string.IsNullOrWhiteSpace(HronicneBolesti)
                        ? null
                        : HronicneBolesti.Trim();

                await _kontekst.SaveChangesAsync();


                Poruka = "Podaci o pacijentu su uspešno izmenjeni.";
            }

            await UcitajPacijenteAsync();

            NoviPacijent();
        }
        catch (Exception ex)
        {
            Poruka = $"Greška: {ex.Message}";
        }
    }


    private async Task ObrisiAsync()
    {
        if (_izabraniPacijent == null)
        {
            Poruka = "Izaberite pacijenta.";
            return;
        }


        int pacijentId =
            _izabraniPacijent.PacijentId;


        bool imaPreglede =
            await _kontekst.Pregledi
                .AnyAsync(p => p.PacijentId == pacijentId);

        bool imaLaboratorijskeNalaze =
            await _kontekst.LaboratorijskiNalazi
                .AnyAsync(l => l.PacijentId == pacijentId);

        bool imaDokumente =
            await _kontekst.Dokumenti
                .AnyAsync(d => d.PacijentId == pacijentId);


        if (imaPreglede ||
            imaLaboratorijskeNalaze ||
            imaDokumente)
        {
            Poruka = "Pacijent se ne može obrisati jer postoje povezani podaci.";
            return;
        }


        Pacijent? pacijent =
            await _kontekst.Pacijenti
                .FirstOrDefaultAsync(p =>
                    p.PacijentId == pacijentId);

        if (pacijent == null)
        {
            Poruka = "Pacijent nije pronađen.";
            return;
        }


        _kontekst.Pacijenti.Remove(pacijent);

        await _kontekst.SaveChangesAsync();

        IzabraniPacijent = null;

        NoviPacijent();

        await UcitajPacijenteAsync();

        Poruka = "Pacijent je uspešno obrisan.";
    }


    private void OcistiFormu()
    {
        _izabraniPacijent = null;
        OnPropertyChanged(nameof(IzabraniPacijent));

        BrojZdravstveneKartice = string.Empty;
        JMBG = string.Empty;
        Ime = string.Empty;
        Prezime = string.Empty;
        DatumRodjenja = null;
        Pol = string.Empty;
        IzabraniLekarId = null;
        Adresa = string.Empty;
        Telefon = string.Empty;
        Email = string.Empty;
        KrvnaGrupa = string.Empty;
        Alergije = string.Empty;
        HronicneBolesti = string.Empty;

    }


    public event PropertyChangedEventHandler? PropertyChanged;


    private void OnPropertyChanged(
        [CallerMemberName] string? nazivSvojstva = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nazivSvojstva));
    }
}