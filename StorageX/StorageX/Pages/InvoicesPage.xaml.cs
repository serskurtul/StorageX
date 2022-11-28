using System;
using System.Collections.Generic;
using Xamarin.Forms;
using StorageX.Models;
using System.Diagnostics;
using System.IO;

namespace StorageX.Pages
{
    public partial class InvoicesPage : ContentPage
    {
        public InvoicesPage(List<InvoiceViewModel> invoicesViewModel)
        {
            InitializeComponent();
            list.ItemsSource = invoicesViewModel;
        }

        void list_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {

        }
        public class InvoiceViewModel
        {
            public InvoiceViewModel(Invoice invoice)
            {
                Id = invoice.Id;
                Type = invoice.Type;
                ItemId = invoice.ItemId;
                Amount = invoice.Amount;
                Total = invoice.Total;
                DateTime = invoice.DateTime;

                var item = App.Database.GetCarpetAsync(ItemId).Result;
                Name = item.Name;
                ItemsAmount = item.Amount;
                Price = item.Price;
                Purchase = item.Purchase;
                ImageFullPath = item.ImageFullPath;
                ItemSize = item.Height.ToString() + "x" + item.Width.ToString();
            }
            public Guid Id { get; set; }
            public InvoiceType Type { get; set; }
            public int ItemId { get; set; }
            public double Amount { get; set; }
            public decimal Total { get; set; }
            public DateTime DateTime { get; set; }

            public string Name { get; set; }
            public int ItemsAmount { get; set; }
            public decimal Price { get; set; }
            public decimal Purchase { get; set; }
            public string ImageFullPath { get; set; }
            public string ItemSize { get; set; }
            public string Description
            {
                get
                {
                    return ItemSize + "- ціна:" + Price;
                }
            }
            public string GetInvoiceStr()
            {
                return Name + " " + Description
                    + " " + Amount + "/" + Total;
            }
        }
    }
}

