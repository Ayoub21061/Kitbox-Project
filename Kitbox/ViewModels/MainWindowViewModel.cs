﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;

namespace Kitbox.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private int _userWindowId;
        private int _inputNumber;

        public MainWindowViewModel()
        {
            User currentUser = GetCurrentUser();
            UserWindowId = currentUser.WindowId;
            InputNumber = 0;

            // Initialize commands
            ShowWindowCommand = new RelayCommand(ExecuteShowWindowCommand);
            TestNewWindowCommand = new RelayCommand(ExecuteTestNewWindowCommand);
        }

        public int UserWindowId
        {
            get => _userWindowId;
            set => SetProperty(ref _userWindowId, value);
        }

        public int InputNumber
        {
            get => _inputNumber;
            set => SetProperty(ref _inputNumber, value);
        }

        public IRelayCommand ShowWindowCommand { get; }
        public IRelayCommand TestNewWindowCommand { get; }

        private User GetCurrentUser()
        {
            return new Customer();
        }

        private void ShowViewModel(int inputNumber)
        {
            switch (inputNumber)
            {
                case 1:
                    var customerViewModel = new CustomerWindow();
                    customerViewModel.Show();
                    // Logic to show the customer view model
                    break;
                case 2:
                    var skView = new WindowSK();
                    skView.Show();
                    // Logic to show the stock keeper view model
                    break;
                default:
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    break;
            }
        }

        private void ExecuteShowWindowCommand()
        {
            ShowViewModel(InputNumber);
        }

        private void ExecuteTestNewWindowCommand()
        {
            var second= new SecondPageView();
            second.Show();
        }


    }
}