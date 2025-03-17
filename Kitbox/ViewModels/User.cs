namespace Kitbox.ViewModels
{
    public abstract class User
    {
        protected string username = "";
        protected string password = "";
        protected string email = "";
        protected string userType = "";

        // This will change the window depending on the user type
        protected int windowId;

        // Public property to access windowId
        public int WindowId
        {
            get => windowId;
            set => windowId = value;
        }
    }
}
