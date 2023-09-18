
namespace Application.Interfaces.Application
{
    public interface INotificationService
    {

        Task<BaseResponse<NotificationDto>> CreateNotification(NotificationDto NotificationDto);
        Task<BaseResponse<IEnumerable<NotificationDto>>> GetNotifications();
        Task<BaseResponse<NotificationDto>> GetNotificationById(int id);
        Task<BaseResponse<NotificationDto>> DeleteNotification(int id);
        Task<BaseResponse<NotificationDto>> UpdateNotification(NotificationDto NotificationDto);
        Task<BaseResponse<NotificationDto>> MarkNotificationAsRead(int notificationId);
        Task<BaseResponse<NotificationDto>> MarkNotificationAsUnread(int notificationId);
    }
}
