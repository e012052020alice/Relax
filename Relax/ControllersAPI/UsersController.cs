using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using Newtonsoft.Json;
using Relax.Data;
using Relax.DTO;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Cryptography;
using System.Data;
using System;
using Humanizer;


namespace Relax.ControllersAPI
{
    [EnableCors("AllowMy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ManagePasswordDTO> _logger;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<ManagePasswordDTO> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return _context.Users.Select(e => new UserDTO
            {
                Id = e.Id,
                UserName = e.UserName,
                Name = e.Name,
                Gender = e.Gender,
                BirthDay = e.BirthDay,
                Email = e.Email,
                //Points = e.Points,
            });

        }

        // GET: api/Users/GetManage
        [HttpGet("GetManage")]
        public async Task<IEnumerable<ManageDTO>> GetManage()
        {
            var _Id = _userManager.GetUserId(User);
            var userManager = from users in _context.Users
                              where users.Id == _Id
                              select new ManageDTO
                              {
                                  Id = users.Id,
                                  UserName = users.UserName,
                                  Name = users.Name,
                                  Email = users.Email,
                                  Gender = users.Gender,
                                  BirthDay = users.BirthDay,
                              };


            return userManager;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<UserDTO> GetUsers(string id)
        {
            var users = await _context.Users.FindAsync(id);
            UserDTO userDTO = null;
            if (users != null)
            {
                userDTO = new UserDTO
                {
                    Id = users.Id,
                    UserName = users.UserName,
                    Name = users.Name,
                    Gender = users.Gender,
                    BirthDay = users.BirthDay,
                    Email = users.Email,
                };
            }
            return userDTO;
        }
        // POST: api/Users/Fliter
        [HttpPost("Fliter")]
        public async Task<IEnumerable<UserDTO>> FliterUser(UserDTO userDTO)
        {
            var users = await _context.Users.ToListAsync();
            var userRoles = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            var userlist = from user in users
                           join userRole in userRoles on user.Id equals userRole.UserId into userRoleGroup
                           from userRole in userRoleGroup.DefaultIfEmpty()
                           join role in roles on userRole?.RoleId equals role.Id into roleGroup
                           from role in roleGroup.DefaultIfEmpty()
                           select new UserDTO
                           {
                               Id = user.Id,
                               UserName = user.UserName,
                               RoleName = role?.Name,
                               Name = user.Name,
                               Email = user.Email,
                               Gender = user.Gender,
                               BirthDay = user.BirthDay,
                           };
            return userlist.Where(
                dto => dto.Id.Contains(userDTO.Id) || dto.UserName.Contains(userDTO.UserName) ||
                      dto.Name.Contains(userDTO.Name) || dto.Gender.Contains(userDTO.Gender) ||
                      dto.Email.Contains(userDTO.Email))
                .Select(dto => new UserDTO
                {
                    Id = dto.Id,
                    UserName = dto.UserName,
                    RoleName = dto?.RoleName,
                    Name = dto.Name,
                    Email = dto.Email,
                    Gender = dto.Gender,
                    BirthDay = dto.BirthDay,
                });
        }

        // POST: api/Users/Role
        [HttpPost("Role")]
        public async Task<IEnumerable<RoleDTO>> GetRole()
        {

            var roles = await _context.Roles.ToListAsync();

            var userlist = from role in _context.Roles
                           select new RoleDTO
                           {
                               RoleId = role.Id,
                               RoleName = role.Name,
                           };
            return userlist;
        }

        // POST: api/Users/PutName/5
        [HttpPost("PostName")]
        public async Task<string> PostName(ManageNameDTO manageNameDTO)
        {
            var users = await _userManager.GetUserAsync(User);
            if (users == null)
            {
                return "查無使用者";
            }
            else if (users.Name == manageNameDTO.Name)
            {
                return "暱稱未更變";
            }
            var userId = await _userManager.GetUserIdAsync(users);
            if (userId != manageNameDTO.Id)
            {
                return "修改失敗";
            }
            users.Name = manageNameDTO.Name;
            _context.Entry(users).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(userId))
                {
                    return "修改失敗";
                }
                else
                {
                    throw;
                }
            }
            return "修改成功";
        }

        //POST: api/Users/PostEmail
        [HttpPost("PostEmail")]
        public async Task<string> PostEmail(ManageEmailDTO manageEmailDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return "查無使用者";
            }

            var userId = await _userManager.GetUserIdAsync(user);
            if (userId != manageEmailDTO.Id)
            {
                return "修改失敗";
            }

            if (!manageEmailDTO.IsValidEmail())
            {
                return "信箱格式錯誤";
            }
            var email = await _userManager.GetEmailAsync(user);
            var emailList = _context.Users.Select(x => x.Email).ToList();
            foreach (var item in emailList)
            {
                if (item == manageEmailDTO.NewEmail && item != email)
                {
                    return "信箱已註冊";
                }
            }

            if (manageEmailDTO.NewEmail != email)
            {
                await _userManager.SetEmailAsync(user, manageEmailDTO.NewEmail);

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, code);

                await _userManager.UpdateAsync(user);


                return "您的電子郵件已變更。";
            }

            return "您的電子郵件未變更。";
        }

        //POST: api/Users/PostPassword
        [HttpPost("PostPassword")]
        public async Task<string> PostPassword(ManagePasswordDTO managePasswordDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return "查無使用者";
            }

            if (!managePasswordDTO.IsValidPassword())
            {
                return "密碼格式錯誤";
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, managePasswordDTO.OldPassword, managePasswordDTO.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return "修改失敗";
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            return "修改成功";
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<string> PutUsers(string id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return "修改失敗";
            }

            var emailList = _context.Users.Select(x => new { Email = x.Email, Id = x.Id }).ToList();
            foreach (var item in emailList)
            {
                if (item.Email == userDTO.Email && item.Id != userDTO.Id)
                {
                    return "信箱已註冊";
                }
            }
            var idList = _context.Users.Select(x => new { UserName = x.UserName, Id = x.Id }).ToList();
            foreach (var item in idList)
            {
                if (item.UserName == userDTO.UserName && item.Id != userDTO.Id)
                {
                    return "會員帳號已註冊";
                }
            }
            var user = await _context.Users.FindAsync(id);

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.AddToRoleAsync(user, userDTO.RoleName);

            user.UserName = userDTO.UserName;
            user.Name = userDTO.Name;
            user.Gender = userDTO.Gender;
            user.BirthDay = userDTO.BirthDay;
            user.Email = userDTO.Email;

            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id))
                {
                    return "修改失敗";
                }
                else
                {
                    throw;
                }
            }
            return "修改成功";
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<string> DeleteUsers(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return "刪除失敗";
            }

            _context.Users.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return "刪除關聯紀錄失敗";
            }

            return "刪除成功";
        }

        private bool UsersExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
