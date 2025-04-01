using System;

namespace Kitbox.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        public Supplier(int id, string name)
        {
            SupplierId = id;
            SupplierName = name;
        }
    }
}
