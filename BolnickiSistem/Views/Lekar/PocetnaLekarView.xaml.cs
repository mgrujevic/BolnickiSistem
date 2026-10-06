using BolnickiSistem.Data;
using BolnickiSistem.Services;
using BolnickiSistem.ViewModels;
using System.Windows.Controls;

namespace BolnickiSistem.Views.Lekar;

public partial class PocetnaLekarView : UserControl
{
    public PocetnaLekarView(BolnicaContext kontekst, TrenutniKorisnikServis trenutniKorisnik)
    {
        InitializeComponent();

        DataContext = new PocetnaLekarViewModel(
                    kontekst,
                    trenutniKorisnik.KorisnikId);

    }
}