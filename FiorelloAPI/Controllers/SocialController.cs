using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Socials;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{
   
    public class SocialController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SocialController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var socials = await _context.Socials.ToListAsync();
            var socialDtos = _mapper.Map<List<SocialDto>>(socials);
            return Ok(socials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var social = await _context.Socials.FindAsync(id);

            if (social == null) return NotFound();

            var socialDto = _mapper.Map<SocialDto>(social);
            return Ok(social);
        }
       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SocialCreateDto request)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);

            var social = _mapper.Map<Social>(request);

            _context.Socials.Add(social);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<SocialCreateDto>(social);
            return CreatedAtAction(nameof(Create), social);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Social request)
        {
            if (id != request.Id) return BadRequest();

            var existingSocial = await _context.Socials.FindAsync(id);

            if (existingSocial == null) return NotFound();

            _mapper.Map(request, existingSocial);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var social = await _context.Socials.FindAsync(id);

            if (social == null)  return NotFound();

            _context.Socials.Remove(social);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
