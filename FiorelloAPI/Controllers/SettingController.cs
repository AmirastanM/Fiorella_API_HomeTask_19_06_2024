using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Settings;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{
    

    public class SettingController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SettingController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

    
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var settings = await _context.Settings.ToListAsync();
            var settingsDto = _mapper.Map<List<SettingDto>>(settings);
            return Ok(settings);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var setting = await _context.Settings.FindAsync(id);

            if (setting == null) return NotFound();

            var settingDto = _mapper.Map<SettingDto>(setting);
            return Ok(settingDto);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SettingDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
           
            var setting = _mapper.Map<Setting>(request);
            
            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();

            var settingDto = _mapper.Map<SettingDto>(setting);

            return CreatedAtAction(nameof(GetById), new { id = setting.Id }, settingDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Setting request)
        {
            if (id != request.Id) return BadRequest();

            var existingSetting = await _context.Settings.FindAsync(id);
            if (existingSetting == null) return NotFound();

            _mapper.Map(request, existingSetting);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var setting = await _context.Settings.FindAsync(id);

            if (setting == null) return NotFound();

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
