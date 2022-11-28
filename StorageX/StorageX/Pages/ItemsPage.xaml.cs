using System;
using System.Collections;
using System.Collections.Generic;
using StorageX.Models;
using Xamarin.Forms;
using StorageX;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Linq;

namespace StorageX.Pages
{
    public partial class ItemsPage : ContentPage
    {
        public string[] AllCategories { get; set; }

        public ItemsPage()
        {
            InitializeComponent();
            AllCategories = App.Database.GetCategories();
            BindingContext = this;
            pickerType.ItemsSource = new string[] { "Всe", "Коври", "Доріжки" };
            pickerType.SelectedIndex = 0;
        }
        async void list_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var carpet = e.SelectedItem as Item;
                var detailPage = new ItemDetailPage();
                detailPage.BindingContext = carpet;

                list.SelectedItem = null;
                await Navigation.PushAsync(detailPage);
            }
        }
        protected override async void OnAppearing()
        {
            try
            {
                list.ItemsSource = await App.Database.GetCarpetsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            FilterClear(null, null);
            filterView.IsVisible = false;
        }

        async void AddItem(System.Object sender, System.EventArgs e)
        {
            var createItemPage = new CreateNewItemPage();
            await Navigation.PushAsync(createItemPage);
        }

        void FilterShow(System.Object sender, System.EventArgs e)
        {
            filterView.IsVisible = !filterView.IsVisible;
        }

        async void FilterRun(System.Object sender, System.EventArgs e)
        {
            var selectedCategory = pickerCategory.SelectedItem as string;
            var selectedType = pickerType.SelectedIndex;
            List<Item> filteredItems = await App.Database.GetCarpetsAsync();

            switch (pickerType.SelectedIndex)
            {
                case 1:
                    filteredItems = filteredItems.Where(x => x.isPath == false).ToList();
                    break;
                case 2:
                    filteredItems = filteredItems.Where(x => x.isPath == true).ToList();
                    break;
            }

            if (selectedCategory != null)
            {
                filteredItems = filteredItems.FindAll(x =>
                {
                    if (x.Categories != null)
                        return x.Categories.Contains(selectedCategory);
                    else
                        return false;
                });
            }
            list.ItemsSource = filteredItems;
        }

        void FilterClear(System.Object sender, System.EventArgs e)
        {
            pickerCategory.SelectedIndex = -1;
            pickerType.SelectedIndex = -1;
            FilterRun(null, null);
        }
    }
}

