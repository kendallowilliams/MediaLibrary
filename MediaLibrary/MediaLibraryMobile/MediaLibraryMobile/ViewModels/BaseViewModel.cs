using MediaLibraryMobile.ViewModels.Interfaces;
using MediaLibraryMobile.Views.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MediaLibraryMobile.ViewModels
{
    public class BaseViewModel<TView>: IViewModel where TView: IView
    {
        private string title;
        private readonly TView view;

        public BaseViewModel(TView view)
        {
            this.view = view;
            this.view.BindingContext = this;
        }

        public string Title { get => title; set => SetProperty<string>(ref title, value); }

        public Page View { get => view as Page; }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
