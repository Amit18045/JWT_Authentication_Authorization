using JWT_Authentication_Authorization.DataContext;
using JWT_Authentication_Authorization.Interface;
using JWT_Authentication_Authorization.Model;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace JWT_Authentication_Authorization.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _jwtContext;
        private readonly IConfiguration _configuration;
        public AuthService(JwtContext jwtContext, IConfiguration configuration)
        {
            _jwtContext = jwtContext;
            _configuration = configuration;
        }

        public Role AddRole(Role role)
        {
           var added=_jwtContext.Roles.Add(role);
            _jwtContext.SaveChanges();
            return added.Entity;
        }

        public User AddUser(User user)
        {
            var added=_jwtContext.Users.Add(user);
            _jwtContext.SaveChanges();
            return added.Entity;
        }

        public bool AssignRoleToUser(AddUserRole obj)
        {
            try
            {
                var addRoles = new List<UserRole>();
                var user = _jwtContext.Users.SingleOrDefault(x => x.Id == obj.UserId);
                if (user == null)
                {
                    throw new Exception("User is not valid");
                }
                foreach (int role in obj.RoleIds)
                {
                    var userRole = new UserRole();
                    userRole.RoleId = role;
                    userRole.UserId = user.Id;
                    addRoles.Add(userRole);
                }
                _jwtContext.UserRoles.AddRange(addRoles);
                _jwtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        //public string Login(LoginRequest loginRequest)
        //{
        //   if(loginRequest.Username!=null  && loginRequest.Password!=null)
        //   {
        //        var user = _jwtContext.Users.SingleOrDefault(x => x.Username == loginRequest.Username && x.Password == loginRequest.Password);
        //        if(user == null)
        //        {
        //            var claim = new List<Claim>
        //            {
        //                new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
        //                new Claim("Id",user.Id.ToString()),
        //                new Claim("UserName",user.Name)
        //            };
        //            var userRole = _jwtContext.UserRoles.Where(u => u.UserId == user.Id).ToList();
        //            var roleIDS=userRole.Select(x=>x.RoleId).ToList();
        //            var roles=_jwtContext.Roles.Where(r=> roleIDS.Contains(r.Id)).ToList();
        //            foreach(var role in roles)
        //            {
        //                claim.Add(new Claim("Role",role.Name));
        //            }
        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
        //            var signIN=new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
        //            var token = new JwtSecurityToken(
        //                _configuration["Jwt:Issuer"],
        //                _configuration["Jwt:Audience"],
        //                claim,
        //                expires: DateTime.UtcNow.AddMinutes(10),
        //                signingCredentials: signIN);
        //            var jwtToken=new JwtSecurityTokenHandler().WriteToken(token);
        //            return jwtToken;
        //        }
        //        else
        //        {
        //            throw new Exception("User is not Vaild");
        //        }
        //   }
        //    else
        //    {
        //        throw new Exception("Credentials are not vaild");
        //    }
        //}


        public string Login(LoginRequest loginRequest)
        {
            if (loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = _jwtContext.Users.SingleOrDefault(s => s.Username == loginRequest.Username && s.Password == loginRequest.Password);
                if (user != null)
                {
                    var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.Name)
                    };
                    var userRoles = _jwtContext.UserRoles.Where(u => u.UserId == user.Id).ToList();
                    var roleIds = userRoles.Select(s => s.RoleId).ToList();
                    var roles = _jwtContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    throw new Exception("user is not valid");
                }
            }
            else
            {
                throw new Exception("credentials are not valid");
            }
        }
    }
}
