using FileRenamer.Model;
using FileRenamer.ViewModel.Binding;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        }

        public void OnUploadFiles(object commandParameter)
        {
            _openFileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            _openFileDialog.ShowDialog();
            FileNames = new List<File>();

            foreach (var fileName in _openFileDialog.SafeFileNames)
            {
                FileNames.Add(new File { Name = fileName, IsSelected = false });
            }

            OriginalFileName = _openFileDialog.SafeFileName;
        }

        public void OnRenameFiles(object commandParameter)
        {
            Trace.WriteLine(FileNames);
        }

        public bool CanUploadFiles(object commandParameter)
        {
            return true;
        }

        public bool CanRenameFiles(object commandParameter)
        {
            return true;
        }
    }
}
