using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
    public class AdsLikesController : ControllerBase
    {
        private readonly SpotDbContext _context;

        public AdsLikesController(SpotDbContext context)
        {
            _context = context;
        }

        // GET: api/AdsLikes
        [HttpGet]
        public async Task<IEnumerable<AdsLike>> GetAdsLike()
        {
            return _context.AdsLike.Select(like=> new AdsLike
            {
                AdsLikeId = like.AdsLikeId,
                memberId = like.memberId,
                AdsId = like.AdsId,
                Device = like.Device,
                Age = like.Age,
                Gender = like.Gender,
                UserIp = like.UserIp,
                AdsLikeTime = like.AdsLikeTime
            });
        }

        // GET: api/AdsLikes/5
        [HttpGet("{id}")]
        public async Task<AdsLikeDTO> GetAdsLike(int id)
        {
            var adsLike = await _context.AdsLike.FindAsync(id);
            AdsLikeDTO likeDTO = null;
            if (adsLike != null)
            {
                return likeDTO = new AdsLikeDTO {
                    AdsLikeId = adsLike.AdsLikeId,
                    memberId = adsLike.memberId,
                    AdsId = adsLike.AdsId,
                    Device = adsLike.Device,
                    Age= adsLike.Age,
                    Gender = adsLike.Gender,
                    UserIp = adsLike.UserIp,
                    AdsLikeTime = adsLike.AdsLikeTime
                };
            }

            return likeDTO;
        }

        // PUT: api/AdsLikes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<string> PutAdsLike(int id, AdsLikeDTO likeDTO)
        {
            if (id != likeDTO.AdsLikeId)
            {
                return "修改失敗: id不相符";
            }
            AdsLike like = await _context.AdsLike.FindAsync(id);
            if (like == null) {
                return $"修改失敗: {id}不存在";
            }
            like.memberId = likeDTO.memberId;
            like.AdsId = likeDTO.AdsId;
            like.UserIp = likeDTO.UserIp;
            like.Age = likeDTO.Age;
            like.Gender = likeDTO.Gender;
            like.AdsLikeTime = likeDTO.AdsLikeTime;
            _context.Entry(like).State = EntityState.Modified;


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

        // POST: api/AdsLikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<AdsLike> PostAdsLike(AdsLikeDTO likeDTO)
        {
            AdsLike like = new AdsLike { 
                memberId = likeDTO.memberId,
                AdsId = likeDTO.AdsId,
                Device = likeDTO.Device,
                Age = likeDTO.Age,
                Gender = likeDTO.Gender,
                UserIp = likeDTO.UserIp,
                AdsLikeTime = likeDTO.AdsLikeTime,
            };
            _context.AdsLike.Add(like);
            await _context.SaveChangesAsync();

            return like;
        }

        // DELETE: api/AdsLikes/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteAdsLike(int id)
        {
            var adsLike = await _context.AdsLike.FindAsync(id);
            if (adsLike == null)
            {
                return $"刪除失敗: {id}不存在";
            }

            _context.AdsLike.Remove(adsLike);
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

        private bool AdsLikeExists(int id)
        {
            return _context.AdsLike.Any(e => e.AdsLikeId == id);
        }
    }
}
