using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.DTO
{
    public class CreateProductDTO
    {
        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public int Stock { get; set; }

        public decimal Price { get; set; }

    }
}
