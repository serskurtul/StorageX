using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Threading;
using StorageX.Pages;

namespace StorageX
{
    public partial class App : Application
    {
        private static Database _database;

        public static Database Database
        {
            get
            {
                if(_database == null)
                {
                    _database = new Database(
                        Environment.GetFolderPath(Environment.SpecialFolder.
                        LocalApplicationData));
                }
                return _database;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new AppPage();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

