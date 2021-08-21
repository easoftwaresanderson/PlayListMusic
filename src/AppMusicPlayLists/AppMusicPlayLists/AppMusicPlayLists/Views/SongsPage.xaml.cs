using AppMusicPlayLists.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppMusicPlayLists.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongsPage : ContentPage
    {
        private SongsViewModel _vm;
        public SongsPage()
        {
            InitializeComponent();
            _vm = new SongsViewModel();
            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }
    }
}