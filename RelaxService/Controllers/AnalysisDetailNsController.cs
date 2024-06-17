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
    public class AnalysisDetailNsController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AnalysisDetailNsController(SpotDbContext context)
        {
            _context = context;
        }

        [HttpPost("Filter")]
        public async Task<IEnumerable<AnalysisDetailNDTO>> FilterAnalysisDetailN([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            return _context.AnalysisDetailN.Where(
                aDN => aDN.DataTime >= dateStart && aDN.DataTime <= dateEnd)
                     .OrderByDescending(aDN => aDN.DataTime)
                     .Select(aDN => new AnalysisDetailNDTO
                     {
                         AnalysisDId = aDN.AnalysisDId,
                         analysisId = aDN.analysisId,
                         UserIp = aDN.UserIp,
                         SearchClicks = aDN.SearchClicks,
                         AdsClicks = aDN.AdsClicks,
                         LikeClicks = aDN.LikeClicks,
                         Device = aDN.Device,
                         DataTime = aDN.DataTime,
                     });
        }

        // GET: api/AnalysisDetailNs
        [HttpGet]
        public async Task<IEnumerable<AnalysisDetailNDTO>> GetAnalysisDetailN()
        {
            return _context.AnalysisDetailN.Select(aDN => new AnalysisDetailNDTO {
                AnalysisDId = aDN.AnalysisDId,
                analysisId = aDN.analysisId,
                UserIp = aDN.UserIp,
                AdsClicks = aDN.LikeClicks,
                SearchClicks = aDN.SearchClicks,
                LikeClicks = aDN.LikeClicks,
                Device = aDN.Device,
                DataTime = aDN.DataTime,
            });
        }

        // GET: api/AnalysisDetailNs/5
        [HttpGet("{id}")]
        public async Task<AnalysisDetailNDTO> GetAnalysisDetailN(int id)
        {
            var analysisDetailN = await _context.AnalysisDetailN.FindAsync(id);
            AnalysisDetailNDTO aDNDTO = null;
            if (analysisDetailN != null)
            {
                return aDNDTO = new AnalysisDetailNDTO { 
                    AnalysisDId = analysisDetailN.AnalysisDId,
                    analysisId = analysisDetailN.analysisId,
                    UserIp = analysisDetailN.UserIp,
                    SearchClicks = analysisDetailN.SearchClicks,
                    AdsClicks = analysisDetailN.LikeClicks,
                    LikeClicks = analysisDetailN.LikeClicks,
                    Device = analysisDetailN.Device,
                    DataTime = analysisDetailN.DataTime,
                };
            }

            return aDNDTO;
        }

        // PUT: api/AnalysisDetailNs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAnalysisDetailN(int id, AnalysisDetailNDTO aDNDTO)
        {
            if (id != aDNDTO.AnalysisDId)
            {
                return "修改失敗: id不相符";
            }
            AnalysisDetailN analysisDetailN = await _context.AnalysisDetailN.FindAsync(id);
            if (analysisDetailN == null)
            {
                return $"修改失敗: {id}不存在";
            }
            analysisDetailN.analysisId = aDNDTO.analysisId;
            analysisDetailN.UserIp = aDNDTO.UserIp;
            analysisDetailN.Device = aDNDTO.Device;
            analysisDetailN.SearchClicks = aDNDTO.SearchClicks;
            analysisDetailN.AdsClicks = aDNDTO.AdsClicks;
            analysisDetailN.LikeClicks = aDNDTO.LikeClicks;
            analysisDetailN.DataTime = aDNDTO.DataTime;
            _context.Entry(analysisDetailN).State = EntityState.Modified;

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

        // POST: api/AnalysisDetailNs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<AnalysisDetailN> PostAnalysisDetailN(AnalysisDetailNDTO aDNDTO)
        {
            AnalysisDetailN analysisDetailN = new AnalysisDetailN
            {
                analysisId = aDNDTO.analysisId,
                UserIp = aDNDTO.UserIp,
                Device = aDNDTO.Device,
                SearchClicks = aDNDTO.SearchClicks,
                AdsClicks = aDNDTO.AdsClicks,
                LikeClicks = aDNDTO.LikeClicks,
                DataTime = aDNDTO.DataTime,
            };
            _context.AnalysisDetailN.Add(analysisDetailN);
            await _context.SaveChangesAsync();

            return analysisDetailN;
        }

        // DELETE: api/AnalysisDetailNs/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAnalysisDetailN(int id)
        {
            var analysisDetailN = await _context.AnalysisDetailN.FindAsync(id);
            if (analysisDetailN == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.AnalysisDetailN.Remove(analysisDetailN);
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

        private bool AnalysisDetailNExists(int id)
        {
            return _context.AnalysisDetailN.Any(e => e.AnalysisDId == id);
        }
    }
}
