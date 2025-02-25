

namespace Kitbox.ViewModels
{
    public abstract class User
    {
        protected string username = "";
        protected string password = "";
        protected string email = "";

        //This will change the window depending on the user type
        protected int windowId = 0;
        
        
    }
}
