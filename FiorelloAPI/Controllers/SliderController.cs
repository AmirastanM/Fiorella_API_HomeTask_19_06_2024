using AutoMapper;
using FiorelloAPI.Data;
using FiorelloAPI.DTOs.Sliders;
using FiorelloAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiorelloAPI.Controllers
{

    public class SliderController : BaseController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sliders = await _context.Sliders.ToListAsync();
            var sliderDtos = _mapper.Map<List<SliderDto>>(sliders);
            return Ok(sliders);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetSliderInfo()
        {
            var sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync(info => !info.SoftDeleted);
            if (sliderInfo == null) return NotFound("Slider info not found");

            return Ok(sliderInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SliderCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            List<string> fileNames = new List<string>();

            foreach (var formFile in request.Images)
            {
                if (formFile.Length > 0)
                {
                    string fileName = $"{Guid.NewGuid().ToString()}-{formFile.FileName}";
                    string filePath = Path.Combine(_env.WebRootPath, "img", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    fileNames.Add(fileName);
                }
            }

            var sliders = fileNames.Select(fileName => new Slider { Image = fileName });

            await _context.Sliders.AddRangeAsync(sliders);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Create), sliders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] SliderEditDto request)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            if (request.NewImage != null)
            {
                string newFileName = $"{Guid.NewGuid().ToString()}-{request.NewImage.FileName}";
                string newPath = Path.Combine(_env.WebRootPath, "img", newFileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }

                slider.Image = newFileName;
            }

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
                return NotFound();

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
