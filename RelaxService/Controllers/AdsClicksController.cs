using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RelaxService.Data;
using RelaxService.DTOs;
using RelaxService.Models;

namespace RelaxService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsClicksController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AdsClicksController(SpotDbContext context)
        {
            _context = context;
        }

        // GET: api/AdsClicks
        [HttpGet]
        public async Task<IEnumerable<AdsClickDTO>> GetAdsClick()
        {
            return _context.AdsClick.Select(click=>new AdsClickDTO
            {
                AdsClickId = click.AdsClickId,
                memberId = click.memberId,
                AdsId = click.AdsId,
                Device = click.Device,
                Age=click.Age,
                Gender=click.Gender,
                UserIp = click.UserIp,
                AdsClickTime = click.AdsClickTime,
            });
        }

        // GET: api/AdsClicks/5
        [HttpGet("{id}")]
        public async Task<AdsClickDTO> GetAdsClick(int id)
        {
            var adsClick = await _context.AdsClick.FindAsync(id);
            AdsClickDTO clickDTO = null;
            if (adsClick != null)
            {
                return clickDTO = new AdsClickDTO
                {
                    AdsClickId = adsClick.AdsClickId,
                    memberId = adsClick.memberId,
                    AdsId = adsClick.AdsId,
                    Device = adsClick.Device,
                    Age = adsClick.Age,
                    Gender = adsClick.Gender,
                    UserIp = adsClick.UserIp,
                    AdsClickTime= adsClick.AdsClickTime,
                };
            }

            return clickDTO;
        }

        // PUT: api/AdsClicks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAdsClick(int id, AdsClickDTO clickDTO)
        {
            if (id != clickDTO.AdsClickId)
            {
                return "修改失敗: id不相符";
            }
            AdsClick adsClick = await _context.AdsClick.FindAsync(id);
            if (adsClick == null)
            {
                return $"修改失敗: {id}不存在";
            }
            adsClick.memberId = clickDTO.memberId;
            adsClick.AdsId = clickDTO.AdsId;
            adsClick.Device = clickDTO.Device;
            adsClick.UserIp = clickDTO.UserIp;
            adsClick.Age = clickDTO.Age;
            adsClick.Gender = clickDTO.Gender;
            adsClick.AdsClickTime = clickDTO.AdsClickTime;
            _context.Entry(adsClick).State = EntityState.Modified;

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

        // POST: api/AdsClicks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<AdsClick> PostAdsClick(AdsClickDTO clickDTO)
        {
            AdsClick adsClick = new AdsClick
            {
                memberId = clickDTO.memberId,
                AdsId = clickDTO.AdsId,
                Device = clickDTO.Device,
                UserIp = clickDTO.UserIp,
                Age = clickDTO.Age,
                Gender = clickDTO.Gender,
                AdsClickTime = clickDTO.AdsClickTime,
            };
            _context.AdsClick.Add(adsClick);
            await _context.SaveChangesAsync();

            return adsClick;
        }

        // DELETE: api/AdsClicks/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAdsClick(int id)
        {
            var adsClick = await _context.AdsClick.FindAsync(id);
            if (adsClick == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.AdsClick.Remove(adsClick);

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

        private bool AdsClickExists(int id)
        {
            return _context.AdsClick.Any(e => e.AdsClickId == id);
        }
    }
}
