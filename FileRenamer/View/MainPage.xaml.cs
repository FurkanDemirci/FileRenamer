using FileRenamer.Model;
using FileRenamer.ViewModel;
using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace FileRenamer.View
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private readonly FileViewModel fileViewModel;

        public MainPage()
        {
            fileViewModel = new FileViewModel();
            DataContext = fileViewModel;
            InitializeComponent();
        }
    }
}
