using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StorageX.Pages
{
    public partial class DateInvoice : ContentPage
    {
        List<InvoicesPage.InvoiceViewModel> invoices = new();
        public DateInvoice()
        {
            InitializeComponent();
        }

        void datePicker_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            invoices.Clear();
            labelSells.Text = "";
            foreach (var item in App.Database.GetData(datePicker.Date))
            {
                var invoice = new InvoicesPage.InvoiceViewModel(item);
                labelSells.Text += invoice.GetInvoiceStr() + "\n";
                invoices.Add(invoice);
            }
            if(invoices.Count != 0)
                SellsPanel.IsVisible = true;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new InvoicesPage(invoices));
        }
    }
}