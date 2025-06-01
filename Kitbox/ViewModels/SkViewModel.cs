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
        

        public SkViewModel()
        {
            _dbService = new DatabaseService();

            if (!_dbService.TesterConnexion())
            {
                System.Diagnostics.Debug.WriteLine("Erreur : impossible de se connecter à la base de données !");
            }

            var ordersList = _dbService.GetOrders();
            System.Diagnostics.Debug.WriteLine("SkViewModel créé");
            System.Diagnostics.Debug.WriteLine($"Orders récupérés: {ordersList.Count}");

            Orders = new ObservableCollection<Order_Client>(ordersList);


            foreach (var order in Orders)
            {
                System.Diagnostics.Debug.WriteLine($"Order: {order.OrderId}, {order.ClientId}, {order.StatusDelivery}");
            }

            StocksPage = new RelayCommand(StocksPageCommand);


        }

        public IRelayCommand SkPageCommand { get; }

       
        public void StocksPageCommand()
        {
            var StocksPage = new Stocks();
            StocksPage.Show();
        }
    }
}
