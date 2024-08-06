using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.EF
{
    public interface IProduct : ICrud<Product>
    {
        IEnumerable<Product> GetByCategory(int categoryId);
        IEnumerable<Product> GetByProductName(string productName);
        IEnumerable<Product> GetProducsWithCategory();
        int GetProductStock(int productId);
    }
}
