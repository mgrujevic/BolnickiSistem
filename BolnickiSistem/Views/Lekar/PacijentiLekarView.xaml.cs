using BolnickiSistem.Data;
using BolnickiSistem.Models;
using BolnickiSistem.Services;
using BolnickiSistem.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace BolnickiSistem.Views.Lekar;

public partial class PacijentiLekarView : UserControl
{
    public event Action<Pacijent>? OtvoriKartonRequested;

    public PacijentiLekarView(BolnicaContext kontekst, TrenutniKorisnikServis trenutniKorisnik)
    {
        InitializeComponent();

        DataContext = new PacijentiLekarViewModel(
                            kontekst,
                            trenutniKorisnik.KorisnikId);
    }

    private void PacijentiDataGrid_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (DataContext is PacijentiLekarViewModel viewModel &&
            viewModel.IzabraniPacijent != null)
        {
            OtvoriKartonRequested?.Invoke(viewModel.IzabraniPacijent);
        }
    }
}