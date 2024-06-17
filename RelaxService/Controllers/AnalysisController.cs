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
    public class AnalysisController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AnalysisController(SpotDbContext context)
        {
            _context = context;
        }
        // POST: api/Analysis/Filter

        [HttpPost("Filter")]
        public async Task<IEnumerable<AnalysisDTO>> FilterAnalysis([FromBody] DateRangeRequest request)
        {
            var dateStart = request.DateStart;
            var dateEnd = request.DateEnd;
            return _context.Analysis.Where(
                aDM => aDM.DataDate >= dateStart && aDM.DataDate <= dateEnd)
                     .OrderByDescending(aDM => aDM.DataDate)
                     .Select(aDM => new AnalysisDTO
                     {
                         AnalysisId = aDM.AnalysisId,
                         TotalUser = aDM.TotalUser,
                         TotalEnter = aDM.TotalEnter,
                         TotalSearch = aDM.TotalSearch,
                         TotalAds = aDM.TotalAds,
                         TotalLike = aDM.TotalLike,
                         DataDate = aDM.DataDate,
                     });
        }
        // GET: api/Analyses
        [HttpGet]
        public async Task<IEnumerable<AnalysisDTO>> GetAnalysis()
        {
            return _context.Analysis.Select(analysis => new AnalysisDTO {
                AnalysisId = analysis.AnalysisId,
                TotalUser = analysis.TotalUser,
                TotalEnter = analysis.TotalEnter,
                EnterDesktop = analysis.EnterDesktop,
                EnterMobile = analysis.EnterMobile,
                TotalSearch = analysis.TotalSearch,
                SearchDesktop = analysis.SearchDesktop,
                SearchMobile = analysis.SearchMobile,
                TotalAds = analysis.TotalAds,
                AdsDesktop = analysis.AdsDesktop,
                AdsMobile = analysis.AdsMobile,
                TotalLike = analysis.TotalLike,
                LikeDesktop = analysis.LikeDesktop,
                LikeMobile = analysis.LikeMobile,
                DataDate = analysis.DataDate,
            });
        }

        // GET: api/Analyses/5
        [HttpGet("{id}")]
        public async Task<AnalysisDTO> GetAnalysis(int id)
        {
            var analysis = await _context.Analysis.FindAsync(id);
            AnalysisDTO anDTO = null;
            if (analysis != null)
            {
                return anDTO = new AnalysisDTO
                {
                    AnalysisId = analysis.AnalysisId,
                    TotalUser = analysis.TotalUser,
                    TotalEnter = analysis?.TotalEnter,
                    EnterMobile = analysis?.EnterMobile,
                    EnterDesktop = analysis?.EnterDesktop,
                    TotalSearch = analysis?.TotalSearch,
                    SearchDesktop = analysis?.SearchDesktop,
                    SearchMobile = analysis?.SearchMobile,
                    TotalAds = analysis?.TotalAds,
                    AdsMobile = analysis?.AdsMobile,
                    AdsDesktop = analysis?.AdsDesktop,
                    TotalLike = analysis?.TotalLike,
                    LikeMobile = analysis?.LikeMobile,
                    LikeDesktop = analysis?.LikeDesktop,
                    DataDate = analysis.DataDate,
                };
            }

            return anDTO;
        }

        // PUT: api/Analyses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAnalysis(int id, AnalysisDTO anDTO)
        {
            if (id != anDTO.AnalysisId)
            {
                return "修改失敗: id不相符";
            }
            var analysis = await _context.Analysis.FindAsync(id);
            if (analysis ==null)
            {
                return $"修改失敗: {id}不存在";
            }
            analysis.TotalUser = anDTO.TotalUser;
            analysis.TotalEnter = anDTO?.TotalEnter;
            analysis.EnterDesktop = anDTO?.EnterDesktop;
            analysis.EnterMobile = anDTO?.EnterMobile;
            analysis.TotalSearch = anDTO?.TotalSearch;
            analysis.SearchDesktop = anDTO?.SearchDesktop;
            analysis.SearchMobile = anDTO?.SearchMobile;
            analysis.TotalAds = anDTO?.TotalAds;
            analysis.AdsMobile = anDTO?.AdsMobile;
            analysis.AdsDesktop = anDTO?.AdsDesktop;
            analysis.TotalLike = anDTO?.TotalLike;
            analysis.LikeMobile = anDTO?.LikeMobile;
            analysis.LikeDesktop = anDTO?.LikeDesktop;
            analysis.DataDate = anDTO.DataDate;
            _context.Entry(analysis).State = EntityState.Modified;

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

        // POST: api/Analyses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Analysis> PostAnalysis(AnalysisDTO anDTO)
        {//測試API時出現錯誤
            Analysis analysis = new Analysis {
                TotalUser = anDTO.TotalUser,
                TotalEnter = anDTO?.TotalEnter,
                EnterDesktop = anDTO?.EnterDesktop,
                EnterMobile = anDTO?.EnterMobile,
                TotalSearch = anDTO?.TotalSearch,
                SearchDesktop = anDTO?.SearchDesktop,
                SearchMobile = anDTO?.SearchMobile,
                TotalAds = anDTO?.TotalAds,
                AdsDesktop = anDTO?.AdsDesktop,
                AdsMobile = anDTO?.AdsMobile,
                TotalLike = anDTO?.TotalLike,
                LikeDesktop = anDTO?.LikeDesktop,
                LikeMobile = anDTO?.LikeMobile,
                DataDate = anDTO.DataDate,
            };
            _context.Analysis.Add(analysis);
            await _context.SaveChangesAsync();

            return analysis;
        }

        // DELETE: api/Analyses/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAnalysis(int id)
        {
            var analysis = await _context.Analysis.FindAsync(id);
            if (analysis == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.Analysis.Remove(analysis);

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

        private bool AnalysisExists(int id)
        {
            return _context.Analysis.Any(e => e.AnalysisId == id);
        }
    }
}
