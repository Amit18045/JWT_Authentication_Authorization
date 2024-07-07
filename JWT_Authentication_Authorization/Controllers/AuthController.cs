using JWT_Authentication_Authorization.Interface;
using JWT_Authentication_Authorization.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Authentication_Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("Login")]

        public string Login( [FromBody]LoginRequest loginRequest)
        {
            var token=_authService.Login(loginRequest);
            return token;
        }
        [HttpPost("AssignRoleToUser")]
        public bool AssignRoleToUser([FromBody] AddUserRole addUserRole)
        {
            var addedUserRole = _authService.AssignRoleToUser(addUserRole);
            return addedUserRole;
        }
        [HttpPost("AddUser")]
        public User AddUser([FromBody] User user)
        {
            var addUser=_authService.AddUser(user);
            return addUser;
        }
        [HttpPost("AddRole")]
        public Role AddRole(Role role)
        {
            var addedRole=_authService.AddRole(role);
            return addedRole;
        }
      
    }
}
