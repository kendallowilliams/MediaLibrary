using LibVLCSharp.Forms.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MediaLibraryMobile.Controls
{
    public class CustomPlaybackControls : PlaybackControls
    {
        public CustomPlaybackControls() : base()
        {
            IsRewindButtonVisible = false;
            IsSeekButtonVisible = false;
        }

        public static readonly BindableProperty PreviousCommandProperty = BindableProperty.Create(nameof(PreviousCommand), typeof(ICommand), typeof(CustomPlaybackControls));

        public static readonly BindableProperty NextCommandProperty = BindableProperty.Create(nameof(NextCommand), typeof(ICommand), typeof(CustomPlaybackControls));

        public ICommand PreviousCommand { get => (ICommand)GetValue(PreviousCommandProperty); set => SetValue(PreviousCommandProperty, value); }

        public ICommand NextCommand { get => (ICommand)GetValue(NextCommandProperty); set => SetValue(NextCommandProperty, value); }
    }
}