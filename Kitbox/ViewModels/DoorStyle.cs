namespace Kitbox.Models
{
    public class DoorStyle
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public DoorStyle(string name)
        {
            Name = name;
            IsSelected = false; // Par défaut, aucune porte n'est sélectionnée
        }
    }
}
