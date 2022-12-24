using SharedParameterFileEditor.Views;
using System.Windows;

namespace SharedParameterFileEditor;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    void App_Startup(object sender, StartupEventArgs e)
    {
        //register the syncfusion license
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("##SyncfusionLicense##");

        MainView mainView = new();
        mainView.Show();
        return;
    }
}
