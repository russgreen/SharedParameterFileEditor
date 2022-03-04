using System.ComponentModel;

namespace SharedParameterFileEditor.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
