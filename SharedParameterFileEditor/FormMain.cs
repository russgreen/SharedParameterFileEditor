using SharedParameterFileEditor.Requesters;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.ListView.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedParametersFile;
using SharedParametersFile.Models;

namespace SharedParameterFileEditor
{
    public partial class FormMain : Form, IParameterListRequester
    {
        private FileInfo _fileInfo;
        private SharedParametersDefinitionFile _defFile; 
        private bool _unsavedChanges = false;

        private SharedParametersDefinitionFile _mergeSourceFile;

        public FormMain()
        {
            InitializeComponent();

            HideGroupsPanel();

            var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            this.Text = $"Shared Parameter File Editor {informationVersion}";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Shared Parameter Files (*.txt)|*.txt";
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Open Shared Parameter Definition File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _fileInfo = new FileInfo(openFileDialog.FileName);
                    _defFile = new SharedParametersDefinitionFile(_fileInfo.FullName);
                    _defFile.LoadFile();

                    BuildParametersDataGrid();
                    BuildGroupsDataGrid();

                    this.saveToolStripMenuItem.Enabled = false;
                    this.saveAsToolStripMenuItem.Enabled = true;

                    this.mergeNewIntoCurrentToolStripMenuItem.Enabled = true;
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.Filter = "Shared Parameter Files (*.txt)|*.txt";
                saveFileDialog.Title = "Save Shared Parameter Definition File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _defFile.SaveFile(saveFileDialog.FileName);
                    _unsavedChanges = false;
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _defFile.SaveFile();
            RefreshDataSources();
            _unsavedChanges = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_unsavedChanges == true)
            {
                // Create the page which we want to show in the dialog.
                TaskDialogButton btnCancel = TaskDialogButton.Cancel;
                TaskDialogButton btnSave = new TaskDialogButton("&Save");
                TaskDialogButton btnDontSave = new TaskDialogButton("Do&n't save");

                var page = new TaskDialogPage()
                {
                    Caption = $"{this.Text} closing",
                    Heading = $"Do you want to save changes to {_fileInfo.Name}?",
                    Buttons = { btnCancel, btnSave, btnDontSave }
                };

                // Show a modal dialog, then check the result.
                TaskDialogButton result = TaskDialog.ShowDialog(this, page);

                if (result == btnSave)
                {
                    _defFile.SaveFile();
                    this.Close();
                }
                else if (result == btnDontSave)
                {
                    this.Close();
                }
                //else do nothing
            }
            else
            {
                this.Close();
            }
        }

        private void showGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGroupsPanel();
        }

        private void hideGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideGroupsPanel();
        }

        private void mergeNewIntoCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Shared Parameter Files (*.txt)|*.txt";
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "Open Shared Parameter Definition File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _mergeSourceFile = new SharedParametersDefinitionFile(openFileDialog.FileName);
                    _mergeSourceFile.LoadFile();

                    Form frm = new FormMergeParameters(this, _defFile.definitionFileModel, _mergeSourceFile.definitionFileModel);

                    frm.ShowDialog(this);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new AboutBox1();
            frm.ShowDialog();
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
                Width = 150
            };

            GridComboBoxColumn gridComboBoxGroup = new GridComboBoxColumn
            {
                MappingName = "Group",
                HeaderText = "Group",
                ValueMember = "ID",
                DisplayMember = "Name",
                DropDownStyle = DropDownStyle.DropDownList,
                DataSource = _defFile.definitionFileModel.Groups,
                Width = 200
            };

            //GUID  NAME    DATATYPE    DATACATEGORY    GROUP   VISIBLE DESCRIPTION USERMODIFIABLE  HIDEWHENNOVALUE
            this.sfDataGridParams.Columns.Clear();
            this.sfDataGridParams.Columns.Add(new GridTextColumn() { MappingName = "Guid", HeaderText = "GUID", MinimumWidth = 300, AllowEditing = false });
            this.sfDataGridParams.Columns.Add(new GridTextColumn() { MappingName = "Name", HeaderText = "Name", MinimumWidth = 300 });
            this.sfDataGridParams.Columns.Add(gridComboBoxDataType);
            this.sfDataGridParams.Columns.Add(new GridTextColumn() { MappingName = "DataCategory", HeaderText = "DataCategory", AllowEditing = false });
            this.sfDataGridParams.Columns.Add(gridComboBoxGroup);
            this.sfDataGridParams.Columns.Add(new GridCheckBoxColumn() { MappingName = "Visible", HeaderText = "Visible" });
            this.sfDataGridParams.Columns.Add(new GridTextColumn() { MappingName = "Description", HeaderText = "Tooltip Description", MinimumWidth = 200 });
            this.sfDataGridParams.Columns.Add(new GridCheckBoxColumn() { MappingName = "UserModifiable", HeaderText = "UserModifiable" });
            this.sfDataGridParams.Columns.Add(new GridCheckBoxColumn() { MappingName = "HideWhenNoValue", HeaderText = "HideWhenNoValue" });

            this.sfDataGridParams.DataSource = _defFile.definitionFileModel.Parameters;

        }

        private void BuildGroupsDataGrid()
        {
            this.sfDataGridGroups.Columns.Clear();
            this.sfDataGridGroups.Columns.Add(new GridTextColumn() { MappingName = "ID", HeaderText = "ID", MinimumWidth = 50, AllowEditing = false });
            this.sfDataGridGroups.Columns.Add(new GridTextColumn() { MappingName = "Name", HeaderText = "Name", MinimumWidth = 300 });

            this.sfDataGridGroups.DataSource = _defFile.definitionFileModel.Groups;
        }

        private void HideGroupsPanel()
        {
            this.splitContainer1.Panel1Collapsed = true;
            this.splitContainer1.Panel1.Hide();
        }

        private void ShowGroupsPanel()
        {
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer1.Panel1.Show();
        }

        private void sfDataGridParams_CurrentCellValidated(object sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellValidatedEventArgs e)
        {
            if(e.NewValue != e.OldValue)
            {
                this.saveToolStripMenuItem.Enabled = true;
                this.saveAsToolStripMenuItem.Enabled = true;

                _unsavedChanges = true;
            }
        }

        private void sfDataGridGroups_CurrentCellValidated(object sender, Syncfusion.WinForms.DataGrid.Events.CurrentCellValidatedEventArgs e)
        {
            if(e.NewValue != e.OldValue)
            {
                this.saveToolStripMenuItem.Enabled = true;
                this.saveAsToolStripMenuItem.Enabled = true;

                _unsavedChanges = true;
            }
        }

        private void sfDataGridGroups_AddNewRowInitiating(object sender, Syncfusion.WinForms.DataGrid.Events.AddNewRowInitiatingEventArgs e)
        {
            var ID = _defFile.definitionFileModel.Groups.Max(x => x.ID);
            ID++;

            var newGroup = e.NewObject as GroupModel;

            newGroup.ID = ID;
        }

        private void sfDataGridGroups_RecordDeleting(object sender, Syncfusion.WinForms.DataGrid.Events.RecordDeletingEventArgs e)
        {
            var group = e.Items.First() as GroupModel;

            var count = _defFile.definitionFileModel.Parameters.Count(x => x.Group == group.ID);

            if(count > 0)
            {
                e.Cancel = true;
            }
        }

        private void sfDataGridParams_AddNewRowInitiating(object sender, Syncfusion.WinForms.DataGrid.Events.AddNewRowInitiatingEventArgs e)
        {
            var param = e.NewObject as ParameterModel;

            param.Group = _defFile.definitionFileModel.Groups.Min(x => x.ID);
        }

        private void sfDataGridParams_RecordDeleting(object sender, Syncfusion.WinForms.DataGrid.Events.RecordDeletingEventArgs e)
        {

        }

        private void sfDataGridParams_RecordDeleted(object sender, Syncfusion.WinForms.DataGrid.Events.RecordDeletedEventArgs e)
        {
            this.saveToolStripMenuItem.Enabled = true;
        }

        private void sfDataGridGroups_RecordDeleted(object sender, Syncfusion.WinForms.DataGrid.Events.RecordDeletedEventArgs e)
        {
            this.saveToolStripMenuItem.Enabled = true;
        }

        void sfDataGridParams_DrawCell(object sender, Syncfusion.WinForms.DataGrid.Events.DrawCellEventArgs e)
        {
            if (e.DataRow.RowType == RowType.CaptionCoveredRow && !string.IsNullOrEmpty(e.DisplayText))
            {
                var displayText = string.Empty;
                var group = (e.DataRow.RowData as Syncfusion.Data.Group);
                if (group != null)
                {
                    displayText = $"{_defFile.definitionFileModel.Groups.Where(x => x.ID == int.Parse(group.Key.ToString())).FirstOrDefault().Name} : { group.Records.Count } parameters";
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

        public void ParameterListComplete(List<ParameterModel> parameters, List<GroupModel> groups)
        {
            var ID = _defFile.definitionFileModel.Groups.Max(x => x.ID);

            //TODO: map the parameter goup IDs

            //if (groups.Count > 0)
            //{
            //    foreach (var group in groups)
            //    {
            //        ID++;
            //        group.ID = ID;
            //        _defFile.definitionFileModel.Groups.Add(group);
            //    }
            //}


            if( parameters.Count > 0)
            {
                ID++;

                var newGroup = new GroupModel
                {
                    ID = ID,
                    Name = "Merged Parameters"
                };

                _defFile.definitionFileModel.Groups.Add(newGroup);

                //for each parameter.  
                foreach (var parameter in parameters)
                {
                    parameter.Group = newGroup.ID;
                    _defFile.definitionFileModel.Parameters.Add(parameter);
                }

                this.saveToolStripMenuItem.Enabled = true;
                this.saveAsToolStripMenuItem.Enabled = true;

                _unsavedChanges = true;

                RefreshDataSources();
            }

        }

        private void RefreshDataSources()
        {
            this.sfDataGridGroups.DataSource = null;
            this.sfDataGridParams.DataSource = null;
            this.sfDataGridGroups.DataSource = _defFile.definitionFileModel.Groups;
            this.sfDataGridParams.DataSource = _defFile.definitionFileModel.Parameters;
        }


    }
}
