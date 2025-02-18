namespace Kitbox.ViewModels
{
    public class MainWindowViewModel
    {

        //get the window Id here according to the user type to show the right window.

        public CustomerViewModel CustomerViewModel { get; } = new CustomerViewModel();
    }
}