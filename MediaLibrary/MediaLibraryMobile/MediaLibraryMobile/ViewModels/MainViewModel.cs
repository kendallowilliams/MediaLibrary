using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using Xamarin.Forms;
using static MediaLibraryMobile.Enums;
using System.Windows.Input;

namespace MediaLibraryMobile.ViewModels
{
    [Export]
    public class MainViewModel : BaseViewModel<IView>
    {
        private IDictionary<Pages, string> menuItems;
        private ICommand menuItemCommand;
        private KeyValuePair<Pages, string> selectedMenuItem;

         [ImportingConstructor]
        public MainViewModel(IMainView mainView, IMenuView menuView): base(mainView)
        {
            mainView.Master = menuView as Page;
            mainView.Master.BindingContext = this;
            Title = nameof(MainViewModel);
        }

        public IDictionary<Pages, string> MenuItems { get => menuItems; set => SetProperty<IDictionary<Pages, string>>(ref menuItems, value); }
        public ICommand MenuItemCommand { get => menuItemCommand; set => SetProperty<ICommand>(ref menuItemCommand, value); }
        public KeyValuePair<Pages, string> SelectedMenuItem { get => selectedMenuItem; set => SetProperty<KeyValuePair<Pages, string>>(ref selectedMenuItem, value); }
    }
}
