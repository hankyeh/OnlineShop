

namespace ShopBackend.ViewModels
{
    public class ViewCategory
    {
        public ViewCategory()
        {
            Product = new HashSet<ViewProduct>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ViewProduct> Product { get; set; }

    }
}

