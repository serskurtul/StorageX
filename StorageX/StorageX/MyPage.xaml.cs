using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StorageX
{
    public partial class MyPage : ContentPage
    {
        public double someText { get; set; }
        public MyPage()
        {
            InitializeComponent();
            someText = 13.13d;
            BindingContext = this;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            someText++;
            RefreshBinding();
        }
        void RefreshBinding()
        {
            var temp = BindingContext;
            BindingContext = null;
            BindingContext = temp;
        }
    }
}

