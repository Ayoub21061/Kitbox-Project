using ReactiveUI;
using System.Reactive;
using Kitbox.ViewModels;
using Avalonia.Threading;

namespace Kitbox.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        
        private SellerViewModel _sellerViewModel;
        private EmptyViewModel _emptyViewModel;
        private ReactiveObject _currentViewModel;
        public ReactiveObject CurrentViewModel
        {
            get => _currentViewModel;
            set => this.RaiseAndSetIfChanged(ref _currentViewModel, value);
        }

        public ReactiveCommand<Unit, Unit> ShowSellerViewCommand { get; }
        public ReactiveCommand<Unit, Unit> ShowEmptyViewCommand { get; }

        public MainWindowViewModel()
        {
            Dispatcher.UIThread.Post(() =>
            {
                _sellerViewModel = new SellerViewModel();
                _emptyViewModel = new EmptyViewModel();
                CurrentViewModel = _sellerViewModel;
            });

            ShowSellerViewCommand = ReactiveCommand.Create(() =>
            {
                Dispatcher.UIThread.Post(() => CurrentViewModel = _sellerViewModel);
                return Unit.Default;
            });

            ShowEmptyViewCommand = ReactiveCommand.Create(() =>
            {
                Dispatcher.UIThread.Post(() => CurrentViewModel = _emptyViewModel);
                return Unit.Default;
            });

            
        }
    }
}
