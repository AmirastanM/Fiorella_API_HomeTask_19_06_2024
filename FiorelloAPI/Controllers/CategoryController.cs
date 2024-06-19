using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Categories;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{
    
    public class CategoryController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context,
                              IMapper mapper,
                              IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArchivePaginate([FromQuery] int page, [FromQuery] int take)
        {
            var categories = await _context.Categories
                .IgnoreQueryFilters()
                .Where(m => m.SoftDeleted)
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .ToListAsync();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWithProductCount()
        {
            var categories = await _context.Categories
                .Include(m => m.Products)
                .OrderByDescending(m => m.Id)
                .ToListAsync();

            var categoryProductDtos = _mapper.Map<IEnumerable<CategoryProductDto>>(categories);
            return Ok(categoryProductDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            var count = await _context.Categories.CountAsync();
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> GetArchiveCount()
        {
            var count = await _context.Categories
                .IgnoreQueryFilters()
                .Where(m => m.SoftDeleted)
                .CountAsync();
            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginate([FromQuery] int page, [FromQuery] int take)
        {
            var categories = await _context.Categories
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .Include(m => m.Products)
                .ToListAsync();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var category = await _context.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null) return NotFound();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdWithProducts([FromRoute] int id)
        {
            var category = await _context.Categories
                .Where(m => m.Id == id)
                .Include(m => m.Products)
                .FirstOrDefaultAsync();

            if (category == null) return NotFound();

            var categoryProductDto = _mapper.Map<CategoryProductDto>(category);
            return Ok(categoryProductDto);
        }

        [HttpGet]
        public async Task<IActionResult> Exist([FromQuery] string name)
        {
            var exists = await _context.Categories.AnyAsync(m => m.Name.Trim() == name.Trim());
            return Ok(exists);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var category = _mapper.Map<Category>(request);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, categoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ExistExceptById([FromRoute] int id, [FromQuery] string name)
        {
            var exists = await _context.Categories.AnyAsync(m => m.Name == name && m.Id != id);
            return Ok(exists);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSelected()
        {
            var categories = await _context.Categories
                .OrderBy(m => m.Name)
                .ToListAsync();

            var selectList = new SelectList(categories, "Id", "Name");
            return Ok(selectList);
        }
    }
}
