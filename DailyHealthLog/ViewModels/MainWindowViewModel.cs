using DailyHealthLog.Config;
using DailyHealthLog.Cryptography;
using DailyHealthLog.FileControl;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace DailyHealthLog.ViewModels
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private ObservableCollection<WorkspaceViewModel> _workspaces;

        public ICommand MenuMain { get; set; }
        public ICommand MenuOpenDb { get; set; }
        public ICommand MenuCloseDb { get; set; }
        public ICommand MenuExit { get; set; }
        public ICommand MenuReport { get; set; }

        public MainWindowViewModel()
        {
            MenuMain = new RelayCommand(ShowMain);
            MenuOpenDb = new RelayCommand(OpenDatabase);
            MenuCloseDb = new RelayCommand(CloseDatabase);
            MenuExit = new RelayCommand(ExitDailyLogs);
            MenuReport = new RelayCommand(ShowReportWindow);

            ShowMainWindow();
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            bool result = EncryptHealthLogs();
            if (result)
                FileHelper.WipeFile(Settings.DBPATH);
        }

        private bool EncryptHealthLogs()
        {
            GC.Collect();
            AES.SetDefaultKey(Settings.KEY, Settings.AUTHKEY);
            byte[] logfile = FileHelper.GetFileBytes(Settings.DBPATH);
            if (logfile == null) return false;

            string savepath = $"{AppDomain.CurrentDomain.BaseDirectory}healthlogs.aes";
            try
            {
                using (FileStream fs = File.Open(savepath, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = AES.Encrypt(logfile);
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ShowMain(object obj)
        {
            ShowMainWindow();
        }

        public void ShowMainWindow()
        {
            Workspaces.Clear();
            MainViewModel workspace = new MainViewModel(Workspaces);
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        public void ShowReportWindow(object obj)
        {
            Workspaces.Clear();
            Report2ViewModel workspace = new Report2ViewModel(Workspaces);
            Workspaces.Add(workspace);
            SetActiveWorkspace(workspace);
        }

        private void OpenDatabase(object obj)
        {

        }

        private void CloseDatabase(object obj)
        {

        }

        private void ExitDailyLogs(object obj)
        {
            bool result = EncryptHealthLogs();
            if (result)
                FileHelper.WipeFile(Settings.DBPATH);

            App.Current.MainWindow.Close();
        }

        public void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
            collectionView?.MoveCurrentTo(workspace);
        }
    }
}
