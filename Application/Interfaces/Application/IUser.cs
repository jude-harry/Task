
namespace Application.Interfaces.Application
{
    public interface IUser
    {
        Task<BaseResponse<UserDto>> CreateUser(UserDto UserDto);
        Task<BaseResponse<IEnumerable<UserDto>>> GetUsers();
        Task<BaseResponse<UserDto>> GetUserById(int id);
        Task<BaseResponse<UserDto>> DeleteUser(int id);
        Task<BaseResponse<UserDto>> UpdateUser(UserDto UserDto);
    }
}
