using System;
using System.Collections.Generic;
using StorageX.Models;
using Xamarin.Forms;
using System.Linq;

namespace StorageX.Pages
{
    public partial class SellPage : ContentPage
    {
        private Item item;
        public SellPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            item = (BindingContext as Item);
            if (item.isPath)
                amountStepper.Increment = 0.1;
            priceEntry.Text = Convert.ToString(item.Price);
        }
        void amountEntry_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            amountStepper.Value = double.Parse(amountEntry.Text);
            priceEntry.Text = Convert.ToString(item.Price * (decimal)amountStepper.Value);
        }

        void amountStepper_ValueChanged(System.Object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            if (amountEntry.Text != amountStepper.Value.ToString())
            {
                amountEntry.Text = amountStepper.Value.ToString();
                priceEntry.Text = Convert.ToString(item.Price * (decimal)amountStepper.Value);
            }
        }

        async void SaveButtonClicked(System.Object sender, System.EventArgs e)
        {
            if (item.isPath)
            {
                item.Height -= amountStepper.Value;
            }
            else
            {
                item.Amount -= (int)amountStepper.Value;
            }
            Invoice invoice = new Invoice()
            {
                ItemId = item.Id,
                Type = InvoiceType.Sell,
                Amount = amountStepper.Value,
                Total = decimal.Parse(priceEntry.Text)
            };
            await App.Database.PutCarpetAsync(item);
            App.Database.WriteData(invoice);
            await Navigation.PopAsync();
            
        }
    }
}

