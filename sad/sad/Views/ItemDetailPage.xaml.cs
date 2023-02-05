using System.ComponentModel;
using Xamarin.Forms;
using sad.ViewModels;

namespace sad.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
