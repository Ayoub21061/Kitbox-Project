using System;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Avalonia.Threading;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Kitbox.ViewModels
{
    public class SellerViewModel : ReactiveObject
    {
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set => this.RaiseAndSetIfChanged(ref _isEditing, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => this.RaiseAndSetIfChanged(ref _phone, value);
        }

        public ReactiveCommand<Unit, Unit> EditCommand { get; }
        public ReactiveCommand<Unit, Unit> ConfirmCommand { get; }

        public SellerViewModel()
        {
            Email = "example@example.com";
            Name = "John Doe";
            Address = "123 Street";
            Phone = "123-456-7890";

            // Command to enable editing mode
            EditCommand = ReactiveCommand.Create(
                () => { IsEditing = true; },
                outputScheduler: AvaloniaScheduler.Instance
            );

            // Command to confirm changes and exit editing mode
            ConfirmCommand = ReactiveCommand.Create(
                () => { IsEditing = false; },
                outputScheduler: AvaloniaScheduler.Instance
            );
        }
    }

    // Converter to handle IsVisible based on boolean values
    public class BooleanToIsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value;  // Return true to make the element visible

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value;  // Return the same boolean value for binding
    }

    // Inverse of the BooleanToIsVisibleConverter
    public class InverseBooleanToIsVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;  // Return false to make the element invisible

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;  // Return the inverted boolean value for binding
    }
}

