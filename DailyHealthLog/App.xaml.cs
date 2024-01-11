using DailyHealthLog.Config;
using DailyHealthLog.Cryptography;
using DailyHealthLog.FileControl;
using DailyHealthLog.Models;
using DailyHealthLog.ViewModels;
using SQLite;
using System;
using System.IO;
using System.Windows;

namespace DailyHealthLog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (!File.Exists(Settings.EDBPATH))
                BuildDatabase();
            else
            {
                bool result = DecryptHealthLogs();
                if (result)
                    FileHelper.WipeFile(Settings.EDBPATH);
                else
                    App.Current.Shutdown();
            }

            base.OnStartup(e);
            MainWindow window = new MainWindow();
            var viewModel = new MainWindowViewModel();
            window.DataContext = viewModel;
            window.Show();
            window.Closing += viewModel.OnWindowClosing;
        }

        private bool DecryptHealthLogs()
        {
            AES.SetDefaultKey(Settings.KEY, Settings.AUTHKEY);
            byte[] eLogFile = File.ReadAllBytes(Settings.EDBPATH);
            try
            {
                using (FileStream fs = File.Open(Settings.DBPATH, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = AES.Decrypt(eLogFile);
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

        private void BuildDatabase()
        {
            if (!File.Exists(Settings.DBPATH))
                File.Create(Settings.DBPATH).Close();

            SQLiteConnection conn = new SQLiteConnection(Settings.DBPATH, SQLiteOpenFlags.ReadWrite);
            conn.CreateTable<DailyLogs>();
            conn.CreateTable<BloodPressure>();
            conn.CreateTable<Food>();
            conn.CreateTable<Liquid>();
            conn.CreateTable<Medication>();
            conn.CreateTable<Sleep>();
            conn.CreateTable<Weight>();
            conn.CreateTable<WeightDetail>();
            conn.CreateTable<Workout>();
            conn.Close();
        }
    }
}
