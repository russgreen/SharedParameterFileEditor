using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedParameterFileEditor.Models
{
    public class SharedParameterDefinitionFileModel : BaseModel
    {
        public string Filename { get; set; }

        public int Version { get; set; }
        public int MinVersion { get; set; }

        public ObservableCollection<GroupModel> Groups { get; set; } = new ObservableCollection<GroupModel>();
        public ObservableCollection<ParameterModel> Parameters { get; set; } = new ObservableCollection<ParameterModel>();
    }
}
