using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using BolnickiSistem.Models;
using BolnickiSistem.Services;
using BolnickiSistem.Views.Admin;



namespace BolnickiSistem.Views.Admin;

public partial class AdminDashboard : Window
{
    private readonly TrenutniKorisnikServis _trenutniKorisnik;

    public AdminDashboard(TrenutniKorisnikServis trenutniKorisnik)
    {
        InitializeComponent();

        _trenutniKorisnik = trenutniKorisnik;

        PrikaziPocetnu();
    }

    private void PrikaziPocetnu()
    {
        GlavniSadrzaj.Content = new PocetnaView();
    }

    private void KorisniciButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        var korisniciView = App.Host.Services.GetRequiredService<KorisniciView>();

        GlavniSadrzaj.Content = korisniciView;
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
        var pacijentiView = App.Host.Services.GetRequiredService<PacijentiView>();

        GlavniSadrzaj.Content = pacijentiView;
    }

    private void IzvestajiButton_Click(object sender, RoutedEventArgs e)
    {
        var izvestajiView = App.Host.Services.GetRequiredService<IzvestajiView>();

        GlavniSadrzaj.Content = izvestajiView;
    }

    private void OdjavaButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        _trenutniKorisnik.OdjaviKorisnika();

        LoginWindow loginProzor = App.Host.Services.GetRequiredService<LoginWindow>();

        loginProzor.Show();

        Close();
    }
}