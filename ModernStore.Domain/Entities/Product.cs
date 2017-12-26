using ModernStore.Shared.Entities;

namespace ModernStore.Domain.Entities
{
    public class Product : Entity
    {
        protected Product() { }

        public Product(string title, decimal price, string image, int quantityHand)
        {
            Title = title;
            Price = price;
            Image = image;
            QuantityHand = quantityHand;
        }

        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int QuantityHand { get; set; }


        public void DecreaseQuantity(int quantity) => QuantityHand -= quantity;
    }
}
