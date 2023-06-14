using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SharedParameterFileEditor.Messages;
using SharedParameterFileEditor.Models;
using SharedParametersFile;
using SharedParametersFile.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace SharedParameterFileEditor.ViewModels;
internal partial class MainViewModel : BaseViewModel
{
	public string WindowTitle { get; private set; }

    [ObservableProperty]
    private System.Windows.Visibility _groupsVisible = System.Windows.Visibility.Visible;

    [ObservableProperty]
    private string _toggleGroupMenuText = "Hide groups";

    [ObservableProperty]
    private FileInfo _fileInfo;

    [ObservableProperty]
    private SharedParametersDefinitionFile _defFile;

    [ObservableProperty]
    private string _newFileName;

    [ObservableProperty]
    private bool _unsavedChanges = false;

    [ObservableProperty]
    private bool _mergeEnabled = false;

    [ObservableProperty]
    private bool _editGuid = false;

    private SharedParametersDefinitionFile _mergeSourceFile;

    [ObservableProperty]
    private List<ParameterType> _types = Enum.GetValues(typeof(ParameterType)).Cast<ParameterType>().ToList();


    public MainViewModel()
	{
        var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        WindowTitle = $"Shared Parameter View Editor {informationVersion}";

        //WeakReferenceMessenger.Default.Register<ListOfParametersMessage>(this, (r, m) =>
        //{
        //    MergeDefinitionFile((List<ParameterModel>)m.Value);
        //});

        WeakReferenceMessenger.Default.Register<ListsOfGroupsAndParametersMessage>(this, (r, m) =>
        {
            MergeDefinitionFile((GroupsAndParametersModel)m.Value);
        });
    }

    [RelayCommand]
    private void ToggleGroupVisibility()
    {
        if(GroupsVisible == System.Windows.Visibility.Visible)
        {
            GroupsVisible = System.Windows.Visibility.Hidden;
            ToggleGroupMenuText = "Show groups";
            return;
        }

        if(GroupsVisible == System.Windows.Visibility.Hidden)
        {
            GroupsVisible = System.Windows.Visibility.Visible;
            ToggleGroupMenuText = "Hide groups";
            return;
        }
    }

    [RelayCommand]
    public void LoadDefinitionFile()
    {
        DefFile = new SharedParametersDefinitionFile(FileInfo.FullName);
        DefFile.LoadFile();

        DefFile.definitionFileModel.Parameters.CollectionChanged += Parameters_CollectionChanged;
        DefFile.definitionFileModel.Groups.CollectionChanged += Groups_CollectionChanged;

        MergeEnabled = true;
    }


    [RelayCommand]
    public void SaveDefinitionFile()
    {
        if(NewFileName != null)
        {
            DefFile?.SaveFile(NewFileName);
            return;
        }

        DefFile?.SaveFile();
        UnsavedChanges = false;
    }

    [RelayCommand]
    private void MergeDefinitionFile(GroupsAndParametersModel itemsToMerge)
    {
        if (itemsToMerge.ParameterModels.Count > 0)
        {
            var newGroup = new GroupModel
            {
                Name = "Merged Parameters"
            };

            DefFile.definitionFileModel.Groups.Add(newGroup);

            //for each parameter.  
            foreach (var parameter in itemsToMerge.ParameterModels)
            {
                parameter.Group = newGroup.ID;
                DefFile.definitionFileModel.Parameters.Add(parameter);
            }

            UnsavedChanges = true;
        }
    }

    private void Groups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var ID = DefFile.definitionFileModel.Groups.Max(x => x.ID);
            ID++;

            var newGroup = e.NewItems[0] as GroupModel;
            newGroup.ID = ID;
        }

        UnsavedChanges = true;
    }

    private void Parameters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            var param = e.NewItems[0] as ParameterModel;

            if(param.Group <= 1)
            {
                param.Group = DefFile.definitionFileModel.Groups.Min(x => x.ID);
            }

        }

        UnsavedChanges = true;
    }
}