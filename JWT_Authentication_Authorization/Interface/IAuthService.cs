using JWT_Authentication_Authorization.Model;

namespace JWT_Authentication_Authorization.Interface
{
    public interface IAuthService
    {
         User AddUser(User user);
        string Login(LoginRequest loginRequest);
        Role AddRole(Role role);
       bool AssignRoleToUser(AddUserRole obj);

    }
}
