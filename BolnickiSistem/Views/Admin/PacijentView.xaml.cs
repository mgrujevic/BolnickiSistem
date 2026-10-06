using System.Windows.Controls;
using BolnickiSistem.ViewModels;

namespace BolnickiSistem.Views.Admin;

public partial class PacijentiView : UserControl
{
    public PacijentiView(PacijentiViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}