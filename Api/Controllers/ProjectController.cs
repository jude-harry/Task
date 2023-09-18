
namespace Api.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProject _projectService;

        public ProjectController(IProject projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateRequest(ProjectDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _projectService.CreateProject(request);
            if (response.ResponseCode == ResponseCodes.CREATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("project")]
        public async Task<IActionResult> GetRequest()
        {
            var response = await _projectService.GetProjects();
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("project-by-id")]
        public async Task<IActionResult> GetRequestId(int id)
        {
            var response = await _projectService.GetProjectById(id);
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpPut("project")]
        public async Task<IActionResult> UpdateRequest(ProjectDto request)
        {
            var response = await _projectService.UpdateProject(request);
            if (response.ResponseCode == ResponseCodes.UPDATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpDelete("project-by-id")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var response = await _projectService.DeleteProject(id);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }
    }
}
