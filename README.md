![GitHub issues](https://img.shields.io/github/issues/russgreen/SharedParameterFileEditor)
![GitHub last commit](https://img.shields.io/github/last-commit/russgreen/SharedParameterFileEditor)
<img src="https://img.shields.io/badge/.net-6.0-blue">

# SharedParameterFileEditor
Standalone editor for Revit shared parameter files
![image](https://user-images.githubusercontent.com/1886088/156920547-0b7fb0a7-09ba-40da-9388-7f5c5ffd8810.png)

Parameters from a another (source) shared parameter file can be loaded and selectively merged or added to the current (target) file when GUID's in the source do not exist in the target.

## Roadmap

- [ ] Limit number of backuop files saved...delete older files on save.
- [ ] Revit addin to allow batch importing on shared parameters

## Syncfusion Controls
The UI makes use of Syncfusion controls community license. To use the app from this code a license must be passed into the pre and post build script.  The Syncfusion license key is stored in a text file at the root of the solution and is not committed to the Git repo.
