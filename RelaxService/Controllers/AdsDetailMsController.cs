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
    public class AdsDetailMsController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AdsDetailMsController(SpotDbContext context)
        {
            _context = context;
        }

        // POST: api/AdsDetailMs/Filter
        [HttpPost("Filter")]
        public async Task<IEnumerable<AdsDetailMDTO>> FilterAdsDetailM([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            return _context.AdsDetailM.Where(
                adsDM => adsDM.DataDate >= dateStart && adsDM.DataDate <= dateEnd)
                     .OrderByDescending(adsDM => adsDM.DataDate)
                     .Select(adsDM => new AdsDetailMDTO
                     {
                         AdsDId = adsDM.AdsDId,
                         adsId = adsDM.adsId,
                         AnalysisId = adsDM.AnalysisId,
                         ClickMemberId = adsDM.ClickMemberId,
                         AdsClick = adsDM.AdsClick,
                         LikeClick = adsDM.LikeClick,
                         Device = adsDM.Device,
                         UserIp = adsDM.UserIp,
                         DataDate = adsDM.DataDate,
                     });
        }
        
        // GET: api/AdsDetailMs
        [HttpGet]
        public async Task<IEnumerable<AdsDetailMDTO>> GetAdsDetailM()
        {
            return _context.AdsDetailM.Select(adsdm => new AdsDetailMDTO{ 
                AdsDId = adsdm.AdsDId,
                adsId = adsdm.adsId,
                AnalysisId = adsdm.AnalysisId,
                ClickMemberId = adsdm.ClickMemberId,
                Device = adsdm.Device,
                UserIp = adsdm.UserIp,
                AdsClick = adsdm.AdsClick,
                LikeClick = adsdm.LikeClick,
                DataDate = adsdm.DataDate,
            });
        }

        // GET: api/AdsDetailMs/5
        [HttpGet("{id}")]
        public async Task<AdsDetailMDTO> GetAdsDetailM(int id)
        {
            var adsDetailM = await _context.AdsDetailM.FindAsync(id);
            AdsDetailMDTO adsDMDTO = null;
            if (adsDetailM != null)
            {
                return adsDMDTO = new AdsDetailMDTO
                {
                    AdsDId = adsDetailM.AdsDId,
                    adsId = adsDetailM.adsId,
                    AnalysisId= adsDetailM.AnalysisId,
                    ClickMemberId= adsDetailM.ClickMemberId,
                    Device= adsDetailM.Device,
                    UserIp= adsDetailM.UserIp,
                    AdsClick = adsDetailM.AdsClick,
                    LikeClick= adsDetailM.LikeClick,
                    DataDate = adsDetailM.DataDate,
                };
            }

            return adsDMDTO;
        }

        // PUT: api/AdsDetailMs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAdsDetailM(int id, AdsDetailMDTO adsDMDTO)
        {
            if (id != adsDMDTO.AdsDId)
            {
                return "修改失敗: id不相符";
            }
            var adsDetailM = await _context.AdsDetailM.FindAsync(id);
            if (adsDetailM == null)
            {
                return $"修改失敗: {id}不存在";
            }
            adsDetailM.adsId = adsDMDTO.adsId;
            adsDetailM.AnalysisId = adsDMDTO.AnalysisId;
            adsDetailM.ClickMemberId = adsDMDTO.ClickMemberId;
            adsDetailM.Device = adsDMDTO.Device;
            adsDetailM.AdsClick = adsDMDTO.AdsClick;
            adsDetailM.LikeClick = adsDMDTO.LikeClick;
            adsDetailM.UserIp = adsDMDTO.UserIp;
            adsDetailM.DataDate = adsDMDTO.DataDate;
            _context.Entry(adsDetailM).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "修改失敗: 例外錯誤";
            }

            return $"{id} 修改成功";
        }

        // POST: api/AdsDetailMs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<AdsDetailM> PostAdsDetailM(AdsDetailMDTO adsDMDTO)
        {
            AdsDetailM adsDetailM = new AdsDetailM { 
                adsId = adsDMDTO.adsId,
                AnalysisId = adsDMDTO.AnalysisId,
                ClickMemberId = adsDMDTO.ClickMemberId,
                Device = adsDMDTO?.Device,
                UserIp = adsDMDTO?.UserIp,
                AdsClick = adsDMDTO.AdsClick,
                LikeClick = adsDMDTO.LikeClick,
                DataDate = adsDMDTO.DataDate,
            };
            _context.AdsDetailM.Add(adsDetailM);
            await _context.SaveChangesAsync();

            return adsDetailM;
        }

        // DELETE: api/AdsDetailMs/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAdsDetailM(int id)
        {
            var adsDetailM = await _context.AdsDetailM.FindAsync(id);
            if (adsDetailM == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.AdsDetailM.Remove(adsDetailM);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                return $"刪除失敗: {ex.Message}";
            }

            return $"{id} 刪除成功";
        }

        private bool AdsDetailMExists(int id)
        {
            return _context.AdsDetailM.Any(e => e.AdsDId == id);
        }
    }
}
