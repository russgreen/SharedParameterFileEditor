using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SharedParametersFile.Models;

public partial class SharedParameterDefinitionFileModel : ObservableObject //BaseModel
{

    //public string Filename { get; set; }

    //public int Version { get; set; }
    //public int MinVersion { get; set; }

    //public ObservableCollection<GroupModel> Groups { get; set; } = new ObservableCollection<GroupModel>();
    //public ObservableCollection<ParameterModel> Parameters { get; set; } = new ObservableCollection<ParameterModel>();

    [ObservableProperty]
    private string _filename;// { get; set; }
    [ObservableProperty]
    private int _version;// { get; set; }
    [ObservableProperty]
    private int _minVersion;// { get; set; }


    [ObservableProperty]
    private ObservableCollection<GroupModel> _groups = new(); //{ get; set; } = new ObservableCollection<GroupModel>();
    [ObservableProperty]
    private ObservableCollection<ParameterModel> _parameters = new(); //{ get; set; } = new ObservableCollection<ParameterModel>();
}
