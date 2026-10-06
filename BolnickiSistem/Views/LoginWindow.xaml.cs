using BolnickiSistem.Models;
using BolnickiSistem.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using BolnickiSistem.Views.Admin;

namespace BolnickiSistem.Views;

public partial class LoginWindow : Window
{
    private readonly AutentifikacioniServis _autentifikacioniServis;

    public LoginWindow(AutentifikacioniServis autentifikacioniServis)
    {
        InitializeComponent();

        _autentifikacioniServis = autentifikacioniServis;

        KorisnickoImeTextBox.Focus();
    }

    private async void PrijavaButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        PorukaTextBlock.Text = string.Empty;

        string korisnickoIme = KorisnickoImeTextBox.Text.Trim();
        string lozinka = LozinkaPasswordBox.Password;

        if (string.IsNullOrWhiteSpace(korisnickoIme) ||
            string.IsNullOrWhiteSpace(lozinka))
        {
            PorukaTextBlock.Text = "Unesite korisničko ime i lozinku.";

            return;
        }

        try
        {
            Korisnik? korisnik = await _autentifikacioniServis.PrijaviKorisnikaAsync(korisnickoIme, lozinka);

            if (korisnik == null)
            {
                PorukaTextBlock.Text = "Pogrešno korisničko ime ili lozinka.";

                return;
            }

            var trenutniKorisnik = App.Host.Services.GetRequiredService<TrenutniKorisnikServis>();

            trenutniKorisnik.PostaviKorisnika(korisnik);

            if (korisnik.Uloga?.Naziv == "Administrator")
            {
                var adminDashboard = App.Host.Services.GetRequiredService<AdminDashboard>();

                adminDashboard.Show();
                Close();
            }
            else if (korisnik.Uloga?.Naziv == "Lekar")
            {
                var lekarDashboard = App.Host.Services.GetRequiredService<LekarDashboard>();

                lekarDashboard.Show();
                Close();
            }
            else
            {
                PorukaTextBlock.Text = "Korisnik nema definisanu ulogu.";
            }
        }
        catch (Exception ex)
        {
            PorukaTextBlock.Text = $"Došlo je do greške prilikom prijave. {ex.Message}";
        }
    }
}