namespace Kitbox.ViewModels
{
    public abstract class AppUser
    {
<<<<<<< HEAD
        public string Username { get; set; } = string.Empty;
=======
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
>>>>>>> main
    }
}
