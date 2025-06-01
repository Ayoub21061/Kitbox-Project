using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Models;
using Kitbox.Services;
using Kitbox.Views;
using System;
using System.Collections.ObjectModel;

namespace Kitbox.ViewModels
{
    public class SkViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        private ObservableCollection<Order_Client> _orders;
        public ObservableCollection<Order_Client> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }


        public IRelayCommand StocksPage { get; }
                // Tu peux aussi gérer une UI ou message utilisateur ici

            foreach (var order in Orders)
            {
                System.Diagnostics.Debug.WriteLine($"Order: {order.OrderId}, {order.ClientId}, {order.StatusDelivery}");
            }

            StocksPage = new RelayCommand(StocksPageCommand);
            SkPageCommand = new RelayCommand(ShowSkPage);
            foreach (var order in Orders)
            {
                System.Diagnostics.Debug.WriteLine($"Order: {order.OrderId}, {order.ClientId}, {order.StatusDelivery}");
            }
        public void StocksPageCommand()
        private void ShowSkPage()
    }
}
