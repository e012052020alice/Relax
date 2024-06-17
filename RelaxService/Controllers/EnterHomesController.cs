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
    public class EnterHomesController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public EnterHomesController(SpotDbContext context)
        {
            _context = context;
        }

        // GET: api/EnterHomes
        [HttpGet]
        public async Task<IEnumerable<EnterHomeDTO>> GetEnterHome()
        {
            return  _context.EnterHome.Select(Ip => new EnterHomeDTO
            {
                EnterId = Ip.EnterId,
                memberId = Ip.memberId,
                EnterDevice = Ip.EnterDevice,
                PublicIp = Ip.PublicIp,
                EnterPage = Ip.EnterPage,
                EnterTime = Ip.EnterTime,
            });
        }

        // GET: api/EnterHomes/5
        [HttpGet("{id}")]
        public async Task<EnterHomeDTO> GetEnterHome(int id)
        {
            var enterHome = await _context.EnterHome.FindAsync(id);
            EnterHomeDTO eDTO = null;

            if (enterHome != null)
            {
                return eDTO = new EnterHomeDTO
                {
                    EnterId = enterHome.EnterId,
                    memberId = enterHome.memberId,
                    EnterDevice = enterHome.EnterDevice,
                    PublicIp = enterHome.PublicIp,
                    EnterPage= enterHome.EnterPage,
                    EnterTime = enterHome.EnterTime,
                };
            }

            return eDTO;
        }

        // PUT: api/EnterHomes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutEnterHome(int id, EnterHomeDTO eDTO)
        {
            if (id != eDTO.EnterId)
            {
                return "修改失敗: id不相符";
            }
            EnterHome enterHome = await _context.EnterHome.FindAsync(id);
            if (enterHome == null)
            {
                return "修改失敗: id不存在";
            }

            enterHome.EnterDevice = eDTO.EnterDevice;
            enterHome.PublicIp = eDTO.PublicIp;
            enterHome.EnterTime = eDTO.EnterTime;
            _context.Entry(enterHome).State = EntityState.Modified;

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

        // POST: api/EnterHomes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<EnterHome> PostEnterHome(EnterHomeDTO eDTO)
        {
            EnterHome enterHome = new EnterHome
            {
                memberId = eDTO.memberId,
                EnterDevice = eDTO.EnterDevice,
                PublicIp = eDTO.PublicIp,
                EnterPage = eDTO.EnterPage,
                EnterTime = eDTO.EnterTime,
            };
            _context.EnterHome.Add(enterHome);
            await _context.SaveChangesAsync();

            return enterHome;
        }

        // DELETE: api/EnterHomes/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteEnterHome(int id)
        {
            var enterHome = await _context.EnterHome.FindAsync(id);
            if (enterHome == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.EnterHome.Remove(enterHome);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return $"刪除失敗: {ex.Message}";
            }

            return $"刪除成功: {id}已刪除";
        }

        private bool EnterHomeExists(int id)
        {
            return _context.EnterHome.Any(e => e.EnterId == id);
        }
    }
}
