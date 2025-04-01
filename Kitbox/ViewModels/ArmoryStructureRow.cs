using System;

namespace Kitbox.ViewModels
{
    public class ArmoryStructureRow
    {
        // Propriété représentant la clé (le nom de l'attribut)
        public string? Key { get; set; }

        // Propriété représentant la valeur associée à la clé
        public string? Value { get; set; }

        // Constructeur par défaut
        public ArmoryStructureRow() { }

        // Constructeur avec paramètres pour initialiser rapidement une ligne
        public ArmoryStructureRow(string key, string value)
        {
            Key = key;
            Value = value;
        }

        // Optionnel : redéfinition de ToString() pour faciliter le débogage ou l'affichage
        public override string ToString()
        {
            return $"{Key}: {Value}";
        }
    }
}
