using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using BolnickiSistem.Data;
using BolnickiSistem.Models;

namespace BolnickiSistem.ViewModels;

public class IzvestajiViewModel : INotifyPropertyChanged
{
    private readonly BolnicaContext _kontekst;

    private int _ukupanBrojPacijenata;
    private int _ukupanBrojLekara;
    private int _ukupanBrojPregleda;
    private int _ukupanBrojLaboratorijskihNalaza;
    private int _ukupanBrojDokumenata;

    private DateTime? _datumOd;
    private DateTime? _datumDo;
    private int _brojPregledaUPeriodu;

    private string _poruka = string.Empty;

    public int UkupanBrojPacijenata
    {
        get => _ukupanBrojPacijenata;
        set
        {
            _ukupanBrojPacijenata = value;
            OnPropertyChanged();
        }
    }

    public int UkupanBrojLekara
    {
        get => _ukupanBrojLekara;
        set
        {
            _ukupanBrojLekara = value;
            OnPropertyChanged();
        }
    }

    public int UkupanBrojPregleda
    {
        get => _ukupanBrojPregleda;
        set
        {
            _ukupanBrojPregleda = value;
            OnPropertyChanged();
        }
    }

    public int UkupanBrojLaboratorijskihNalaza
    {
        get => _ukupanBrojLaboratorijskihNalaza;
        set
        {
            _ukupanBrojLaboratorijskihNalaza = value;
            OnPropertyChanged();
        }
    }

    public int UkupanBrojDokumenata
    {
        get => _ukupanBrojDokumenata;
        set
        {
            _ukupanBrojDokumenata = value;
            OnPropertyChanged();
        }
    }

    public DateTime? DatumOd
    {
        get => _datumOd;
        set
        {
            _datumOd = value;
            OnPropertyChanged();
        }
    }

    public DateTime? DatumDo
    {
        get => _datumDo;
        set
        {
            _datumDo = value;
            OnPropertyChanged();
        }
    }

    public int BrojPregledaUPeriodu
    {
        get => _brojPregledaUPeriodu;
        set
        {
            _brojPregledaUPeriodu = value;
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

    public ObservableCollection<StatistikaLekara> PreglediPoLekaru { get; } = new();

    public ObservableCollection<StarosnaGrupa> PacijentiPoStarosnimGrupama { get; } = new();

    public ObservableCollection<StatistikaPol> PacijentiPoPolu { get; } = new();

    public RelayCommand PrikaziPregledeUPerioduCommand { get; }

    public IzvestajiViewModel(BolnicaContext kontekst)
    {
        _kontekst = kontekst;

        PrikaziPregledeUPerioduCommand =
            new RelayCommand(async _ => await PrikaziPregledeUPerioduAsync());

        DatumOd = DateTime.Today.AddDays(-30);
        DatumDo = DateTime.Today;

        _ = UcitajIzvestajeAsync();
    }

    private async Task UcitajIzvestajeAsync()
    {
        try
        {
            await UcitajStatistikuSistemaAsync();
            await UcitajPregledePoLekaruAsync();
            await UcitajPacijentePoStarosnimGrupamaAsync();
            await UcitajPacijentePoPoluAsync();
            await PrikaziPregledeUPerioduAsync();
        }
        catch (Exception ex)
        {
            Poruka = $"Došlo je do greške prilikom učitavanja izveštaja: {ex.Message}";
        }
    }

    private async Task UcitajStatistikuSistemaAsync()
    {
        UkupanBrojPacijenata = await _kontekst.Pacijenti.CountAsync();

        UkupanBrojLekara = await _kontekst.Korisnici.CountAsync(k => k.UlogaId == 2);

        UkupanBrojPregleda = await _kontekst.Pregledi.CountAsync();

        UkupanBrojLaboratorijskihNalaza = await _kontekst.LaboratorijskiNalazi.CountAsync();

        UkupanBrojDokumenata = await _kontekst.Dokumenti.CountAsync();
    }

    private async Task UcitajPregledePoLekaruAsync()
    {
        PreglediPoLekaru.Clear();

        List<StatistikaLekara> statistika =
            await _kontekst.Pregledi
                .Include(p => p.Lekar)
                .GroupBy(p => new
                {
                    p.LekarId,
                    Ime = p.Lekar.Ime,
                    Prezime = p.Lekar.Prezime
                })
                .Select(g => new StatistikaLekara
                {
                    LekarId = g.Key.LekarId,
                    ImeLekara = g.Key.Ime,
                    PrezimeLekara = g.Key.Prezime,
                    BrojPregleda = g.Count()
                })
                .OrderByDescending(x => x.BrojPregleda)
                .ToListAsync();

        foreach (StatistikaLekara stavka in statistika)
        {
            PreglediPoLekaru.Add(stavka);
        }
    }

    private async Task PrikaziPregledeUPerioduAsync()
    {
        Poruka = string.Empty;

        if (DatumOd == null || DatumDo == null)
        {
            Poruka = "Izaberite početni i krajnji datum.";
            BrojPregledaUPeriodu = 0;
            return;
        }

        if (DatumOd.Value.Date > DatumDo.Value.Date)
        {
            Poruka = "Početni datum ne može biti nakon krajnjeg datuma.";

            BrojPregledaUPeriodu = 0;
            return;
        }

        DateTime datumOd = DatumOd.Value.Date;
        DateTime datumDo = DatumDo.Value.Date.AddDays(1);

        BrojPregledaUPeriodu =
            await _kontekst.Pregledi
                .CountAsync(p =>
                    p.DatumPregleda >= datumOd &&
                    p.DatumPregleda < datumDo);
    }

    private async Task UcitajPacijentePoStarosnimGrupamaAsync()
    {
        PacijentiPoStarosnimGrupama.Clear();

        List<Pacijent> pacijenti =
            await _kontekst.Pacijenti
                .AsNoTracking()
                .ToListAsync();

        DateTime danas = DateTime.Today;

        int grupa0Do18 = 0;
        int grupa19Do30 = 0;
        int grupa31Do50 = 0;
        int grupa51Do65 = 0;
        int grupa66Plus = 0;

        foreach (Pacijent pacijent in pacijenti)
        {
            int godine = danas.Year - pacijent.DatumRodjenja.Year;

            if (pacijent.DatumRodjenja.Date >
                danas.AddYears(-godine))
            {
                godine--;
            }

            if (godine <= 18)
            {
                grupa0Do18++;
            }
            else if (godine <= 30)
            {
                grupa19Do30++;
            }
            else if (godine <= 50)
            {
                grupa31Do50++;
            }
            else if (godine <= 65)
            {
                grupa51Do65++;
            }
            else
            {
                grupa66Plus++;
            }
        }

        PacijentiPoStarosnimGrupama.Add(
            new StarosnaGrupa
            {
                Naziv = "0–18",
                BrojPacijenata = grupa0Do18
            });

        PacijentiPoStarosnimGrupama.Add(
            new StarosnaGrupa
            {
                Naziv = "19–30",
                BrojPacijenata = grupa19Do30
            });

        PacijentiPoStarosnimGrupama.Add(
            new StarosnaGrupa
            {
                Naziv = "31–50",
                BrojPacijenata = grupa31Do50
            });

        PacijentiPoStarosnimGrupama.Add(
            new StarosnaGrupa
            {
                Naziv = "51–65",
                BrojPacijenata = grupa51Do65
            });

        PacijentiPoStarosnimGrupama.Add(
            new StarosnaGrupa
            {
                Naziv = "66+",
                BrojPacijenata = grupa66Plus
            });
    }

    private async Task UcitajPacijentePoPoluAsync()
    {
        PacijentiPoPolu.Clear();

        List<StatistikaPol> statistika =
            await _kontekst.Pacijenti
                .AsNoTracking()
                .GroupBy(p => p.Pol)
                .Select(g => new StatistikaPol
                {
                    Pol = g.Key,
                    BrojPacijenata = g.Count()
                })
                .OrderBy(x => x.Pol)
                .ToListAsync();

        foreach (StatistikaPol stavka in statistika)
        {
            PacijentiPoPolu.Add(stavka);
        }
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


public class StatistikaLekara
{
    public int LekarId { get; set; }

    public string ImeLekara { get; set; } = string.Empty;

    public string PrezimeLekara { get; set; } = string.Empty;

    public string PunoImeLekara => $"{ImeLekara} {PrezimeLekara}";

    public int BrojPregleda { get; set; }
}


public class StarosnaGrupa
{
    public string Naziv { get; set; } = string.Empty;

    public int BrojPacijenata { get; set; }
}


public class StatistikaPol
{
    public string Pol { get; set; } = string.Empty;

    public int BrojPacijenata { get; set; }
}