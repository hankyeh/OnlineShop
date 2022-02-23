using ShopBackend.Models;

namespace ShopBackend.ViewModels
{
    public class ViewCategory
    {
        public ViewCategory()
        {
            Product = new HashSet<Product>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Product { get; set; }

    }
}

