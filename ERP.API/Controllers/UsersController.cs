using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Auth;
using ERP.API.Models;
using ERP.API.Models.Users;
using ERP.BL.Helpers;
using ERP.DAL;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using System.Security.Claims;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly IJWTManager _jwtManager;
        private readonly IUserRepository _userRepository;

        public UsersController(UserManager<SystemUser> userManager, IJWTManager jwtManager, IUserRepository userRepository)
        {
            this._userManager = userManager;
            this._jwtManager = jwtManager;
            this._userRepository = userRepository;
        }


        [Route("authenticate")]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateVM userData)
        {
            var user = await this._userManager.FindByNameAsync(userData.Username);
            if (user != null)
            {
                var isPasswordValid = await this._userManager.CheckPasswordAsync(user, userData.Password);
                if (isPasswordValid)
                {
                    var claims = new List<Claim>();
                    var userRoles = await this._userManager.GetRolesAsync(user);
                    if (userRoles != null)
                    {
                        userRoles.ToList().ForEach(r =>
                        {
                            claims.Add(new Claim(ClaimTypes.Role, r));
                        });
                    }

                    claims.Add(new Claim(ClaimTypes.Name, userData.Username));
                    claims.Add(new Claim(ClaimTypes.Email, userData.Username));
                    var token = this._jwtManager.Authenticate(claims);

                    var userEntity = await this._userRepository.Get(user.Id).Include(u => u.Roles).Include(u => u.Permissions).FirstOrDefaultAsync();

                    var userDetails = new UserGetVM
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Token = token.Token,
                        Permissions = userEntity.Permissions.Select(p => p.Name).ToList(),
                        Roles = new List<string>(userRoles?.ToArray() ?? new List<string>().ToArray()),
                    };


                    return Ok(new APIResponse<UserGetVM> 
                    { 
                        IsError = false, 
                        Message = "", 
                        data = userDetails 
                    });
                }
            }
            return Ok(new { IsError = true, Message = "Invalid username or password" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {

            var userEntity = UserRegistrationHelper.GetUserEntity(model.Email, model.FirstName, model.LastName, model.PhoneNumber, "Admin*123");

            userEntity.UserPermissions = model.Permissions.Select(p => new UserPermission { PermissionId = (int)Enum.Parse(typeof(PERMISSIONS), p) }).ToList();
            userEntity.UserRoles = new List<UserRole> { new UserRole { RoleId = (int)ROLES.SystemUser } };

            this._userRepository.Add(userEntity);
            await this._userRepository.SaveChanges();

            return Created("", new { IsError = false, Message = "" });
            
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await this._userRepository.Get().Include(u => u.Roles).Include(u => u.Permissions).ToListAsync();
            var result = new List<UserGetVM>();
            foreach (var user in users)
            {
                result.Add(new UserGetVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = user.Roles.Select(x => x.Name).ToList(),
                    Permissions = user.Permissions.Select(x => x.Name).ToList()
                });
            }

            return Ok(new APIResponse<List<UserGetVM>>
            {
                IsError = false,
                Message = "",
                data = result
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await this._userRepository.Get(id).Include(u => u.Roles).Include(u => u.Permissions).FirstOrDefaultAsync();
            if (user != null)
            {
                var data = new UserGetVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = user.Roles.Select(x => x.Name).ToList(),
                    Permissions = user.Permissions.Select(x => x.Name).ToList()
                };

                return Ok(new APIResponse<UserGetVM>
                {
                    IsError = false,
                    Message = "",
                    data = data
                });
            }

            return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateVM model)
        {
            var user = await this._userRepository.Get(id).Include(u => u.UserPermissions).FirstOrDefaultAsync();
            if (user != null)
            {
                user.PhoneNumber = model.PhoneNumber;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserPermissions = model.Permissions.Select(p => new UserPermission { PermissionId = (int)Enum.Parse(typeof(PERMISSIONS), p) }).ToList();

                this._userRepository.Update(user);
                await this._userRepository.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        
        [Route("entityexists/{data}")]
        [HttpGet]
        public async Task<bool> EntityExists(string data)
        {
            var userEntity = await this._userRepository.Get().Where(u => u.Email == data.ToLower()).FirstOrDefaultAsync();
            if (userEntity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
