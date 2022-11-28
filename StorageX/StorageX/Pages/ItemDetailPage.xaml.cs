using System;
using System.Collections.Generic;
using StorageX.Models;
using Xamarin.Forms;
using System.Linq;
using System.Runtime.InteropServices;

namespace StorageX.Pages
{
    public partial class ItemDetailPage : ContentPage
    {
        bool isChangeAmount = false;
        bool isDelete = false;
        public ItemDetailPage()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            var carpet = BindingContext as Item;
            BindingContext = null;
            BindingContext = carpet;
            if (carpet.isPath)
            {
                productType.Text = "Доріжка";
                amountView.IsVisible = false;
            }
            else
            {
                productType.Text = "Коврик";
            }
        }
        void BuyClicked(System.Object sender, System.EventArgs e)
        {
            ChangeAmountView();
        }

        void SellClicked(System.Object sender, System.EventArgs e)
        {
            var sp = new SellPage();
            sp.BindingContext = BindingContext as Item;
            Navigation.PushAsync(sp);
        }
        /// <summary>
        /// create a entry, stepper and button. 
        /// </summary>
        /// <param name="koef">write -1 to substract</param>
        void ChangeAmountView(int koef = 1)
        {
            if (isChangeAmount)
                return;
            var label = new Label { Text = "Введіть кількість" };
            contentLayout.Children.Add(label);
            Stepper stepper;
            if ((BindingContext as Item).isPath)
                stepper = new Stepper { Minimum = 0, Increment = 0.1 };
            else stepper = new Stepper { Minimum = 0, Increment = 1 };

            var entry = new Entry() { Keyboard = Keyboard.Numeric };
            entry.Text = "0";
            entry.WidthRequest = 50;
            stepper.ValueChanged += (sender, e) => entry.Text = e.NewValue.ToString();
            var stacklayout = new StackLayout();
            stacklayout.Orientation = StackOrientation.Horizontal;
            stacklayout.Children.Add(entry);
            stacklayout.Children.Add(stepper);

            contentLayout.Children.Add(stacklayout);

            var saveButton = new Button() { Text = "Зберегти", TextColor = Color.Black, BackgroundColor = Color.Transparent, BorderWidth = 2 };
            contentLayout.Children.Add(saveButton);
            scrollView.ScrollToAsync(0, scrollView.Height / 2, true);
            saveButton.Clicked += async (sender, e) =>
            {

                var carpet = BindingContext as Item;
                
                if (carpet.isPath)
                {
                    carpet.Height += stepper.Value * koef;
                    heighttb.Text = carpet.Height.ToString();
                }
                else
                {
                    carpet.Amount += (int)stepper.Value * koef;
                    amounttb.Text = carpet.Amount.ToString();
                }
                Console.WriteLine(carpet.Price);
                await App.Database.PutCarpetAsync(carpet);
                var res = await App.Database.GetCarpetsAsync();
                contentLayout.Children.Remove(label);
                contentLayout.Children.Remove(stacklayout);
                contentLayout.Children.Remove(saveButton);
                isChangeAmount = false;
            };
            isChangeAmount = true;
        }
        
        void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
        }

        void Edit(System.Object sender, System.EventArgs e)
        {
            
            if (!isDelete)
            {
                isDelete = true;
                var deleteButton = new Button() { Text = "Видалити", BackgroundColor = Color.Red, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 100 };
                contentLayout.Children.Add(deleteButton);
                deleteButton.Clicked += async (sender, e) =>
                {
                    var answer = await DisplayActionSheet("Видалити предмет?", "Відміна", "Видалити");
                    if (answer == "Видалити")
                    {
                        await App.Database.DeleteCarpetAsync(BindingContext as Item);
                        await Navigation.PopAsync();
                    }
                };
            }
        }
    }
}
