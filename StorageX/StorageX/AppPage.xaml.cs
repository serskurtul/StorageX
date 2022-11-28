using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StorageX
{
    public partial class AppPage : FlyoutPage
    {
        public AppPage()
        {
            InitializeComponent();
            flyout.list.ItemSelected += OnSelectedItem;
        }

        private void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PageModel.FlyoutItemPage;
            if (item != null)
            {
                try
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                flyout.list.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

