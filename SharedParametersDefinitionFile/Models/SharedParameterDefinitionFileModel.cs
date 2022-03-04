using System.Collections.ObjectModel;

namespace SharedParametersFile.Models;

public class SharedParameterDefinitionFileModel : BaseModel
{
    public string Filename { get; set; }

    public int Version { get; set; }
    public int MinVersion { get; set; }

    public ObservableCollection<GroupModel> Groups { get; set; } = new ObservableCollection<GroupModel>();
    public ObservableCollection<ParameterModel> Parameters { get; set; } = new ObservableCollection<ParameterModel>();
}
