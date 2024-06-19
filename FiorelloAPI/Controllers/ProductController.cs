using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Products;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{
    
  
    public class ProductController : BaseController
    {
        private readonly AppDbContext _context;     
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public ProductController(AppDbContext context,
                                 IWebHostEnvironment env,
                                 IMapper mapper)
        {
            _context = context;          
            _env = env;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
                .Include(m => m.ProductImages)
                .Include(m => m.Category)
                .ToListAsync();

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _context.Products
                .Include(m => m.Category)
                .Include(m => m.ProductImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            var productDto = _mapper.Map<ProductDetailDto>(product);
            return Ok(product);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto request)
        {
         
            if (!ModelState.IsValid) return BadRequest(ModelState);
                        
            if (_mapper == null) throw new NullReferenceException(nameof(_mapper) + " is not initialized.");
           
            if (_env == null) throw new NullReferenceException(nameof(_env) + " is not initialized.");
           
            var product = _mapper.Map<Product>(request);

            if (product.ProductImages == null)
            {
                product.ProductImages = new List<ProductImage>();
            }

            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                string path = Path.Combine(_env.WebRootPath, "img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }

                product.ProductImages.Add(new ProductImage { Name = fileName, IsMain = false });
            }
             _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);
                   
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDto);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] ProductEditDto request)
        {
            var existProduct = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existProduct == null) return NotFound();

            if (request.NewImages != null)
            {
                foreach (var item in request.NewImages)
                {
                    string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                    string path = Path.Combine(_env.WebRootPath, "img", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    existProduct.ProductImages.Add(new ProductImage { Name = fileName });
                }
            }

            _mapper.Map(request, existProduct);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{productId}/{imgId}")]
        public async Task<IActionResult> SetMainImage([FromRoute]int productId, [FromRoute]int imgId)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return NotFound();
          
            var currentMainImage = product.ProductImages.FirstOrDefault(i => i.IsMain);
            if (currentMainImage != null)
            {
                currentMainImage.IsMain = false;
            }

            var newMainImage = product.ProductImages.FirstOrDefault(i => i.Id == imgId);
            if (newMainImage != null)
            {
                newMainImage.IsMain = true;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
