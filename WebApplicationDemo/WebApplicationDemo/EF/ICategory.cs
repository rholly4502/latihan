using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.EF
{
    public interface ICategory : ICrud<Category>
    {
        IEnumerable<Category> GetByCategoryName(string categoryName);
    }
}
