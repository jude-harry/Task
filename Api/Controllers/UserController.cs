using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUser _userService;

        public UserController(IUser userService)
        {
           _userService = userService;
        }

        [HttpPost("user")]
        public async Task<IActionResult> CreateRequest(UserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _userService.CreateUser(request);
            if (response.ResponseCode == ResponseCodes.CREATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetRequest()
        {
            var response = await _userService.GetUsers();
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("user-by-id")]
        public async Task<IActionResult> GetRequestId(int id)
        {
            var response = await _userService.GetUserById(id);
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateRequest(UserDto request)
        {
            var response = await _userService.UpdateUser(request);
            if (response.ResponseCode == ResponseCodes.UPDATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpDelete("user-by-id")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }

    }
}
