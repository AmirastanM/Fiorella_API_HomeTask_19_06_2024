using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Blogs;
using FiorelloAPI.Helpers.Extensions;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{

    public class BlogController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public BlogController(AppDbContext context,
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
            var response = await _context.Blogs.ToListAsync();

            var mappedDatas = _mapper.Map<List<BlogDto>>(response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            var mappedBlog = _mapper.Map<BlogDto>(blog);

            return Ok(mappedBlog);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BlogCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var blog = _mapper.Map<Blog>(request);

            string fileName = $"{Guid.NewGuid()}-{request.Image.FileName}";
            string path = Path.Combine(_env.WebRootPath, "img", fileName);

            if (!Directory.Exists(Path.Combine(_env.WebRootPath, "img")))
            {
                Directory.CreateDirectory(Path.Combine(_env.WebRootPath, "img"));
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            blog.Image = fileName;

            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = blog.Id }, blog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] BlogEditDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingBlog = await _context.Blogs.FindAsync(id);

            if (existingBlog == null) return NotFound();

            _mapper.Map(request, existingBlog);

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int? id)
        {
            if (id is null) return BadRequest();
            var data = await _context.Blogs.FindAsync(id);
            if (data == null) return NotFound();

            _context.Remove(data);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
