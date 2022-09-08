using petsLighthouseAPI.Models;
using System.Threading.Tasks;

namespace petsLighthouseAPI.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthUserRequest request);
        Response getUserData(int id);
        Response updateUser(UpdateUserDTO userDTO);
        Response createUser(CreateUserDTO userDTO);
        Task<Response> deleteUser(int id);
    }
}
