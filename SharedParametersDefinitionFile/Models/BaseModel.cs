using System.ComponentModel;

namespace SharedParametersFile.Models;

public class BaseModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

}
