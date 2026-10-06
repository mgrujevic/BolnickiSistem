using BolnickiSistem.Data;
using BolnickiSistem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;

namespace BolnickiSistem.ViewModels;

public class ZdravstveniKartonViewModel : INotifyPropertyChanged
{
    private readonly BolnicaContext _kontekst;
    private readonly int _lekarId;

    private Pacijent? _pacijent;
    private Pregled? _izabraniPregled;
    private Pregled? _izabraniPregledZaLaboratorijskiNalaz;
    private Pregled? _izabraniPregledZaDokument;

    private DateTime _datumNovogPregleda = DateTime.Today;
    private string _razlogDolaska = string.Empty;
    private string? _simptomi;
    private string? _nalaz;
    private string? _preporuka;
    private DateTime? _datumKontrole;

    private string _novaDijagnozaSifra = string.Empty;
    private string _novaDijagnozaNaziv = string.Empty;
    private string? _novaDijagnozaOpis;

    private string _novaTerapijaNazivLeka = string.Empty;
    private string _novaTerapijaDoziranje = string.Empty;
    private string _novaTerapijaUcestalost = string.Empty;
    private DateTime _novaTerapijaDatumPocetka = DateTime.Today;
    private DateTime? _novaTerapijaDatumZavrsetka;
    private string? _novaTerapijaUputstvo;

    // =========================================================
    // PODACI ZA NOVI LABORATORIJSKI NALAZ
    // =========================================================

    private string _noviLaboratorijskiNalazNazivAnalize = string.Empty;
    private string _noviLaboratorijskiNalazRezultat = string.Empty;
    private string? _noviLaboratorijskiNalazJedinica;
    private string? _noviLaboratorijskiNalazReferentnaVrednost;
    private DateTime _noviLaboratorijskiNalazDatumAnalize = DateTime.Today;
    private string? _noviLaboratorijskiNalazNapomena;


    private string _noviDokumentNaziv = string.Empty;

    private string _noviDokumentTip = string.Empty;

    private string _noviDokumentPutanja = string.Empty;


    public Pacijent? Pacijent
    {
        get => _pacijent;
        private set
        {
            _pacijent = value;
            OnPropertyChanged();
        }
    }


    public Pregled? IzabraniPregled
    {
        get => _izabraniPregled;
        set
        {
            _izabraniPregled = value;
            OnPropertyChanged();
        }
    }



    public Pregled? IzabraniPregledZaLaboratorijskiNalaz
    {
        get => _izabraniPregledZaLaboratorijskiNalaz;
        set
        {
            _izabraniPregledZaLaboratorijskiNalaz = value;
            OnPropertyChanged();
        }
    }



    public DateTime DatumNovogPregleda
    {
        get => _datumNovogPregleda;
        set
        {
            _datumNovogPregleda = value;
            OnPropertyChanged();
        }
    }

    public string RazlogDolaska
    {
        get => _razlogDolaska;
        set
        {
            _razlogDolaska = value;
            OnPropertyChanged();
        }
    }

    public string? Simptomi
    {
        get => _simptomi;
        set
        {
            _simptomi = value;
            OnPropertyChanged();
        }
    }

    public string? Nalaz
    {
        get => _nalaz;
        set
        {
            _nalaz = value;
            OnPropertyChanged();
        }
    }

    public string? Preporuka
    {
        get => _preporuka;
        set
        {
            _preporuka = value;
            OnPropertyChanged();
        }
    }

    public DateTime? DatumKontrole
    {
        get => _datumKontrole;
        set
        {
            _datumKontrole = value;
            OnPropertyChanged();
        }
    }


    public string NovaDijagnozaSifra
    {
        get => _novaDijagnozaSifra;
        set
        {
            _novaDijagnozaSifra = value;
            OnPropertyChanged();
        }
    }

    public string NovaDijagnozaNaziv
    {
        get => _novaDijagnozaNaziv;
        set
        {
            _novaDijagnozaNaziv = value;
            OnPropertyChanged();
        }
    }

    public string? NovaDijagnozaOpis
    {
        get => _novaDijagnozaOpis;
        set
        {
            _novaDijagnozaOpis = value;
            OnPropertyChanged();
        }
    }



    public string NovaTerapijaNazivLeka
    {
        get => _novaTerapijaNazivLeka;
        set
        {
            _novaTerapijaNazivLeka = value;
            OnPropertyChanged();
        }
    }

    public string NovaTerapijaDoziranje
    {
        get => _novaTerapijaDoziranje;
        set
        {
            _novaTerapijaDoziranje = value;
            OnPropertyChanged();
        }
    }

    public string NovaTerapijaUcestalost
    {
        get => _novaTerapijaUcestalost;
        set
        {
            _novaTerapijaUcestalost = value;
            OnPropertyChanged();
        }
    }

    public DateTime NovaTerapijaDatumPocetka
    {
        get => _novaTerapijaDatumPocetka;
        set
        {
            _novaTerapijaDatumPocetka = value;
            OnPropertyChanged();
        }
    }

    public DateTime? NovaTerapijaDatumZavrsetka
    {
        get => _novaTerapijaDatumZavrsetka;
        set
        {
            _novaTerapijaDatumZavrsetka = value;
            OnPropertyChanged();
        }
    }

    public string? NovaTerapijaUputstvo
    {
        get => _novaTerapijaUputstvo;
        set
        {
            _novaTerapijaUputstvo = value;
            OnPropertyChanged();
        }
    }



    public string NoviLaboratorijskiNalazNazivAnalize
    {
        get => _noviLaboratorijskiNalazNazivAnalize;
        set
        {
            _noviLaboratorijskiNalazNazivAnalize = value;
            OnPropertyChanged();
        }
    }

    public string NoviLaboratorijskiNalazRezultat
    {
        get => _noviLaboratorijskiNalazRezultat;
        set
        {
            _noviLaboratorijskiNalazRezultat = value;
            OnPropertyChanged();
        }
    }

    public string? NoviLaboratorijskiNalazJedinica
    {
        get => _noviLaboratorijskiNalazJedinica;
        set
        {
            _noviLaboratorijskiNalazJedinica = value;
            OnPropertyChanged();
        }
    }

    public string? NoviLaboratorijskiNalazReferentnaVrednost
    {
        get => _noviLaboratorijskiNalazReferentnaVrednost;
        set
        {
            _noviLaboratorijskiNalazReferentnaVrednost = value;
            OnPropertyChanged();
        }
    }

    public DateTime NoviLaboratorijskiNalazDatumAnalize
    {
        get => _noviLaboratorijskiNalazDatumAnalize;
        set
        {
            _noviLaboratorijskiNalazDatumAnalize = value;
            OnPropertyChanged();
        }
    }

    public string? NoviLaboratorijskiNalazNapomena
    {
        get => _noviLaboratorijskiNalazNapomena;
        set
        {
            _noviLaboratorijskiNalazNapomena = value;
            OnPropertyChanged();
        }
    }

    public Pregled? IzabraniPregledZaDokument
    {
        get => _izabraniPregledZaDokument;

        set
        {
            _izabraniPregledZaDokument = value;
            OnPropertyChanged();
        }
    }

    public string NoviDokumentNaziv
    {
        get => _noviDokumentNaziv;

        set
        {
            _noviDokumentNaziv = value;
            OnPropertyChanged();
        }
    }

    public string NoviDokumentTip
    {
        get => _noviDokumentTip;

        set
        {
            _noviDokumentTip = value;
            OnPropertyChanged();
        }
    }

    public string NoviDokumentPutanja
    {
        get => _noviDokumentPutanja;

        set
        {
            _noviDokumentPutanja = value;
            OnPropertyChanged();
        }
    }



    public ObservableCollection<Pregled> Pregledi { get; } = new();

    public ObservableCollection<LaboratorijskiNalaz> LaboratorijskiNalazi { get; } = new();


    public ObservableCollection<Dokument> Dokumenti { get; } = new();

 

    public ICommand SacuvajNoviPregledCommand { get; }

    public ICommand SacuvajDijagnozuCommand { get; }

    public ICommand SacuvajTerapijuCommand { get; }

    public ICommand SacuvajLaboratorijskiNalazCommand { get; }

    public ICommand SacuvajDokumentCommand { get; }




    public ZdravstveniKartonViewModel(
        BolnicaContext kontekst,
        int lekarId)
    {
        _kontekst = kontekst;
        _lekarId = lekarId;

        SacuvajNoviPregledCommand =
            new RelayCommand(
                async _ => await SacuvajNoviPregledAsync());

        SacuvajDijagnozuCommand =
            new RelayCommand(
                async _ => await SacuvajDijagnozuAsync());

        SacuvajTerapijuCommand =
            new RelayCommand(
                async _ => await SacuvajTerapijuAsync());

        SacuvajLaboratorijskiNalazCommand =
            new RelayCommand(
                async _ => await SacuvajLaboratorijskiNalazAsync());

        SacuvajDokumentCommand = new RelayCommand(async _ => await SacuvajDokumentAsync());

    }


    public async Task UcitajPacijentaAsync(int pacijentId)
    {
        Pacijent? pacijent = await _kontekst.Pacijenti
                                    .Include(p => p.Lekar)
                                    .FirstOrDefaultAsync(p =>
                                        p.PacijentId == pacijentId &&
                                        p.LekarId == _lekarId);

        if (pacijent == null)
        {
            Pacijent = null;

            Pregledi.Clear();

            LaboratorijskiNalazi.Clear();

            IzabraniPregled = null;

            IzabraniPregledZaLaboratorijskiNalaz = null;

            return;
        }

        Pacijent = pacijent;

        await UcitajPregledeAsync(pacijentId);

        await UcitajLaboratorijskeNalazeAsync(pacijentId);

        await UcitajDokumenteAsync(pacijentId);

        OcistiFormuNovogPregleda();

        OcistiFormuLaboratorijskogNalaza();

        OcistiFormuDokumenta();
    }

    private async Task UcitajPregledeAsync(int pacijentId)
    {
        List<Pregled> pregledi =
            await _kontekst.Pregledi
                .Include(p => p.Lekar)
                .Include(p => p.Dijagnoze)
                .Include(p => p.Terapije)
                .Where(p => p.PacijentId == pacijentId)
                .OrderByDescending(p => p.DatumPregleda)
                .ToListAsync();

        Pregledi.Clear();

        foreach (Pregled pregled in pregledi)
        {
            Pregledi.Add(pregled);
        }

        IzabraniPregled = null;
    }


    private async Task UcitajLaboratorijskeNalazeAsync(int pacijentId)
    {
        List<LaboratorijskiNalaz> nalazi =
            await _kontekst.LaboratorijskiNalazi
                .Include(n => n.Pregled)
                .Where(n => n.PacijentId == pacijentId)
                .OrderByDescending(n => n.DatumAnalize)
                .ToListAsync();

        LaboratorijskiNalazi.Clear();

        foreach (LaboratorijskiNalaz nalaz in nalazi)
        {
            LaboratorijskiNalazi.Add(nalaz);
        }
    }


    private async Task SacuvajNoviPregledAsync()
    {
        if (Pacijent == null)
            return;

        if (string.IsNullOrWhiteSpace(RazlogDolaska))
            return;

        Pregled noviPregled = new Pregled
        {
            PacijentId = Pacijent.PacijentId,
            LekarId = _lekarId,

            DatumPregleda = DatumNovogPregleda,
            RazlogDolaska = RazlogDolaska.Trim(),

            Simptomi =
                string.IsNullOrWhiteSpace(Simptomi)
                    ? null
                    : Simptomi.Trim(),

            Nalaz =
                string.IsNullOrWhiteSpace(Nalaz)
                    ? null
                    : Nalaz.Trim(),

            Preporuka =
                string.IsNullOrWhiteSpace(Preporuka)
                    ? null
                    : Preporuka.Trim(),

            DatumKontrole = DatumKontrole
        };

        _kontekst.Pregledi.Add(noviPregled);

        await _kontekst.SaveChangesAsync();

        Pregledi.Insert(0, noviPregled);

        IzabraniPregled = noviPregled;

        OcistiFormuNovogPregleda();
    }



    private async Task SacuvajDijagnozuAsync()
    {
        if (IzabraniPregled == null || Pacijent == null)
            return;

        if (string.IsNullOrWhiteSpace(NovaDijagnozaSifra))
            return;

        if (string.IsNullOrWhiteSpace(NovaDijagnozaNaziv))
            return;

        int pregledId = IzabraniPregled.PregledId;
        int pacijentId = Pacijent.PacijentId;

        Dijagnoza novaDijagnoza = new Dijagnoza
        {
            PregledId = pregledId,

            Sifra = NovaDijagnozaSifra.Trim(),

            Naziv = NovaDijagnozaNaziv.Trim(),

            Opis =
                string.IsNullOrWhiteSpace(NovaDijagnozaOpis)
                    ? null
                    : NovaDijagnozaOpis.Trim()
        };

        _kontekst.Dijagnoze.Add(novaDijagnoza);

        await _kontekst.SaveChangesAsync();

        await UcitajPregledeAsync(pacijentId);

        IzabraniPregled =
            Pregledi.FirstOrDefault(
                p => p.PregledId == pregledId);

        NovaDijagnozaSifra = string.Empty;
        NovaDijagnozaNaziv = string.Empty;
        NovaDijagnozaOpis = string.Empty;
    }




    private async Task SacuvajTerapijuAsync()
    {
        if (IzabraniPregled == null)
            return;

        if (string.IsNullOrWhiteSpace(NovaTerapijaNazivLeka))
            return;

        if (string.IsNullOrWhiteSpace(NovaTerapijaDoziranje))
            return;

        if (string.IsNullOrWhiteSpace(NovaTerapijaUcestalost))
            return;

        if (NovaTerapijaDatumZavrsetka.HasValue &&
            NovaTerapijaDatumZavrsetka.Value.Date <
            NovaTerapijaDatumPocetka.Date)
        {
            return;
        }

        int pregledId = IzabraniPregled.PregledId;
        int pacijentId = Pacijent!.PacijentId;

        Terapija novaTerapija = new Terapija
        {
            PregledId = IzabraniPregled.PregledId,

            NazivLeka = NovaTerapijaNazivLeka.Trim(),

            Doziranje = NovaTerapijaDoziranje.Trim(),

            Ucestalost = NovaTerapijaUcestalost.Trim(),

            DatumPocetka = NovaTerapijaDatumPocetka,

            DatumZavrsetka = NovaTerapijaDatumZavrsetka,

            Uputstvo = string.IsNullOrWhiteSpace(NovaTerapijaUputstvo) ? null : NovaTerapijaUputstvo.Trim()
        };

        _kontekst.Terapije.Add(novaTerapija);

        await _kontekst.SaveChangesAsync();

        await UcitajPregledeAsync(pacijentId);

        IzabraniPregled = Pregledi.FirstOrDefault(p => p.PregledId == pregledId);

        NovaTerapijaNazivLeka = string.Empty;
        NovaTerapijaDoziranje = string.Empty;
        NovaTerapijaUcestalost = string.Empty;
        NovaTerapijaDatumPocetka = DateTime.Today;
        NovaTerapijaDatumZavrsetka = null;
        NovaTerapijaUputstvo = null;
    }


    private async Task SacuvajLaboratorijskiNalazAsync()
    {
        if (Pacijent == null)
            return;

        if (string.IsNullOrWhiteSpace(NoviLaboratorijskiNalazNazivAnalize))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(NoviLaboratorijskiNalazRezultat))
        {
            return;
        }

        LaboratorijskiNalaz noviNalaz =
            new LaboratorijskiNalaz
            {
                PacijentId = Pacijent.PacijentId,

                PregledId = IzabraniPregledZaLaboratorijskiNalaz?.PregledId,

                NazivAnalize = NoviLaboratorijskiNalazNazivAnalize.Trim(),

                Rezultat = NoviLaboratorijskiNalazRezultat.Trim(),

                Jedinica =
                    string.IsNullOrWhiteSpace(
                        NoviLaboratorijskiNalazJedinica)
                        ? null
                        : NoviLaboratorijskiNalazJedinica.Trim(),

                ReferentnaVrednost =
                    string.IsNullOrWhiteSpace(
                        NoviLaboratorijskiNalazReferentnaVrednost)
                        ? null
                        : NoviLaboratorijskiNalazReferentnaVrednost.Trim(),

                DatumAnalize =
                    NoviLaboratorijskiNalazDatumAnalize,

                Napomena =
                    string.IsNullOrWhiteSpace(
                        NoviLaboratorijskiNalazNapomena)
                        ? null
                        : NoviLaboratorijskiNalazNapomena.Trim()
            };

        _kontekst.LaboratorijskiNalazi.Add(noviNalaz);

        await _kontekst.SaveChangesAsync();

        await UcitajLaboratorijskeNalazeAsync(Pacijent.PacijentId);

        OcistiFormuLaboratorijskogNalaza();
    }


    private async Task UcitajDokumenteAsync(int pacijentId)
    {
        List<Dokument> dokumenti = await _kontekst.Dokumenti
                                    .Include(d => d.Pregled)
                                    .Include(d => d.DodaoKorisnik)
                                    .Where(d => d.PacijentId == pacijentId)
                                    .OrderByDescending(d => d.DatumDodavanja)
                                    .ToListAsync();


        Dokumenti.Clear();

        foreach (Dokument dokument in dokumenti)
        {
            Dokumenti.Add(dokument);
        }
    }

    private async Task SacuvajDokumentAsync()
    {
        if (Pacijent == null)
            return;

        string naziv = NoviDokumentNaziv?.Trim() ?? string.Empty;

        string tip = NoviDokumentTip?.Trim() ?? string.Empty;

        string izvornaPutanja =
            NoviDokumentPutanja?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(naziv))
            return;

        if (string.IsNullOrWhiteSpace(tip))
            return;

        if (string.IsNullOrWhiteSpace(izvornaPutanja))
            return;

        if (!File.Exists(izvornaPutanja))
            return;

        string folderDokumenata =
            Path.Combine(
                AppContext.BaseDirectory,
                "Dokumenti",
                Pacijent.BrojZdravstveneKartice);

        Directory.CreateDirectory(folderDokumenata);

        string imeFajla =
            Path.GetFileName(izvornaPutanja);

        string destinacija =
            Path.Combine(
                folderDokumenata,
                imeFajla);

        if (!string.Equals(
                Path.GetFullPath(izvornaPutanja),
                Path.GetFullPath(destinacija),
                StringComparison.OrdinalIgnoreCase))
        {
            if (File.Exists(destinacija))
            {
                string nazivBezEkstenzije =
                    Path.GetFileNameWithoutExtension(imeFajla);

                string ekstenzija =
                    Path.GetExtension(imeFajla);

                imeFajla =
                    $"{nazivBezEkstenzije}_{DateTime.Now:yyyyMMdd_HHmmss}{ekstenzija}";

                destinacija =
                    Path.Combine(
                        folderDokumenata,
                        imeFajla);
            }

            File.Copy(
                izvornaPutanja,
                destinacija,
                false);
        }

        string relativnaPutanja =
            Path.Combine(
                "Dokumenti",
                Pacijent.BrojZdravstveneKartice,
                imeFajla);

        relativnaPutanja =
            relativnaPutanja.Replace(
                Path.DirectorySeparatorChar,
                '\\');

        Dokument noviDokument =
            new Dokument
            {
                PacijentId = Pacijent.PacijentId,

                PregledId = IzabraniPregledZaDokument?.PregledId,

                Naziv = naziv,

                TipDokumenta = tip,

                PutanjaDoFajla = relativnaPutanja,

                DatumDodavanja = DateTime.Now,

                DodaoKorisnikId = _lekarId
            };

        _kontekst.Dokumenti.Add(noviDokument);

        await _kontekst.SaveChangesAsync();

        await UcitajDokumenteAsync(Pacijent.PacijentId);

        OcistiFormuDokumenta();
    }


    private void OcistiFormuDokumenta()
    {
        NoviDokumentNaziv = string.Empty;

        NoviDokumentTip = string.Empty;

        NoviDokumentPutanja = string.Empty;

        IzabraniPregledZaDokument = null;
    }




    private void OcistiFormuNovogPregleda()
    {
        DatumNovogPregleda = DateTime.Today;

        RazlogDolaska = string.Empty;

        Simptomi = null;

        Nalaz = null;

        Preporuka = null;

        DatumKontrole = null;
    }


    private void OcistiFormuLaboratorijskogNalaza()
    {
        NoviLaboratorijskiNalazNazivAnalize = string.Empty;

        NoviLaboratorijskiNalazRezultat = string.Empty;

        NoviLaboratorijskiNalazJedinica = null;

        NoviLaboratorijskiNalazReferentnaVrednost = null;

        NoviLaboratorijskiNalazDatumAnalize = DateTime.Today;

        NoviLaboratorijskiNalazNapomena = null;

        IzabraniPregledZaLaboratorijskiNalaz = null;
    }


   

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? naziv = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(naziv));
    }
}