using BolnickiSistem.Data;
using BolnickiSistem.Services;
using BolnickiSistem.ViewModels;
using BolnickiSistem.Views;
using BolnickiSistem.Views.Admin;
using BolnickiSistem.Views.Lekar;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace BolnickiSistem;

public partial class App : Application
{
    public static IHost Host { get; private set; } = null!;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        Host = Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                string? vezaSaBazom =
                    context.Configuration.GetConnectionString("BolnicaBaza");

                services.AddDbContext<BolnicaContext>(opcije =>
                    opcije.UseSqlServer(vezaSaBazom));

                services.AddTransient<InicijalizacijaBazeServis>();

                services.AddTransient<AutentifikacioniServis>();
                services.AddTransient<LoginWindow>();

                services.AddTransient<AdminDashboard>();

                services.AddTransient<KorisniciView>();
                services.AddTransient<KorisniciViewModel>();

                services.AddTransient<PacijentiView>();
                services.AddTransient<PacijentiViewModel>();

                services.AddTransient<IzvestajiView>();
                services.AddTransient<IzvestajiViewModel>();

                services.AddSingleton<TrenutniKorisnikServis>();
                services.AddTransient<LekarDashboard>();
               
                services.AddTransient<PocetnaLekarView>();
                services.AddTransient<PacijentiLekarView>();
                services.AddTransient<ZdravstveniKartonView>();
                services.AddTransient<NoviPregledView>();

            })
            .Build();

        await Host.StartAsync();

        var inicijalizacijaBaze = Host.Services.GetRequiredService<InicijalizacijaBazeServis>();
        await inicijalizacijaBaze.InicijalizujAsync();

        var loginProzor = Host.Services.GetRequiredService<LoginWindow>();
        loginProzor.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (Host != null)
        {
            await Host.StopAsync();
            Host.Dispose();
        }

        base.OnExit(e);
    }
}