using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedParameterFileEditor.ViewModels;
internal partial class AboutViewModel : BaseViewModel
{
    public string AppTitle { get; private set; }

    public string AppVersion { get; private set; }

    public string AppCopyright { get; private set; }

    public string AppDescription { get; private set; }

    public AboutViewModel()
    {
        var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

        AppTitle = $"Shared Parameter View Editor";
        AppVersion = $"Version {informationVersion}";
        AppCopyright = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        AppDescription = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyDescriptionAttribute>().Description;    
    }

    [RelayCommand]
    private void OKButton()
    {
        this.OnClosingRequest();
    }
}
