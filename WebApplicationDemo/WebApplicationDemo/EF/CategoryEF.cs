using Microsoft.EntityFrameworkCore;
using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.EF
{
    public class CategoryEF : ICategory
    {
        private readonly AppDbContext _appDbContext;
        public CategoryEF(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Category Add(Category entity)
        {
            try
            {
                _appDbContext.Categories.Add(entity);
                _appDbContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var deleteCategory = GetById(id);
                _appDbContext.Categories.Remove(deleteCategory);
                _appDbContext.SaveChanges();
            }
            catch (Exception sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
        }

        public IEnumerable<Category> GetAll()
        {
            var results = _appDbContext.Categories.OrderBy(c => c.CategoryName).ToList();
            return results;
        }

        public IEnumerable<Category> GetByCategoryName(string categoryName)
        {
            var results = (from c in _appDbContext.Categories
                           where c.CategoryName.Contains(categoryName)
                           select c).ToList();

            return results;
        }

        public Category GetById(int id)
        {
            var result = _appDbContext.Categories.Find(id);
            if (result == null)
            {
                throw new Exception("Category not found");
            }
            return result;
        }

        public Category Update(Category entity)
        {
            try
            {
                var updateCategory = GetById(entity.CategoryId);
                updateCategory.CategoryName = entity.CategoryName;
                _appDbContext.SaveChanges();
                return updateCategory;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
