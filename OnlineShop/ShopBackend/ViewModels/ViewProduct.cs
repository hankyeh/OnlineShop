namespace ShopBackend.ViewModels
{
    public class ViewProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public string ProdImg { get; set; }
        public int CategoryId { get; set; }
        public virtual ViewCategory Category { get; set; }
    }
}
