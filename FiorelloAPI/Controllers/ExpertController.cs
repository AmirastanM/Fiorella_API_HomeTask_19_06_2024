using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Experts;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{
    
    public class ExpertController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ExpertController(AppDbContext context,
                              IMapper mapper,
                              IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;

        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute]int id)
        {
            var expert = await _context.Experts.FindAsync(id);
            if (expert == null) return NotFound();

            var expertDto = _mapper.Map<ExpertDto>(expert);
            return Ok(expertDto);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var experts = await _context.Experts.ToListAsync();
            var expertDtos = _mapper.Map<IEnumerable<ExpertDto>>(experts);
            return Ok(expertDtos);
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ExpertCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var expert = _mapper.Map<Expert>(request);
            _context.Experts.Add(expert);
            await _context.SaveChangesAsync();

            var expertDto = _mapper.Map<ExpertDto>(expert);
            return CreatedAtAction(nameof(Create), request);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int? id)
        {
            var expert = await _context.Experts.FindAsync(id);
            if (expert == null) return NotFound();

            _context.Experts.Remove(expert);
            await _context.SaveChangesAsync();

            return Ok();
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit([FromRoute]int id,[FromForm] ExpertEditDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var expert = await _context.Experts.FindAsync(id);
            if (expert == null) return NotFound();

            _mapper.Map(request, expert);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
