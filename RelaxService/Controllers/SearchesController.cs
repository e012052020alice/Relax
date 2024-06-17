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
    public class SearchesController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public SearchesController(SpotDbContext context)
        {
            _context = context;
        }

        // GET: api/Searches
        [HttpGet]
        public async Task<IEnumerable<SearchDTO>> GetSearch()
        {
            return _context.Search.Select(s => new SearchDTO{ 
                SearchId = s.SearchId,
                memberId = s.memberId,
                SearchIp = s.SearchIp,
                SearchTime = s.SearchTime,
                Device = s.Device,
                Age = s.Age,
                Gender = s.Gender,
            });
        }

        // GET: api/Searches/5
        [HttpGet("{id}")]
        public async Task<SearchDTO> GetSearch(int id)
        {
            var search = await _context.Search.FindAsync(id);
            SearchDTO sDTO = null;
            if (search != null)
            {
                return sDTO = new SearchDTO
                {
                    SearchId = search.SearchId,
                    memberId = search.memberId,
                    SearchIp = search.SearchIp,
                    SearchTime= search.SearchTime,
                    Device= search.Device,
                    Age= search.Age,
                    Gender = search.Gender,
                };
            }

            return sDTO;
        }

        // PUT: api/Searches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutSearch(int id, SearchDTO sDTO)
        {
            if (id != sDTO.SearchId)
            {
                return "修改失敗: id不相符";
            }
            Search search = await _context.Search.FindAsync(id);
            if (search == null)
            {
                return $"修改失敗: {id}不存在";
            }
            search.SearchIp = sDTO.SearchIp;
            search.SearchTime = sDTO.SearchTime;
            search.Device = sDTO.Device;
            search.Age = sDTO.Age;
            search.Gender = sDTO.Gender;
            _context.Entry(search).State = EntityState.Modified;

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

        // POST: api/Searches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<Search> PostSearch(SearchDTO sDTO)
        {
            Search search = new Search
            {
                memberId= sDTO.memberId,
                SearchIp= sDTO.SearchIp,
                SearchTime= sDTO.SearchTime,
                Device = sDTO.Device,
                Age = sDTO.Age,
                Gender = sDTO.Gender,
            };
            _context.Search.Add(search);
            await _context.SaveChangesAsync();

            return search;
        }

        // DELETE: api/Searches/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteSearch(int id)
        {
            var search = await _context.Search.FindAsync(id);
            if (search == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.Search.Remove(search);
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

        private bool SearchExists(int id)
        {
            return _context.Search.Any(e => e.SearchId == id);
        }
    }
}
