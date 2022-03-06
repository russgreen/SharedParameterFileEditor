using SharedParametersFile.Extensions;

namespace SharedParametersFile;

public class SharedParametersDefinitionFile
{
    private string _definitionFileName { get; set; }

    public Models.SharedParameterDefinitionFileModel definitionFileModel { get; set; }

    public SharedParametersDefinitionFile(string fileName)
    {
        _definitionFileName = fileName;

        definitionFileModel = new Models.SharedParameterDefinitionFileModel();
        definitionFileModel.Filename = _definitionFileName;
    }

    public void LoadFile()
    {
        List<string> lines = System.IO.File.ReadAllLines(_definitionFileName).ToList();

        foreach (var line in lines)
        {
            var lineArray = line.Split('\t').ToList();

            //get the meta data
            if (lineArray[0].Equals("META"))
            {
                definitionFileModel.Version = int.Parse(lineArray[1]);
                definitionFileModel.MinVersion = int.Parse(lineArray[2]);
            }

            //get the group data
            if (lineArray[0].Equals("GROUP"))
            {
                definitionFileModel.Groups.Add(new Models.GroupModel
                {
                    ID = int.Parse(lineArray[1]),
                    Name = lineArray[2]
                });
            }

            //get the parameter data
            if (lineArray[0].Equals("PARAM"))
            {
                definitionFileModel.Parameters.Add(new Models.ParameterModel
                {
                    Guid = Guid.Parse(lineArray[1]),
                    Name = lineArray[2],
                    DataType = lineArray[3],
                    DataCategory = lineArray[4].ToNullableInt(),
                    Group = int.Parse(lineArray[5]),
                    Visible = Convert.ToBoolean(int.Parse(lineArray[6])),
                    Description = (lineArray.Count == 8) ? lineArray[7] : string.Empty,
                    UserModifiable = (lineArray.Count == 9) ? Convert.ToBoolean(int.Parse(lineArray[8])) : true,
                    HideWhenNoValue = (lineArray.Count == 10) ? Convert.ToBoolean(int.Parse(lineArray[9])) : false //check if the last column exists in the file
                });
            }
        }
    }

    public void SaveFile(string fileName = null)
    {
        List<string> output = new List<string>();

        output.Add("# This is a Revit shared parameter file.");
        output.Add("# Do not edit manually.");
        output.Add("*META\tVERSION\tMINVERSION");
        output.Add($"META\t{definitionFileModel.Version}\t{definitionFileModel.MinVersion}");
        output.Add("*GROUP\tID\tNAME");

        foreach (var item in definitionFileModel.Groups)
        {
            output.Add($"GROUP\t{item.ID}\t{item.Name}");
        }

        output.Add("*PARAM\tGUID\tNAME\tDATATYPE\tDATACATEGORY\tGROUP\tVISIBLE\tDESCRIPTION\tUSERMODIFIABLE\tHIDEWHENNOVALUE");

        foreach (var item in definitionFileModel.Parameters)
        {
            output.Add($"PARAM\t{item.Guid.ToString()}\t{item.Name}\t{item.DataType}\t{item.DataCategory}\t{item.Group}\t{Convert.ToInt32(item.Visible)}\t{item.Description}\t{Convert.ToInt32(item.UserModifiable)}\t{Convert.ToInt32(item.HideWhenNoValue)}");
        }

        if (fileName == null)
        {
            fileName = _definitionFileName;

            backUpFile(fileName);
        }
        else
        {
            _definitionFileName = fileName;
        }

        System.IO.File.WriteAllLines(fileName, output);
    }

    private void backUpFile(string fileName)
    {
        //backup filename to be in the format filename.nnnn.txt

        System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);

        //first look for existing backups - regex "<filename>.\d\d\d\d.txt"
        var searchPattern = $"{fileInfo.Name.Remove(fileInfo.Name.Length - 4)}.????.txt";
        var files = System.IO.Directory.GetFiles(fileInfo.DirectoryName, searchPattern);

        var backupNumber = 1;


        if (files.Length > 0)
        {
            List<int> backNumbers = new List<int>();

            foreach (var file in files)
            {
                var data = System.Text.RegularExpressions.Regex.Match(file, @"\d\d\d\d.txt").Value;
                var number = int.Parse(data.Remove(data.Length - 4));
                backNumbers.Add(number);
            }

            backupNumber = backNumbers.Max();
            backupNumber++;
        }

        var backupFilename = System.IO.Path.Combine(fileInfo.DirectoryName, $"{fileInfo.Name.Remove(fileInfo.Name.Length - 4)}.{backupNumber.ToString("D4")}.txt");

        fileInfo.CopyTo(backupFilename);
    }

}
