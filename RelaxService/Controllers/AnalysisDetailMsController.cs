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
    public class AnalysisDetailMsController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AnalysisDetailMsController(SpotDbContext context)
        {
            _context = context;
        }
        // POST: api/AnalysisDetailMs/Filter

        [HttpPost("Filter")]
        public async Task<IEnumerable<AnalysisDetailMDTO>> FilterAnalysisDetailM([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            return _context.AnalysisDetailM.Where(
                aDM => aDM.DataTime >= dateStart && aDM.DataTime <= dateEnd)
                     .OrderByDescending(aDM => aDM.DataTime)
                     .Select(aDM => new AnalysisDetailMDTO
                     {
                         AnalysisDId = aDM.AnalysisDId,
                         analysisId = aDM.analysisId,
                         memberId = aDM.memberId,
                         SearchClicks = aDM.SearchClicks,
                         AdsClicks = aDM.AdsClicks,
                         LikeClicks = aDM.LikeClicks,
                         Device = aDM.Device,
                         DataTime = aDM.DataTime,
                     });
        }
        // GET: api/AnalysisDetailMs
        [HttpGet]
        public async Task<IEnumerable<AnalysisDetailMDTO>> GetAnalysisDetailM()
        {
            return _context.AnalysisDetailM.Select(aDM => new AnalysisDetailMDTO { 
                AnalysisDId = aDM.AnalysisDId,
                analysisId = aDM.analysisId,
                memberId = aDM.memberId,
                SearchClicks = aDM.SearchClicks,
                AdsClicks = aDM.AdsClicks,
                LikeClicks = aDM.LikeClicks,
                DataTime = aDM.DataTime,
                Device = aDM.Device,
            });
        }

        // GET: api/AnalysisDetailMs/5
        [HttpGet("{id}")]
        public async Task<AnalysisDetailMDTO> GetAnalysisDetailM(int id)
        {
            var analysisDetailM = await _context.AnalysisDetailM.FindAsync(id);
            AnalysisDetailMDTO aDMDTO = null;
            if (analysisDetailM != null)
            {
                return aDMDTO = new AnalysisDetailMDTO {
                    AnalysisDId = analysisDetailM.AnalysisDId,
                    analysisId = analysisDetailM.analysisId,
                    memberId = analysisDetailM.memberId,
                    SearchClicks = analysisDetailM.SearchClicks,
                    AdsClicks = analysisDetailM.AdsClicks,
                    LikeClicks = analysisDetailM.LikeClicks,
                    Device = analysisDetailM.Device,
                    DataTime = analysisDetailM.DataTime,
                };
            }

            return aDMDTO;
        }

        // PUT: api/AnalysisDetailMs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAnalysisDetailM(int id, AnalysisDetailMDTO aDMDTO)
        {
            if (id != aDMDTO.AnalysisDId)
            {
                return "修改失敗: id不相符";
            }
            AnalysisDetailM analysisDetailM = await _context.AnalysisDetailM.FindAsync(id);
            analysisDetailM.analysisId = aDMDTO.analysisId;
            analysisDetailM.memberId = aDMDTO.memberId;
            analysisDetailM.SearchClicks = aDMDTO.SearchClicks;
            analysisDetailM.AdsClicks = aDMDTO.AdsClicks;
            analysisDetailM.LikeClicks = aDMDTO.LikeClicks;
            analysisDetailM.Device = aDMDTO.Device;
            analysisDetailM.DataTime = aDMDTO.DataTime;
            _context.Entry(analysisDetailM).State = EntityState.Modified;

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

        // POST: api/AnalysisDetailMs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<AnalysisDetailM> PostAnalysisDetailM(AnalysisDetailMDTO aDMDTO)
        {
            AnalysisDetailM analysisDetailM = new AnalysisDetailM {
                analysisId = aDMDTO.analysisId,
                memberId = aDMDTO.memberId,
                SearchClicks = aDMDTO.SearchClicks,
                AdsClicks = aDMDTO.AdsClicks,
                LikeClicks = aDMDTO.LikeClicks,
                Device = aDMDTO.Device,
                DataTime = aDMDTO.DataTime,
            };
            _context.AnalysisDetailM.Add(analysisDetailM);
            await _context.SaveChangesAsync();

            return analysisDetailM;
        }

        // DELETE: api/AnalysisDetailMs/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAnalysisDetailM(int id)
        {
            var analysisDetailM = await _context.AnalysisDetailM.FindAsync(id);
            if (analysisDetailM == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.AnalysisDetailM.Remove(analysisDetailM);
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

        private bool AnalysisDetailMExists(int id)
        {
            return _context.AnalysisDetailM.Any(e => e.AnalysisDId == id);
        }
    }
}
