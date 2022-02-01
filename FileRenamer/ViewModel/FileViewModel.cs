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

        private List<string> _fileNames;

        public List<string> FileNames
        {
            get { return _fileNames; }
            set { SetProperty(ref _fileNames, value); }
        }

        private string _seperator;

        public string Seperator
        {
            get { return _seperator; }
            set { SetProperty(ref _seperator, value); }
        }


        public ICommand UploadFiles => _uploadFiles;

        public FileViewModel()
        {
            // In progress
            _uploadFiles = new DelegateCommand(OnUploadFiles, CanUploadFiles);
            _seperator = "seperator";
        }

        public void OnUploadFiles(object commandParameter)
        {
            _openFileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            _openFileDialog.ShowDialog();
            FileNames = new List<string>(_openFileDialog.SafeFileNames);
            Trace.WriteLine(Seperator);
        }

        public bool CanUploadFiles(object commandParameter)
        {
            return true;
        }
    }
}
