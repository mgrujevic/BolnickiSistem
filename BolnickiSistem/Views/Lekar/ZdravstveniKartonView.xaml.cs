using BolnickiSistem.Data;
using BolnickiSistem.Models;
using BolnickiSistem.Services;
using BolnickiSistem.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;

namespace BolnickiSistem.Views.Lekar;

public partial class ZdravstveniKartonView : UserControl
{
    private readonly ZdravstveniKartonViewModel _viewModel;

    public event Action? NazadRequested;

    public ZdravstveniKartonView(
        BolnicaContext kontekst,
        TrenutniKorisnikServis trenutniKorisnik)
    {
        InitializeComponent();

        _viewModel =
            new ZdravstveniKartonViewModel(
                kontekst,
                trenutniKorisnik.KorisnikId);

        DataContext = _viewModel;

        PrikaziPocetniSadrzaj();
    }


    public async void PostaviPacijenta(Pacijent pacijent)
    {
        await _viewModel.UcitajPacijentaAsync(
            pacijent.PacijentId);

        PrikaziPocetniSadrzaj();
    }


    private void PrikaziPocetniSadrzaj()
    {
        SadrzajKartona.Content =
            new TextBlock
            {
                Text = "Izaberite pregled iz istorije ili kliknite „Novi pregled“.",
                Foreground =
                    new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(
                            120, 144, 156)),
                FontSize = 17,
                HorizontalAlignment =
                    HorizontalAlignment.Center,
                VerticalAlignment =
                    VerticalAlignment.Center
            };
    }

    private void PreglediDataGrid_SelectionChanged(
    object sender,
    SelectionChangedEventArgs e)
    {
        if (sender is not DataGrid dataGrid)
            return;

        if (dataGrid.SelectedItem is not Pregled pregled)
            return;

        _viewModel.IzabraniPregled = pregled;

        SadrzajKartona.Content =
            new DetaljiPregledaView
            {
                DataContext = _viewModel
            };
    }


    private void NoviPregledButton_Click(
     object sender,
     RoutedEventArgs e)
    {
        _viewModel.IzabraniPregled = null;

        if (PreglediDataGrid != null)
        {
            PreglediDataGrid.UnselectAll();
        }

        SadrzajKartona.Content =
            new NoviPregledView
            {
                DataContext = _viewModel
            };
    }

    private void DodajLaboratorijskiNalazButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NoviLaboratorijskiNalazPanel.Visibility =
            NoviLaboratorijskiNalazPanel.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;

        if (NoviLaboratorijskiNalazPanel.Visibility == Visibility.Visible &&
            DataContext is ZdravstveniKartonViewModel viewModel)
        {
            viewModel.IzabraniPregledZaLaboratorijskiNalaz =
                viewModel.IzabraniPregled;
        }
    }

    private void OtkaziLaboratorijskiNalazButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        NoviLaboratorijskiNalazPanel.Visibility =
            Visibility.Collapsed;
    }


    private void DodajDokumentButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NoviDokumentPanel.Visibility =
            Visibility.Visible;
    }


    private void IzaberiDokumentButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        OpenFileDialog dijalog =
            new OpenFileDialog
            {
                Title = "Izaberite dokument",
                Filter =
                    "Svi fajlovi|*.*|" +
                    "PDF dokumenti|*.pdf|" +
                    "Word dokumenti|*.doc;*.docx|" +
                    "Slike|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };

        bool? rezultat =
            dijalog.ShowDialog();

        if (rezultat == true &&
            DataContext is ZdravstveniKartonViewModel viewModel)
        {
            viewModel.NoviDokumentPutanja =
                dijalog.FileName;

            if (string.IsNullOrWhiteSpace(
                    viewModel.NoviDokumentNaziv))
            {
                viewModel.NoviDokumentNaziv =
                    System.IO.Path.GetFileNameWithoutExtension(
                        dijalog.FileName);
            }
        }
    }


    private void OtvoriDokumentButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        if (sender is not Button dugme)
            return;

        if (dugme.Tag is not Dokument dokument)
            return;

        string punaPutanja =
            System.IO.Path.Combine(
                AppContext.BaseDirectory,
                dokument.PutanjaDoFajla);

        if (!System.IO.File.Exists(punaPutanja))
        {
            MessageBox.Show(
                $"Dokument nije pronađen:\n\n{punaPutanja}",
                "Dokument",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            return;
        }

        Process.Start(
            new ProcessStartInfo
            {
                FileName = punaPutanja,
                UseShellExecute = true
            });
    }


    private void OtkaziDokumentButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NoviDokumentPanel.Visibility =
            Visibility.Collapsed;

        if (DataContext is ZdravstveniKartonViewModel viewModel)
        {
            viewModel.NoviDokumentNaziv =
                string.Empty;

            viewModel.NoviDokumentTip =
                string.Empty;

            viewModel.NoviDokumentPutanja =
                string.Empty;

            viewModel.IzabraniPregledZaDokument =
                null;
        }
    }



    private void NazadButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        NazadRequested?.Invoke();
    }
}