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
    public partial class PlayListPage : ContentPage
    {
        private FavoritesViewModel _vm;

        public PlayListPage()
        {
            InitializeComponent();
            _vm = new FavoritesViewModel();

            this.BindingContext = _vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _vm.LoadItemsCommand.Execute(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
           
        }
    }
}