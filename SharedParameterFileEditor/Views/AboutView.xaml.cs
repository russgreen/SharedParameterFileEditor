using System.Windows;

namespace SharedParameterFileEditor.Views;
/// <summary>
/// Interaction logic for AboutView.xaml
/// </summary>
public partial class AboutView : Window
{
    private readonly ViewModels.AboutViewModel _viewModel;

    public AboutView()
    {
        InitializeComponent();

        _viewModel = (ViewModels.AboutViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
