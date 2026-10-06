using System.Windows.Controls;
using BolnickiSistem.ViewModels;

namespace BolnickiSistem.Views.Admin;

public partial class IzvestajiView : UserControl
{
    public IzvestajiView(IzvestajiViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}