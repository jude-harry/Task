
namespace Application.Services
{
    public class UserService : IUser
    {

        private readonly ILogger<UserService> _logger;
        private readonly IAsyncRepository<User>  _userRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(ILogger<UserService> logger, IAsyncRepository<User> usersRepo, IUnitOfWork unitOfWork)
        {
            _logger = logger;
             _userRepo = usersRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<UserDto>> CreateUser(UserDto UserDto)
        {
            try
            {
                User User = UserDto.CreateTaskFromDTO();
                var checkName = CreatePrevalidationChecks(User);
                if (checkName.Item1 == false)
                {
                    return new BaseResponse<UserDto>(checkName.Item2, ResponseCodes.DUPLICATE_RESOURCE);
                }
                var createdUser =  _userRepo.Add(User);
                await _unitOfWork.CommitChangesAsync();

                return new BaseResponse<UserDto>("Task  Created Successfully", ResponseCodes.CREATED);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while User.");
                return new BaseResponse<UserDto>("An error occurred while User", ex.Message);
            }
        }
        public async Task<BaseResponse<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                List<UserDto> UserDtos = new();
                var users = await  _userRepo.GetAll();
                foreach (var user in users)
                {
                    UserDto UserDto = new UserDto();
                    UserDto.ConvertToDTO(user);
                    UserDtos.Add(UserDto);
                }

                return new BaseResponse<IEnumerable<UserDto>>("Retrieved all user successfully", UserDtos, ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving list of user.");
                return new BaseResponse<IEnumerable<UserDto>>($"An error occurred while retrieving list of User. {ex.Message})");
            }
        }
        public async Task<BaseResponse<UserDto>> GetUserById(int id)
        {
            try
            {
                var task = await  _userRepo.GetById(id);

                if (task == null)
                {
                    return new BaseResponse<UserDto>("User not found", ResponseCodes.NOT_FOUND);
                }

                UserDto UserDto = new();
                UserDto.ConvertToDTO(task);

                return new BaseResponse<UserDto>("User returned successfully", UserDto, ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving User with {id}.");
                return new BaseResponse<UserDto>($"An error occurred while retrieving User with {id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<UserDto>> UpdateUser(UserDto UserDto)
        {
            try
            {
                var existingObject = _userRepo.SingleOrDefaultAsync(x => x.Id == UserDto.Id).Result; 
                UserDto.ConvertFromDTO(existingObject);
                if (existingObject == null)
                {
                    return new BaseResponse<UserDto>($"Object with {UserDto.Id} Doesn't Exist", ResponseCodes.NOT_FOUND);
                }

                 _userRepo.Update(existingObject);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<UserDto>("User updated successfully", ResponseCodes.UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Updating User with {UserDto.Id}.");
                return new BaseResponse<UserDto>($"An error occurred while updating User with {UserDto.Id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<UserDto>> DeleteUser(int id)
        {
            try
            {
                var user = await  _userRepo.GetById(id);
                if (user == null)
                {
                    _logger.LogInformation("User not found. ID: {ID}", id);
                    return new BaseResponse<UserDto>("User not found", ResponseCodes.NOT_FOUND);
                }
                var existingObject = DeletePrevalidationChecks(user);
                if (existingObject.Item1 == false)
                {
                    return new BaseResponse<UserDto>(existingObject.Item2, ResponseCodes.DUPLICATE_RESOURCE);
                }
                 _userRepo.Delete(user);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("User deleted successfully. ID: {ID}", id);
                return new BaseResponse<UserDto>("User deleted successfully", ResponseCodes.DELETED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting User with {id}.");
                return new BaseResponse<UserDto>($"An error occurred while deleting User with {id}.", ex.Message);
            }
        }
        private (bool, string) CreatePrevalidationChecks(User User)
        {
            User existingUser = new User();
            existingUser =  _userRepo.SingleOrDefaultAsync(x => x.Name == User.Name).Result;
            if (existingUser != null)
            {
                return (false, "True already exist");
            }
            return (true, string.Empty);
        }
        private (bool, string) DeletePrevalidationChecks(User User)
        {
            User existingUser = new User();
            existingUser =  _userRepo.SingleOrDefaultAsync(x => x.Id == User.Id).Result;
            if (existingUser == null)
            {
                return (false, $"Object with {User.Id} has been deleted previously");
            }
            return (true, User.Id.ToString());
        }
    }
}
