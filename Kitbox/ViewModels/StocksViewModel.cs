using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kitbox.Models;
using Kitbox.Services;
using ReactiveUI;
using System.Reactive;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;
using Avalonia.Threading; // Pour Dispatcher.UIThread

namespace Kitbox.ViewModels
{
    public class StocksViewModel : ReactiveObject
    {
        private readonly DatabaseService _dbService = new DatabaseService();

        public IRelayCommand OrderPage { get; set; }

        public ObservableCollection<Stock> Stocks { get; } = new ObservableCollection<Stock>();

        private Stock _selectedStock;
        public Stock SelectedStock
        {
            get => _selectedStock;
            set => this.RaiseAndSetIfChanged(ref _selectedStock, value);
        }

        public ReactiveCommand<Unit, Unit> AddStockCommand { get; }
        public ReactiveCommand<Unit, Unit> UpdateStockCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteStockCommand { get; }

        public StocksViewModel()
        {
            LoadStocks();

            AddStockCommand = ReactiveCommand.CreateFromTask(AddStock);
            UpdateStockCommand = ReactiveCommand.CreateFromTask(UpdateStock);
            DeleteStockCommand = ReactiveCommand.CreateFromTask(DeleteStock);
            OrderPage = new RelayCommand(OrderPageCommand);
        }

        public void OrderPageCommand()
        {
            var orderPage = new WindowSK();
            orderPage.Show();
        }

        private async Task LoadStocks()
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                Stocks.Clear();
                var stockList = _dbService.GetStocks();
                foreach (var stock in stockList)
                    Stocks.Add(stock);
            });
        }

        private async Task AddStock()
        {
            if (SelectedStock == null) return;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _dbService.AddStock(SelectedStock);
            });
            await LoadStocks();
        }

        private async Task UpdateStock()
        {
            if (SelectedStock == null) return;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _dbService.UpdateStock(SelectedStock);
            });
            await LoadStocks();
        }

        private async Task DeleteStock()
        {
            if (SelectedStock == null) return;

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _dbService.DeleteStock(SelectedStock.ProductId);
            });
            await LoadStocks();
        }
    }
}
