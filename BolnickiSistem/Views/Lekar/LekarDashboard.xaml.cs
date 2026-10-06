using BolnickiSistem.Models;
using BolnickiSistem.Views.Lekar;
using Microsoft.Extensions.DependencyInjection;
using BolnickiSistem.Services;
using System.Windows;

namespace BolnickiSistem.Views;

public partial class LekarDashboard : Window
{

    private readonly TrenutniKorisnikServis _trenutniKorisnik;

    public LekarDashboard(TrenutniKorisnikServis trenutniKorisnik)
    {
        InitializeComponent();

        _trenutniKorisnik = trenutniKorisnik;

        PrikaziPocetnu();
    }


    private void PrikaziPocetnu()
    {
        GlavniSadrzaj.Content = App.Host.Services.GetRequiredService<PocetnaLekarView>();
    }


    private void PocetnaButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        PrikaziPocetnu();
    }


    private void PacijentiButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        var pacijentiView = App.Host.Services.GetRequiredService<PacijentiLekarView>();

        pacijentiView.OtvoriKartonRequested += OtvoriZdravstveniKarton;

        GlavniSadrzaj.Content = pacijentiView;
    }

    private void OtvoriZdravstveniKarton(Pacijent pacijent)
    {
        var kartonView = App.Host.Services.GetRequiredService<ZdravstveniKartonView>();

        kartonView.NazadRequested += VratiNaPacijente;

        kartonView.PostaviPacijenta(pacijent);

        GlavniSadrzaj.Content = kartonView;
    }
        

    private void VratiNaPacijente()
    {
        var pacijentiView = App.Host.Services.GetRequiredService<PacijentiLekarView>();

        pacijentiView.OtvoriKartonRequested += OtvoriZdravstveniKarton;

        GlavniSadrzaj.Content = pacijentiView;
    }


    private void OdjavaButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        _trenutniKorisnik.OdjaviKorisnika();

        LoginWindow loginProzor =
            App.Host.Services.GetRequiredService<LoginWindow>();

        loginProzor.Show();

        Close();
    }


}