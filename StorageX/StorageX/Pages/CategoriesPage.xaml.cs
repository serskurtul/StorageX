using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StorageX.Pages
{
    public partial class CategoriesPage : ContentPage
    {
        public string[] Categories { get; set; }
        public CategoriesPage()
        {
            InitializeComponent();
            Categories = App.Database.GetCategories();
            BindingContext = this;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("Нова Категорія", "Введіть нову категорію");
            App.Database.PostCategories(result);
            Categories = App.Database.GetCategories();
            BindingContext = null;
        }
    }
}

