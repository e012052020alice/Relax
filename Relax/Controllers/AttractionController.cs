using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relax.Data;
using Relax.DTO;
using Relax.Models;

namespace Relax.Controllers
{
    [EnableCors("AllowMy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController : ControllerBase
    {
        private readonly RelaxDbContext _context;

        public AttractionController(RelaxDbContext context)
        {
            _context = context;
        }
        [HttpPost("FilterNoOk")]
        public async Task<IEnumerable<AttractionDTO>> FilterAttractions(AttractionDTO aDTO)
        {
            //return null; //確認說明文件成功產生
            return _context.Attractions.Where(
                a => (a.AttractionId == aDTO.AttractionId ||
                     a.memberId == aDTO.memberId ||
                     a.AttractionName.Contains(aDTO.AttractionName) ||
                     a.Description.Contains(aDTO.Description) ||
                     a.Tag.Contains(aDTO.Tag)) &&
                     a.IsApproved == false)
                     .Select(a => new AttractionDTO
                     {
                         AttractionId = a.AttractionId,
                         memberId = a.memberId,
                         AttractionName = a.AttractionName,
                         Description = a.Description,
                         Tag = aDTO.Tag,
                     });
        }

        [HttpPut("{id}")]
        public async Task<string> PutAttractions(int id, AttractionDTO aDTO)
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
    }
}
