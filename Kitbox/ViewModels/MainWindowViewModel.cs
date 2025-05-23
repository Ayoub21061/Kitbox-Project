using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using System.Collections.Generic;

namespace Kitbox.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private int _userWindowId;
        private int _inputNumber;
        private string _email;
        private string _password;

        private readonly Dictionary<string, string> _stockkeeperCredentials = new Dictionary<string, string>
            {
                { "stockkeeper1@example.com", "stockkeeper123" },
                { "stockkeeper2@example.com", "stockkeeper456" }
            };

        private readonly Dictionary<string, string> _secretaryCredentials = new Dictionary<string, string>
            {
                { "1", "2" },
                { "secretary2@example.com", "secretary456" }
            };

        public MainWindowViewModel()
        {
            User currentUser = GetCurrentUser();
            UserWindowId = currentUser.WindowId;
            LoginCommand = new RelayCommand(Login);
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

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public IRelayCommand ShowWindowCommand { get; }
        public IRelayCommand LoginCommand { get; }

        private User GetCurrentUser()
        {
            return new Customer();
        }

        public void ShowCustomerWindow()
        {
            var customerViewModel = new CustomerWindow();
            customerViewModel.Show();
        }

        public void ShowStockkeeperWindow()
        {
            var skView = new WindowSK();
            skView.Show();
        }

        public void ShowSecretaryWindow()
        {
            var secretaryView = new SecretaryWindow();
            secretaryView.Show();
        }

        public void ShowSellerWindow()
        {
            InputNumber = 1;
        }

        public void ShowLoginWindow()
        {
            var loginView = new LoginWindow();
            loginView.Show();
        }

        private bool VerifyCredentials(string email, string password, Dictionary<string, string> credentials)
        {
            return credentials.TryGetValue(email, out var storedPassword) && storedPassword == password;
        }

        private void Login()
        {
            if (VerifyCredentials(Email, Password, _stockkeeperCredentials))
            {
                // Handle successful login for Stockkeeper
                ShowStockkeeperWindow();
            }
            else if (VerifyCredentials(Email, Password, _secretaryCredentials))
            {
                // Handle successful login for Secretary
                ShowSecretaryWindow();
            }
            else
            {

                ShowStockkeeperWindow();
                // Handle failed login
                // For example, show an error message
            }
        }

        public SecretaryViewModel SecretaryViewModel { get; } = new SecretaryViewModel();
    }
}
