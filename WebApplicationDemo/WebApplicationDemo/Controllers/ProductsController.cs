using Microsoft.AspNetCore.Mvc;
using WebApplicationDemo.EF;
using WebApplicationDemo.DTO;
using WebApplicationDemo.RapidModels;
using WebApplicationDemo.RapidViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RapidBootcamp.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _product;
        public ProductsController(IProduct product)
        {
            _product = product;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<ProductDTO> Get()
        {
            List<ProductDTO> productDTOs = new List<ProductDTO>();
            var products = _product.GetProducsWithCategory();

            foreach (var product in products)
            {
                ProductDTO productDTO = new ProductDTO
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Stock = product.Stock,
                    Price = product.Price,
                    Category = new CategoryDTO
                    {
                        CategoryId = product.Category.CategoryId,
                        CategoryName = product.Category.CategoryName
                    }
                };
                productDTOs.Add(productDTO);
            }

            return productDTOs;
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public ProductDTO Get(int id)
        {
            var product = _product.GetById(id);

            var productDto = new ProductDTO
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Stock = product.Stock,
                Price = product.Price,
                Category = new CategoryDTO
                {
                    CategoryId = product.Category.CategoryId,
                    CategoryName = product.Category.CategoryName
                }
            };

            return productDto;
        }

        [HttpGet("ByCategory/{categoryId}")]
        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            var products = _product.GetByCategory(categoryId);
            return products;
        }

        [HttpGet("ByProductName")]
        public IEnumerable<Product> GetByProductName(string name)
        {
            var products = _product.GetByProductName(name);
            return products;
        }

        // POST api/<ProductsController>
        [HttpPost]
        public ActionResult Post(CreateProductDTO createProductDTO)
        {
            try
            {
                Product product = new Product
                {
                    ProductName = createProductDTO.ProductName,
                    CategoryId = createProductDTO.CategoryId,
                    Stock = createProductDTO.Stock,
                    Price = createProductDTO.Price
                };
                var result = _product.Add(product);

                ProductDTO productDTO = new ProductDTO
                {
                    ProductId = result.CategoryId,
                    ProductName = result.ProductName,
                    Stock = result.Stock,
                    Price = result.Price,
                    Category = new CategoryDTO
                    {
                        CategoryId = result.Category.CategoryId,
                    }
                };
                return CreatedAtAction(nameof(Get), new { id = result.ProductId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            var updateProduct = _product.GetById(id);
            if (updateProduct == null)
            {
                return NotFound();
            }
            try
            {
                updateProduct.ProductName = updateProductDto.ProductName;
                updateProduct.CategoryId = updateProductDto.CategoryId;
                updateProduct.Stock = updateProductDto.Stock;
                updateProduct.Price = updateProductDto.Price;
                var result = _product.Update(updateProduct);

                ProductDTO productDTO = new ProductDTO
                {
                    ProductId = result.ProductId,
                    ProductName = result.ProductName,
                    Stock = result.Stock,
                    Price = result.Price,
                    Category = new CategoryDTO
                    {
                        CategoryId = result.Category.CategoryId,
                        CategoryName = result.Category.CategoryName
                    }
                };
                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var deleteProduct = _product.GetById(id);
            if (deleteProduct == null)
            {
                return NotFound();
            }
            try
            {
                _product.Delete(id);
                return Ok($"Product {id} berhasil didelete..");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
