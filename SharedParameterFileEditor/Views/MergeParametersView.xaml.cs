using SharedParameterFileEditor.ViewModels;
using SharedParametersFile.Models;
using System.Windows;

namespace SharedParameterFileEditor.Views;
/// <summary>
/// Interaction logic for MergeParametersWindow.xaml
/// </summary>
public partial class MergeParametersView : Window
{

    private readonly ViewModels.MergeParametersViewModel _viewModel;

    //public MergeParametersView()
    //{
    //    InitializeComponent();
    //}

    public MergeParametersView(SharedParameterDefinitionFileModel targetModel, SharedParameterDefinitionFileModel sourceModel)
    {
        InitializeComponent();

        _viewModel = new MergeParametersViewModel(targetModel, sourceModel);
        this.DataContext = _viewModel;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }

    private void SfDataGrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
    {
        // couldn't get the databinding to work on the selecteditems property.
        // TODO investigate why not working
        var items = this.sfDataGrid.SelectedItems;
        _viewModel.SelectedParameterModels.Clear();

        if (items.Count > 0)
        {
            //viewModel.SheetsSelected = true;
            foreach (var item in items)
            {
                _viewModel.SelectedParameterModels.Add((ParameterModel)item);
            }
        }
    }
}
