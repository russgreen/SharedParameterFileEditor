using SharedParametersFile.Models;
using SharedParameterFileEditor.Requesters;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.ListView.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SharedParameterFileEditor;

public partial class FormMergeParameters : Form
{
    private IParameterListRequester _callingForm;

    private List<ParameterModel> _parameterModels = new List<ParameterModel>();
    private List<GroupModel> _groupModels = new List<GroupModel>();

    private List<ParameterModel> _selectedParameterModels = new List<ParameterModel>();

    private SharedParameterDefinitionFileModel _targetModel;
    private SharedParameterDefinitionFileModel _sourceModel;

    public FormMergeParameters()
    {
        InitializeComponent();
    }

    public FormMergeParameters(IParameterListRequester caller, SharedParameterDefinitionFileModel targetModel, SharedParameterDefinitionFileModel sourceModel)
    {
        InitializeComponent();

        _callingForm = caller;

        _targetModel = targetModel;
        _sourceModel = sourceModel;

        BuildParametersDataGrid();
    }

    private void BuildParametersDataGrid()
    {
        var types = Enum.GetValues(typeof(ParameterType)).Cast<ParameterType>().ToList();

        GridComboBoxColumn gridComboBoxDataType = new GridComboBoxColumn
        {
            MappingName = "DataType",
            HeaderText = "DataType",
            DropDownStyle = DropDownStyle.DropDownList,
            DataSource = types,
            Width = 150,
            AllowEditing = false
        };

        GridComboBoxColumn gridComboBoxGroupSource = new GridComboBoxColumn
        {
            MappingName = "Group",
            HeaderText = "Group",
            ValueMember = "ID",
            DisplayMember = "Name",
            DropDownStyle = DropDownStyle.DropDownList,
            DataSource = _sourceModel.Groups,
            Width = 200,
            AllowEditing = false
        };

        //TODO:  map the parameters at source
        GridComboBoxColumn gridComboBoxGroupTarget = new GridComboBoxColumn
        {
            MappingName = "Group",
            HeaderText = "Target Group",
            ValueMember = "ID",
            DisplayMember = "Name",
            DropDownStyle = DropDownStyle.DropDownList,
            DataSource = _targetModel.Groups,
            Width = 200
        };

        //GUID  NAME    DATATYPE    DATACATEGORY    GROUP   VISIBLE DESCRIPTION USERMODIFIABLE  HIDEWHENNOVALUE
        this.sfDataGrid1.Columns.Clear();
        this.sfDataGrid1.Columns.Add(new GridCheckBoxSelectorColumn() { MappingName = "SelectorColumn", HeaderText = string.Empty, AllowCheckBoxOnHeader = false, Width = 34, CheckBoxSize = new Size(14, 14) });
        this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "Guid", HeaderText = "GUID", MinimumWidth = 300, AllowEditing = false });
        this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "Name", HeaderText = "Name", MinimumWidth = 300, AllowEditing = false });
        this.sfDataGrid1.Columns.Add(gridComboBoxDataType);
        this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "DataCategory", HeaderText = "DataCategory", AllowEditing = false });
        this.sfDataGrid1.Columns.Add(gridComboBoxGroupSource);
        //this.sfDataGrid1.Columns.Add(gridComboBoxGroupTarget);
        this.sfDataGrid1.Columns.Add(new GridCheckBoxColumn() { MappingName = "Visible", HeaderText = "Visible", AllowEditing = false });
        this.sfDataGrid1.Columns.Add(new GridTextColumn() { MappingName = "Description", HeaderText = "Tooltip Description", MinimumWidth = 200 });
        this.sfDataGrid1.Columns.Add(new GridCheckBoxColumn() { MappingName = "UserModifiable", HeaderText = "UserModifiable", AllowEditing = false });
        this.sfDataGrid1.Columns.Add(new GridCheckBoxColumn() { MappingName = "HideWhenNoValue", HeaderText = "HideWhenNoValue", AllowEditing = false });

        this.sfDataGrid1.DataSource = _sourceModel.Parameters
            .Where(p => _targetModel.Parameters
            .All(p2 => p2.Guid != p.Guid)).ToList();

    }

    void sfDataGrid1_DrawCell(object sender, Syncfusion.WinForms.DataGrid.Events.DrawCellEventArgs e)
    {
        if (e.DataRow.RowType == RowType.CaptionCoveredRow && !string.IsNullOrEmpty(e.DisplayText))
        {
            var displayText = string.Empty;
            var group = (e.DataRow.RowData as Syncfusion.Data.Group);
            if (group != null)
            {
                displayText = $"{_sourceModel.Groups.Where(x => x.ID == int.Parse(group.Key.ToString())).FirstOrDefault().Name} : { group.Records.Count } parameters";
                e.DisplayText = displayText;
            }
        }
    }

    private int GetTotalRecordsCount(Group group)
    {
        int count = 0;

        if (group.Groups != null)
        {
            foreach (var g in group.Groups)
            {
                if (g.Groups != null)
                    foreach (var g1 in g.Groups)
                        count += GetTotalRecordsCount(g1);

                if (g.Records != null)
                    count += g.Records.Count;
            }
        }
        else
            if (group.Records != null)
            count += group.Records.Count;

        return count;
    }

    private void buttonMergeParameters_Click(object sender, EventArgs e)
    {
        _parameterModels = this.sfDataGrid1.SelectedItems.Cast<ParameterModel>().ToList();

        //_groupModels = _sourceModel.Groups
        //    .Where(p => _targetModel.Groups
        //    .All(p2 => p2.Name != p.Name))
        //    .ToList();

        _groupModels = _sourceModel.Groups
            .Except(_targetModel.Groups)
            .ToList();

        _callingForm.ParameterListComplete(_parameterModels, _groupModels);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void sfDataGrid1_SelectionChanged(object sender, Syncfusion.WinForms.DataGrid.Events.SelectionChangedEventArgs e)
    {

        if (this.sfDataGrid1.SelectedItems.Count > 0)
        {
            this.buttonMergeParameters.Enabled = true;
        }
        else
        {
            this.buttonMergeParameters.Enabled = false;
        }

    }

}
