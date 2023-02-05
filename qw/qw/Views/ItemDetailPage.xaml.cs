using System.ComponentModel;
using Xamarin.Forms;
using qw.ViewModels;

namespace qw.Views
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
