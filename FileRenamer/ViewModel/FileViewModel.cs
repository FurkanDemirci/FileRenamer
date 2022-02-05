using FileRenamer.Model;
using FileRenamer.ViewModel.Binding;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace FileRenamer.ViewModel
{
    public class FileViewModel : ViewModelBase
    {
        private OpenFileDialog _openFileDialog;

        private readonly DelegateCommand _uploadFiles;
        private readonly DelegateCommand _renameFiles;

        private List<File> _fileNames;
        private bool _isSelected;
        private string _originalFileName;
        private string _changeToName;
        private int _startWith;

        public List<File> FileNames
        {
            get { return _fileNames; }
            set { SetProperty(ref _fileNames, value); }
        }

        public string OriginalFileName
        {
            get { return _originalFileName; }
            set { SetProperty(ref _originalFileName, value); }
        }

        public string ChangeToName
        {
            get { return _changeToName; }
            set
            {
                SetProperty(ref _changeToName, value);
                _renameFiles.InvokeCanExecuteChanged();
            }
        }

        public int StartWith
        {
            get { return _startWith; }
            set { SetProperty(ref _startWith, value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        public ICommand UploadFiles => _uploadFiles;
        public ICommand RenameFiles => _renameFiles;

        public FileViewModel()
        {
            // In progress
            _uploadFiles = new DelegateCommand(OnUploadFiles, CanUploadFiles);
            _renameFiles = new DelegateCommand(OnRenameFiles, CanRenameFiles);

            _originalFileName = "...";
            _changeToName = "";
        }

        public void OnUploadFiles(object commandParameter)
        {
            _openFileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            _openFileDialog.ShowDialog();
            FileNames = new List<File>();

            for (int i = 0; i < _openFileDialog.FileNames.Length; i++)
            {
                FileNames.Add(
                    new File
                    {
                        Name = _openFileDialog.SafeFileNames[i],
                        Location = _openFileDialog.FileNames[i]
                    });
            }

            OriginalFileName = _openFileDialog.SafeFileName;
            _renameFiles.InvokeCanExecuteChanged();
        }

        public void OnRenameFiles(object commandParameter)
        {
            try
            {
                int number = StartWith;
                string location = FileNames[0].Location;
                int indexLocation = location.LastIndexOf(@"\");
                if (indexLocation >= 0)
                    location = location.Substring(0, indexLocation + 1);

                string extensionType = FileNames[0].Location;
                int indexExtension = extensionType.LastIndexOf(@".");
                if (indexExtension >= 0)
                    extensionType = extensionType.Substring(indexExtension);

                foreach (var file in FileNames)
                {
                    string newName = ChangeToName;
                    newName = newName.Replace("*", number.ToString());
                    number++;

                    newName = location + newName + extensionType;
                    FileSystem.Rename(file.Location, newName);
                }
                MessageBox.Show("Successfully changed names of all files!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception e)
            {
                MessageBox.Show("Something went wrong! \nFollowing error message was generated:\n\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            OriginalFileName = "...";
            ChangeToName = "";
            StartWith = 0;
            FileNames = new List<File>();
        }

        public bool CanUploadFiles(object commandParameter)
        {
            return true;
        }

        public bool CanRenameFiles(object commandParameter)
        {
            var firstIndex = ChangeToName.IndexOf("*");
            var result = firstIndex != ChangeToName.LastIndexOf("*") && firstIndex != -1;
            return ChangeToName.Contains("*") && !result && FileNames != null && FileNames.Count > 0;
        }
    }
}
