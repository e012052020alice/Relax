using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelaxService.Data;
using RelaxService.DTOs;
using RelaxService.DTOs.Analysis;
using RelaxService.Models;

namespace RelaxService.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AdsController(SpotDbContext context)
        {
            _context = context;
        }
        // GET: api/Ads/Fliter
        [HttpPost("Fliter")]
        public async Task<IEnumerable<AdsDTO>> PostAdsInterval([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            return _context.Advertisements
                .Where(ads => (ads.StartTime <= dateStart && ads.EndTime >= dateStart && ads.EndTime <= dateEnd) || (ads.StartTime <= dateStart && ads.EndTime >= dateEnd) ||
                    (ads.StartTime >= dateStart && ads.StartTime <= dateEnd && ads.EndTime >= dateEnd))
                .Select(ads => new AdsDTO
                {
                    AdId = ads.AdId,
                    tripId = ads.tripId,
                    Clicks = ads.Clicks,
                    StartTime = ads.StartTime,
                    EndTime = ads.EndTime,
                    AdLoved = ads.AdLoved,
                    AdUrl = ads.AdUrl,
                    Image = ads.Image,
                });
        }
        // GET: api/Ads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Advertisements>>> GetAdvertisements()
        {
            return await _context.Advertisements.ToListAsync();
        }

        // GET: api/Ads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Advertisements>> GetAdvertisements(int id)
        {
            var advertisements = await _context.Advertisements.FindAsync(id);

            if (advertisements == null)
            {
                return NotFound();
            }

            return advertisements;
        }

        // PUT: api/Ads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdvertisements(int id, Advertisements advertisements)
        {
            if (id != advertisements.AdId)
            {
                return BadRequest();
            }

            _context.Entry(advertisements).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisementsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Ads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Advertisements>> PostAdvertisements(Advertisements advertisements)
        {
            _context.Advertisements.Add(advertisements);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdvertisements", new { id = advertisements.AdId }, advertisements);
        }

        // DELETE: api/Ads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvertisements(int id)
        {
            var advertisements = await _context.Advertisements.FindAsync(id);
            if (advertisements == null)
            {
                return NotFound();
            }

            _context.Advertisements.Remove(advertisements);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdvertisementsExists(int id)
        {
            return _context.Advertisements.Any(e => e.AdId == id);
        }
    }
}
