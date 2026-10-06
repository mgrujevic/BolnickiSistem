using System.Windows.Controls;
using BolnickiSistem.ViewModels;

namespace BolnickiSistem.Views.Admin;

public partial class KorisniciView : UserControl
{
    public KorisniciView(KorisniciViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}