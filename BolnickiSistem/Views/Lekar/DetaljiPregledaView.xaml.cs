using System.Windows;
using System.Windows.Controls;

namespace BolnickiSistem.Views.Lekar;

public partial class DetaljiPregledaView : UserControl
{
    public DetaljiPregledaView()
    {
        InitializeComponent();
    }

    private void DodajDijagnozuButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NovaDijagnozaPanel.Visibility = NovaDijagnozaPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }
    private void OtkaziDijagnozuButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NovaDijagnozaPanel.Visibility = Visibility.Collapsed;
    }

    private void DodajTerapijuButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NovaTerapijaPanel.Visibility = NovaTerapijaPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    private void OtkaziTerapijuButton_Click(
    object sender,
    RoutedEventArgs e)
    {
        NovaTerapijaPanel.Visibility = Visibility.Collapsed;
    }



}