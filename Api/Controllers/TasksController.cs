
namespace Api.Controllers
{
    public class TasksController : BaseController
    {
        private readonly ITracksService _tracksService;

        public TasksController(ITracksService tracksService)
        {
            _tracksService = tracksService;
        }

        [HttpPost("task")]
        public async Task<IActionResult> CreateRequest(TaskDto request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _tracksService.CreateTaskProject(request);
            if (response.ResponseCode == ResponseCodes.CREATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("task")]
        public async Task<IActionResult> GetRequest()
        {
            var response = await _tracksService.GetTasks();
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("task-by-id")]
        public async Task<IActionResult> GetRequestId(int id)
        {
            var response = await _tracksService.GetTaskById(id);
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpPut("task")]
        public async Task<IActionResult> UpdateRequest(TaskDto request)
        {
            var response = await _tracksService.UpdateTask(request);
            if (response.ResponseCode == ResponseCodes.UPDATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpDelete("task-by-id")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var response = await _tracksService.DeleteTask(id);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("filterByStatusOrPriority")]
        public async Task<IActionResult> TaskByStatusOrPriority(TaskStatus? status, TaskPriority? priority)
        {
            var response = await _tracksService.TaskByStatusOrPriority(status,priority);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }
        [HttpGet("due-this-week")]
        public async Task<IActionResult> GetTasksDueThisWeek()
        {
            var response = await _tracksService.GetTasksDueThisWeek();
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }
        [HttpPut("{taskId}/assign")]
        public async Task<IActionResult> AssignTaskToProject(int taskId, int? projectId)
        {
            var response = await _tracksService.AssignTaskToProject(taskId,projectId);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }

    }


}
