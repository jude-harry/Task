namespace Api.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("notification")]
        public async Task<IActionResult> CreateRequest(NotificationDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _notificationService.CreateNotification(request);
            if (response.ResponseCode == ResponseCodes.CREATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("notification")]
        public async Task<IActionResult> GetRequest()
        {
            var response = await _notificationService.GetNotifications();
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpGet("notification-by-id")]
        public async Task<IActionResult> GetRequestId(int id)
        {
            var response = await _notificationService.GetNotificationById(id);
            if (response.ResponseCode == ResponseCodes.SUCCESS) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpPut("notification")]
        public async Task<IActionResult> UpdateRequest(NotificationDto request)
        {
            var response = await _notificationService.UpdateNotification(request);
            if (response.ResponseCode == ResponseCodes.UPDATED) { return Ok(response); }
            return BadRequest(response.Message);
        }

        [HttpDelete("notification-by-id")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var response = await _notificationService.DeleteNotification(id);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }
        [HttpPut("notifications/{notificationId}/mark-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var response = await _notificationService.MarkNotificationAsRead(notificationId);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }
        [HttpPut("notifications/{notificationId}/mark-unread")]
        public async Task<IActionResult> MarkNotificationAsUnread(int notificationId)
        {
            var response = await _notificationService.MarkNotificationAsRead(notificationId);
            if (response.ResponseCode == ResponseCodes.DELETED) { return Ok(response); }
            return BadRequest(response.Message);
        }
    }
}
