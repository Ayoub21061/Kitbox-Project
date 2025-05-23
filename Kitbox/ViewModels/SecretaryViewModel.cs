
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using Kitbox.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.IO;
using System;

namespace Kitbox.ViewModels
{
    public partial class SecretaryViewModel : ObservableObject
    {
        public  Secretary _secretary;

        public IRelayCommand SecondSecretaryPageCommand { get; }
        public IRelayCommand ?SaveSecretaryCommand { get; }

        public SecretaryViewModel()
        {
            _secretary = new Secretary();

            SecondSecretaryPageCommand = new RelayCommand(SecondNextPageSec);
            //SaveSecretaryCommand = new RelayCommand(SaveSecretaryDataToJson); // Initialisation
        }

        // Naviguer vers la deuxième page
        private void SecondNextPageSec()
        {
            var secondPage = new SecondSecretaryPageView();
            secondPage.Show();
        }
    
    }
}
