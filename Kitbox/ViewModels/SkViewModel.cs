using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Kitbox.Views;


namespace Kitbox.ViewModels
{
    class SkViewModel : ObservableObject
    {
        public SkViewModel()
        {

            SkPageCommand = new RelayCommand(ShowSkPage);
        }
        public IRelayCommand SkPageCommand { get; }
        
        private void ShowSkPage()
        {
            var SkPage = new WindowSK();
            SkPage.Show();
        }
        
    }
    
        
        
}
