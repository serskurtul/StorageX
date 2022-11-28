using System;
using System.Collections.Generic;
using ZXing.Net.Mobile.Forms;
using Xamarin.Forms;
using StorageX.Models;
using System.Linq;
using WebKit;

namespace StorageX.Pages
{
    public partial class ScanPage : ZXingScannerPage
    {

        public ScanPage()
        {
            InitializeComponent();
        }

        void Handle_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var carpets = await App.Database.GetCarpetsAsync();
                await Navigation.PopAsync();
                var carpet = carpets.FirstOrDefault(x => x.Barcode == result.Text);
                if (carpet == null)
                {
                    var page = new CreateNewItemPage();
                    page.Barcode = result.Text;
                    await Navigation.PushAsync(page);
                }
                else
                {
                    var page = new ItemDetailPage();
                    page.BindingContext = carpet;
                    await Navigation.PushAsync(page);
                }
            });
        }
    }
}
