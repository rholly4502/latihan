using Microsoft.EntityFrameworkCore;
using WebApplicationDemo.RapidModels;

namespace WebApplicationDemo.EF
{
    public class ProductEF : IProduct
    {
        private readonly AppDbContext _appDbContext;
        public ProductEF(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Product Add(Product entity)
        {
            try
            {
                _appDbContext.Products.Add(entity);
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
                var deleteProduct = GetById(id);
                _appDbContext.Products.Remove(deleteProduct);
                _appDbContext.SaveChanges();
            }
            catch (Exception sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var results = _appDbContext.Products.OrderBy(p => p.ProductName).ToList();
            return results;
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            var result = _appDbContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            /*var result = (from c in _dbContext.Categories
                          where c.CategoryId == id
                          select c).FirstOrDefault();*/
            if (result == null)
            {
                throw new ArgumentException("Category not found");
            }
            return result;
        }

        public Product GetById(int id)
        {
            Product result = _appDbContext.Products.Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);
            if (result == null)
            {
                throw new Exception("Product not found");
            }
            return result;
        }

        public IEnumerable<Product> GetByProductName(string productName)
        {
            var results = _appDbContext.Products.Where(c => c.ProductName.Contains(productName)).ToList();

            return results;
        }

        public IEnumerable<Product> GetProducsWithCategory()
        {
            var results = _appDbContext.Products.Include(p => p.Category)
                .OrderBy(p => p.ProductName).ToList();
            return results;
        }

        public int GetProductStock(int productId)
        {
            throw new NotImplementedException();
        }

        public Product Update(Product entity)
        {
            try
            {
                var updateProduct = GetById(entity.ProductId);
                updateProduct.ProductName = entity.ProductName;
                updateProduct.CategoryId = entity.CategoryId;
                updateProduct.Stock = entity.Stock;
                updateProduct.Price = entity.Price;
                _appDbContext.SaveChanges();
                return updateProduct;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
