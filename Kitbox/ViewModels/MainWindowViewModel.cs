using ReactiveUI;
using System.Reactive;
using Kitbox.ViewModels;

namespace Kitbox.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
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
            // Par défaut, commencer avec SellerView
            CurrentViewModel = new SellerViewModel();
            
            ShowSellerViewCommand = ReactiveCommand.Create(
                () => {
                    CurrentViewModel = new SellerViewModel();
                    return Unit.Default;
                }
            );

            ShowEmptyViewCommand = ReactiveCommand.Create(
                () => {
                    CurrentViewModel = new EmptyViewModel();
                    return Unit.Default;
                }
            );
        }
    }
}
