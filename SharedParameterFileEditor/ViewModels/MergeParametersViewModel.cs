using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SharedParameterFileEditor.Messages;
using SharedParameterFileEditor.Models;
using SharedParametersFile.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;

namespace SharedParameterFileEditor.ViewModels;
internal partial class MergeParametersViewModel : BaseViewModel
{
    [ObservableProperty]
    private List<ParameterModel> _parameterModels = new List<ParameterModel>();
    [ObservableProperty]
    private List<GroupModel> _groupModels = new List<GroupModel>();

    [ObservableProperty]
    private ObservableCollection<ParameterModel> _selectedParameterModels = new ();

    [ObservableProperty]
    private SharedParameterDefinitionFileModel _targetModel;
    [ObservableProperty]
    private SharedParameterDefinitionFileModel _sourceModel;

    [ObservableProperty]
    private bool _mergeEnabled = false;

    [ObservableProperty]
    private List<ParameterType> _types = Enum.GetValues(typeof(ParameterType)).Cast<ParameterType>().ToList();

    public MergeParametersViewModel(SharedParameterDefinitionFileModel targetModel, SharedParameterDefinitionFileModel sourceModel)
    {
        TargetModel = targetModel;
        SourceModel = sourceModel;

        ParameterModels = SourceModel.Parameters
            .Where(p => TargetModel.Parameters
            .All(p2 => p2.Guid != p.Guid)).ToList();

        SelectedParameterModels.CollectionChanged += SelectedParameters_CollectionChanged;
    }


    [RelayCommand]
    private void MergeParameters()
    {
        if(SelectedParameterModels.Count > 0)
        {
            var listsToSend = new GroupsAndParametersModel();
            listsToSend.ParameterModels = SelectedParameterModels.ToList();

            listsToSend.GroupModels = SourceModel.Groups
                .Where(g => SelectedParameterModels.Any(i => i.Group == g.ID))
                .ToList();

            //WeakReferenceMessenger.Default.Send(new ListOfParametersMessage(_selectedParameterModels.ToList()));
            WeakReferenceMessenger.Default.Send(new ListsOfGroupsAndParametersMessage(listsToSend));
        }

        this.OnClosingRequest();
    }

    private void SelectedParameters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (SelectedParameterModels.Count > 0)
        {
            MergeEnabled = true;
            return;
        }

        MergeEnabled = false;
    }

    partial void OnSelectedParameterModelsChanging(ObservableCollection<ParameterModel> value)
    {

    }
}
