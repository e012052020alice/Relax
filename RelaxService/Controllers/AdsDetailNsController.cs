using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelaxService.Data;
using RelaxService.DTOs.Analysis;
using RelaxService.Models;

namespace RelaxService.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsDetailNsController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AdsDetailNsController(SpotDbContext context)
        {
            _context = context;
        }
        // POST: api/AdsDetailNs/Filter
        [HttpPost("Filter")]
        public async Task<IEnumerable<AdsDetailNDTO>> FilterAdsDetailN([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            return _context.AdsDetailN.Where(
                adsDM => adsDM.DataDate >= dateStart && adsDM.DataDate <= dateEnd)
                     .OrderByDescending(adsDM => adsDM.DataDate)
                     .Select(adsDM => new AdsDetailNDTO
                     {
                         AdsDId = adsDM.AdsDId,
                         adsId = adsDM.adsId,
                         AnalysisId = adsDM.AnalysisId,
                         AdsClick = adsDM.AdsClick,
                         LikeClick = adsDM.LikeClick,
                         Device = adsDM.Device,
                         UserIp = adsDM.UserIp,
                         DataDate = adsDM.DataDate,
                     });
        }
        // GET: api/AdsDetailNs
        [HttpGet]
        public async Task<IEnumerable<AdsDetailNDTO>> GetAdsDetailN()
        {
            return _context.AdsDetailN.Select(adsDN => new AdsDetailNDTO
            {
                AdsDId = adsDN.AdsDId,
                adsId = adsDN.adsId,
                AnalysisId = adsDN.AnalysisId,
                UserIp = adsDN.UserIp,
                Device = adsDN.Device,
                AdsClick = adsDN.AdsClick,
                LikeClick = adsDN.LikeClick,
                DataDate = adsDN.DataDate,
            });
        }

        // GET: api/AdsDetailNs/5
        [HttpGet("{id}")]
        public async Task<AdsDetailNDTO> GetAdsDetailN(int id)
        {
            var adsDetailN = await _context.AdsDetailN.FindAsync(id);
            AdsDetailNDTO adsDNDTO = null;
            if (adsDetailN != null)
            {
                return adsDNDTO = new AdsDetailNDTO {
                    AdsDId = adsDetailN.AdsDId,
                    adsId = adsDetailN.adsId,
                    AnalysisId = adsDetailN.AnalysisId,
                    UserIp = adsDetailN.UserIp,
                    Device = adsDetailN.Device,
                    AdsClick= adsDetailN.AdsClick,
                    LikeClick= adsDetailN.LikeClick,
                    DataDate = adsDetailN.DataDate,
                };
            }

            return adsDNDTO;
        }

        // PUT: api/AdsDetailNs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAdsDetailN(int id, AdsDetailNDTO adsDNDTO)
        {
            if (id != adsDNDTO.AdsDId)
            {
                return "修改失敗: id不相符";
            }
            var adsDetailN = _context.AdsDetailN.FindAsync(id);
            if (adsDetailN == null)
            {
                return $"修改失敗: {id}不存在";
            }
            _context.Entry(adsDetailN).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "修改失敗:例外錯誤";
            }

            return $"{id} 修改成功";
        }

        // POST: api/AdsDetailNs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<AdsDetailN> PostAdsDetailN(AdsDetailNDTO adsDNDTO)
        {
            AdsDetailN adsDetailN = new AdsDetailN
            {
                adsId = adsDNDTO.adsId,
                AnalysisId = adsDNDTO.AnalysisId,
                AdsClick = adsDNDTO.AdsClick,
                LikeClick = adsDNDTO.AdsClick,
                Device = adsDNDTO.Device,
                UserIp = adsDNDTO.UserIp,
                DataDate = adsDNDTO.DataDate,
            };
            _context.AdsDetailN.Add(adsDetailN);
            await _context.SaveChangesAsync();

            return adsDetailN;
        }

        // DELETE: api/AdsDetailNs/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAdsDetailN(int id)
        {
            var adsDetailN = await _context.AdsDetailN.FindAsync(id);
            if (adsDetailN == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.AdsDetailN.Remove(adsDetailN);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return $"刪除失敗: {ex.Message}";
            }

            return $"{id} 刪除成功";
        }

        private bool AdsDetailNExists(int id)
        {
            return _context.AdsDetailN.Any(e => e.AdsDId == id);
        }
    }
}
