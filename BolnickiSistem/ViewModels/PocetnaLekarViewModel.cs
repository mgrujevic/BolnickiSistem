using BolnickiSistem.Data;
using BolnickiSistem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BolnickiSistem.ViewModels;

public class PocetnaLekarViewModel : INotifyPropertyChanged
{
    private readonly BolnicaContext _kontekst;
    private readonly int _lekarId;

    private string _pozdravnaPoruka = string.Empty;

    private int _brojPacijenata;
    private int _brojPregleda;
    private int _brojPregledaDanas;
    private int _brojPredstojecihKontrola;

    public string PozdravnaPoruka
    {
        get => _pozdravnaPoruka;
        set
        {
            _pozdravnaPoruka = value;
            OnPropertyChanged();
        }
    }

    public int BrojPacijenata
    {
        get => _brojPacijenata;
        set
        {
            _brojPacijenata = value;
            OnPropertyChanged();
        }
    }

    public int BrojPregleda
    {
        get => _brojPregleda;
        set
        {
            _brojPregleda = value;
            OnPropertyChanged();
        }
    }

    public int BrojPregledaDanas
    {
        get => _brojPregledaDanas;
        set
        {
            _brojPregledaDanas = value;
            OnPropertyChanged();
        }
    }

    public int BrojPredstojecihKontrola
    {
        get => _brojPredstojecihKontrola;
        set
        {
            _brojPredstojecihKontrola = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Pregled> PoslednjiPregledi { get; set; }
        = new();

    public ObservableCollection<Pregled> PredstojeceKontrole { get; set; }
        = new();


    public PocetnaLekarViewModel(
        BolnicaContext kontekst,
        int lekarId)
    {
        _kontekst = kontekst;
        _lekarId = lekarId;

        _ = UcitajPodatkeAsync();
    }


    private async Task UcitajPodatkeAsync()
    {
        await UcitajLekaraAsync();
        await UcitajStatistikeAsync();
        await UcitajPoslednjePregledeAsync();
        await UcitajPredstojeceKontroleAsync();
    }


    private async Task UcitajLekaraAsync()
    {
        Korisnik? lekar = await _kontekst.Korisnici
            .FirstOrDefaultAsync(k => k.KorisnikId == _lekarId);

        if (lekar != null)
        {
            PozdravnaPoruka =
                $"Dobro došli, {lekar.Ime} {lekar.Prezime}";
        }
    }


    private async Task UcitajStatistikeAsync()
    {
        BrojPacijenata = await _kontekst.Pacijenti
            .CountAsync(p => p.LekarId == _lekarId);


        BrojPregleda = await _kontekst.Pregledi
            .CountAsync(p => p.LekarId == _lekarId);


        DateTime danas = DateTime.Today;
        DateTime sutra = danas.AddDays(1);

        BrojPregledaDanas = await _kontekst.Pregledi
            .CountAsync(p =>
                p.LekarId == _lekarId &&
                p.DatumPregleda >= danas &&
                p.DatumPregleda < sutra);


        BrojPredstojecihKontrola = await _kontekst.Pregledi
            .CountAsync(p =>
                p.LekarId == _lekarId &&
                p.DatumKontrole != null &&
                p.DatumKontrole >= danas);
    }


    private async Task UcitajPoslednjePregledeAsync()
    {
        List<Pregled> pregledi = await _kontekst.Pregledi
            .Include(p => p.Pacijent)
            .Where(p => p.LekarId == _lekarId)
            .OrderByDescending(p => p.DatumPregleda)
            .Take(5)
            .ToListAsync();

        PoslednjiPregledi.Clear();

        foreach (Pregled pregled in pregledi)
        {
            PoslednjiPregledi.Add(pregled);
        }
    }


    private async Task UcitajPredstojeceKontroleAsync()
    {
        DateTime danas = DateTime.Today;

        List<Pregled> kontrole = await _kontekst.Pregledi
            .Include(p => p.Pacijent)
            .Where(p =>
                p.LekarId == _lekarId &&
                p.DatumKontrole != null &&
                p.DatumKontrole >= danas)
            .OrderBy(p => p.DatumKontrole)
            .Take(5)
            .ToListAsync();

        PredstojeceKontrole.Clear();

        foreach (Pregled pregled in kontrole)
        {
            PredstojeceKontrole.Add(pregled);
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? imeSvojstva = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(imeSvojstva));
    }
}