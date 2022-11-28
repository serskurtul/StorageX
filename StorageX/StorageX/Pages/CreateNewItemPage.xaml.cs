using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using StorageX.Models;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using ZXing.Net.Mobile.Forms;

namespace StorageX.Pages
{
    public partial class CreateNewItemPage : ContentPage
    {
        private Item carpet;
        public string[] AllCategories { get; set; }
        public string Barcode
        {
            set
            {
                barcodeText.Text = value;
            }
        }
        private string PhotoName { get; set; }
        public CreateNewItemPage()
        {
            InitializeComponent();
            AllCategories = App.Database.GetCategories();
            BindingContext = this;
        }
        async Task TakePhotoAsync()
        {
            try
            {
                //var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions() { });

                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    CompressionQuality = 25,
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                    CustomPhotoSize = 75,
                });

                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED {PhotoName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW {ex.Message}");
            }
        }

        async Task LoadPhotoAsync( Plugin.Media.Abstractions.MediaFile photo)
        {
            if (photo == null)
            {
                PhotoName = null;
                return;
            }
            var fileName = Path.GetFileName(photo.Path);
            var newFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);
            using (var stream = photo.GetStream())
            {
                using (var newStream = File.OpenWrite(newFile))
                {
                    await stream.CopyToAsync(newStream);
                }
            }
            try
            {
                PhotoName = fileName;
            }
            catch (Exception)
            {
                Console.WriteLine("Помилка завантаження фото");
            }
        }

        private async void ScanerShow(System.Object sender, System.EventArgs e)
        {
            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    var carpets = await App.Database.GetCarpetsAsync();
                    var carpet = carpets.FirstOrDefault(x => x.Barcode == result.Text);
                    if (carpet != null)
                    {

                        await DisplayAlert("Знайдено", "Даний товар вже наявний.", "OK");
                        var page = new ItemDetailPage();
                        page.BindingContext = carpet;
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        barcodeText.Text = result.Text;
                    }
                });
            };
        }

        async void SaveClicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                //carpet = new Carpet
                //{
                //    Name = Nametb.Text,
                //    Price = Convert.ToDecimal(Pricetb.Text),
                //    Purchase = Convert.ToDecimal(Purchasetb.Text),
                //    Width = Convert.ToInt32(Widthtb.Text),
                //    Height = Convert.ToInt32(Heighttb.Text),
                //    ImageUrl = PhotoName,
                //    Amount = Convert.ToInt32(Amounttb.Text),
                //    Barcode = barcodeText.Text
                //};
                carpet = new Item();
                carpet.Name = Nametb.Text;
                carpet.Price = Convert.ToDecimal(Pricetb.Text);
                carpet.Purchase = Convert.ToDecimal(Purchasetb.Text);
                carpet.Width = Convert.ToDouble(Widthtb.Text);
                carpet.Height = Convert.ToDouble(Heighttb.Text);
                carpet.ImageName = PhotoName;
                carpet.isPath = Switcher.IsToggled;
                carpet.Amount = carpet.isPath ? 0 : Convert.ToInt32(Amounttb.Text);
                carpet.Barcode = barcodeText.Text;
                carpet.AddCategory(Picker.SelectedItem as string);

                //var selectedCategories = categories.SelectedItems as List<string>;
                //carpet.CategoriesStr = String.Join("$", selectedCategories);
                Console.WriteLine("carpet has been created");
                Console.WriteLine(await App.Database.PostCarpetAsync(carpet));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            await Navigation.PopAsync();
        }

        void ClearClicked(System.Object sender, System.EventArgs e)
        {
            PhotoImage.Source = null;
            PhotoName = null;
            Nametb.Text = "";
            Pricetb.Text = "";
            Purchasetb.Text = "";
            Widthtb.Text = "";
            Heighttb.Text = "";
            Amounttb.Text = "";
            barcodeText.Text = "";
            Switcher.IsToggled = false;
            Picker.SelectedItem = null;
        }

        void CapturePhoto(System.Object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await TakePhotoAsync();
                try
                {
                    PhotoImage.Source = ImageSource.FromFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.
                            ApplicationData), PhotoName));
                }
                catch (Exception)
                {
                    Console.WriteLine("Помилка завантаження фото");
                }
            });
        }

        void Switcher_Toggled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Amounttb.IsVisible = !Switcher.IsToggled;
        }
    }
}

