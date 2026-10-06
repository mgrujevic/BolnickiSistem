using BolnickiSistem.Data;
using BolnickiSistem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BolnickiSistem.ViewModels;

public class PacijentiLekarViewModel : INotifyPropertyChanged
{
    private readonly BolnicaContext _kontekst;
    private readonly int _lekarId;

    private Pacijent? _izabraniPacijent;
    private string _tekstPretrage = string.Empty;

    public ObservableCollection<Pacijent> Pacijenti { get; }
        = new();

    public ObservableCollection<Pacijent> SviPacijenti { get; }
        = new();

    public Pacijent? IzabraniPacijent
    {
        get => _izabraniPacijent;
        set
        {
            _izabraniPacijent = value;
            OnPropertyChanged();
        }
    }

    public string TekstPretrage
    {
        get => _tekstPretrage;
        set
        {
            _tekstPretrage = value;
            OnPropertyChanged();
            PretraziPacijente();
        }
    }

    public ICommand OsveziCommand { get; }

    public PacijentiLekarViewModel(
        BolnicaContext kontekst,
        int lekarId)
    {
        _kontekst = kontekst;
        _lekarId = lekarId;

        OsveziCommand = new RelayCommand(
            async _ => await UcitajPacijenteAsync());

        _ = UcitajPacijenteAsync();
    }

    private async Task UcitajPacijenteAsync()
    {
        List<Pacijent> pacijenti =
            await _kontekst.Pacijenti
                .Where(p => p.LekarId == _lekarId)
                .OrderBy(p => p.Prezime)
                .ThenBy(p => p.Ime)
                .ToListAsync();

        SviPacijenti.Clear();
        Pacijenti.Clear();

        foreach (Pacijent pacijent in pacijenti)
        {
            SviPacijenti.Add(pacijent);
            Pacijenti.Add(pacijent);
        }
    }

    private void PretraziPacijente()
    {
        string tekst = TekstPretrage.Trim();

        Pacijenti.Clear();

        IEnumerable<Pacijent> rezultat;

        if (string.IsNullOrWhiteSpace(tekst))
        {
            rezultat = SviPacijenti;
        }
        else
        {
            rezultat = SviPacijenti.Where(p =>
                p.BrojZdravstveneKartice.Contains(
                    tekst,
                    StringComparison.OrdinalIgnoreCase)

                 || p.JMBG.Contains(
                    tekst,
                    StringComparison.OrdinalIgnoreCase)


                || p.Ime.Contains(
                    tekst,
                    StringComparison.OrdinalIgnoreCase)

                || p.Prezime.Contains(
                    tekst,
                    StringComparison.OrdinalIgnoreCase));

        }

        foreach (Pacijent pacijent in rezultat)
        {
            Pacijenti.Add(pacijent);
        }
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