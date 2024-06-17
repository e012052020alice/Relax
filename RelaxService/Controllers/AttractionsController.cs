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
using RelaxService.Models;

namespace RelaxService.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionsController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AttractionsController(SpotDbContext context)
        {
            _context = context;
        }

        [HttpPost("FilterNoOk")]
        public async Task<IEnumerable<AttractionsDTO>> FilterAttractions(AttractionsDTO aDTO)
        {
            //return null; //確認說明文件成功產生
            return _context.Attractions.Where(
                a => (a.AttractionId == aDTO.AttractionId ||
                     a.memberId == aDTO.memberId ||
                     a.AttractionName.Contains(aDTO.AttractionName) ||
                     a.Description.Contains(aDTO.Description) ||
                     a.Tag.Contains(aDTO.Tag)) &&
                     a.IsApproved == false)
                     .Select(a => new AttractionsDTO
                     {
                         AttractionId = a.AttractionId,
                         memberId = a.memberId,
                         AttractionName = a.AttractionName,
                         Description = a.Description,
                         Tag = aDTO.Tag,
                     });
        }
        // GET: api/Attractions
        [HttpGet]
        public async Task<IEnumerable<AttractionsDTO>> GetAttractions()
        {
            return _context.Attractions.Select(spot => new AttractionsDTO
            {
                AttractionId = spot.AttractionId,
                AttractionName = spot.AttractionName,
                City = spot.City,
                Description = spot.Description,
                Tag = spot.Tag,
                IsApproved = spot.IsApproved,
            });
        }

        // GET: api/Attractions/5
        [HttpGet("{id}")]
        public async Task<AttractionsDTO> GetAttractions(int id)
        {
            var attractions = await _context.Attractions.FindAsync(id);
            AttractionsDTO aDTO = null;

            if (attractions != null)
            {
                aDTO = new AttractionsDTO
                {
                    AttractionId = attractions.AttractionId,
                    AttractionName = attractions.AttractionName,
                    City = attractions.City,
                    Description = attractions.Description,
                    Tag = attractions.Tag,
                    IsApproved = attractions.IsApproved,
                };
            }

            return aDTO;
        }

        // PUT: api/Attractions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAttractions(int id, AttractionsDTO aDTO)
        {
            if (id != aDTO.AttractionId)
            {
                return "修改失敗: id不相符";
            }
            Attractions attractions = await _context.Attractions.FindAsync(id);
            if (attractions == null)
            {
                return "修改失敗: id不存在";
            }
            attractions.AttractionId = aDTO.AttractionId;
            attractions.AttractionName = aDTO.AttractionName;
            attractions.City = aDTO.City;
            attractions.Description = aDTO.Description;
            attractions.Tag = aDTO.Tag;
            attractions.IsApproved = aDTO.IsApproved;

            _context.Entry(attractions).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "修改失敗:例外錯誤";
            }

            return "修改成功";
        }

        // POST: api/Attractions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Attractions> PostAttractions(AttractionsDTO aDTO)
        {
            Attractions attractions = new Attractions
            {
                AttractionName = aDTO.AttractionName,
                Description = aDTO.Description,
                City = aDTO.City,
                Tag = aDTO.Tag,
                IsApproved = aDTO.IsApproved,
            };
            _context.Attractions.Add(attractions);
            await _context.SaveChangesAsync();

            return attractions;
        }

        // DELETE: api/Attractions/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAttractions(int id)
        {
            var attractions = await _context.Attractions.FindAsync(id);
            if (attractions == null)
            {
                return "刪除失敗: id不存在";
            }

            _context.Attractions.Remove(attractions);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "刪除失敗: 關聯資料表";
            }

            return "刪除成功";
        }

        private bool AttractionsExists(int id)
        {
            return _context.Attractions.Any(e => e.AttractionId == id);
        }
    }
}
