
namespace Application.DTOS
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public void ConvertToDTO(Notification notification)
        {
            if (notification == null)
                throw new NullReferenceException();

            Id = notification.Id;
            Type = notification.Type;
            Message = notification.Message;
            Timestamp = notification.Timestamp;
            IsRead = notification.IsRead;
        }
        public void ConvertFromDTO(Notification notification)
        {
            if (notification == null)
                throw new NullReferenceException();

            notification.Id = Id;
            notification.Type = Type;
            notification.Message = Message;
            notification.Timestamp = Timestamp;
            notification.IsRead = IsRead;
        }
        public Notification CreateNotificationFromDTO()
        {
            var notification = new Notification();
            notification.Id = Id;
            notification.Type = Type;
            notification.Message = Message;
            notification.Timestamp = Timestamp;
            notification.IsRead = IsRead;
            return notification;
        }
    }
}
