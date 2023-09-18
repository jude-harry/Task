using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IAsyncRepository<Notification> _notificationRepo;
        private readonly IAsyncRepository<TaskProject> _tasksRepo;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(ILogger<NotificationService> logger, IAsyncRepository<Notification> notificationRepo, IUnitOfWork unitOfWork, IAsyncRepository<TaskProject> tasksRepo)
        {
            _logger = logger;
            _notificationRepo = notificationRepo;
            _unitOfWork = unitOfWork;
            _tasksRepo = tasksRepo;
        }
        public async Task<BaseResponse<NotificationDto>> CreateNotification(NotificationDto notificationDto)
        {
            try
            {
                Notification notification = notificationDto.CreateNotificationFromDTO();
                var checkName = CreatePrevalidationChecks(notification);
                if (checkName.Item1 == false)
                {
                    return new BaseResponse<NotificationDto>(checkName.Item2, ResponseCodes.DUPLICATE_RESOURCE);
                }
                var createdProject = _notificationRepo.Add(notification);
                await _unitOfWork.CommitChangesAsync();

                return new BaseResponse<NotificationDto>("Task  Created Successfully", ResponseCodes.CREATED);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Project.");
                return new BaseResponse<NotificationDto>("An error occurred while Project", ex.Message);
            }
        }
        public async Task<BaseResponse<IEnumerable<NotificationDto>>> GetNotifications()
        {
            try
            {
                List<NotificationDto> notificationDtos = new();
                var tasks = await _notificationRepo.GetAll();
                foreach (var task in tasks)
                {
                    NotificationDto notificationDto= new NotificationDto();
                    notificationDto.ConvertToDTO(task);
                    notificationDtos.Add(notificationDto);
                }

                return new BaseResponse<IEnumerable<NotificationDto>>("Retrieved all task successfully", notificationDtos, ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving list of Task.");
                return new BaseResponse<IEnumerable<NotificationDto>>($"An error occurred while retrieving list of Task. {ex.Message})");
            }
        }
        public async Task<BaseResponse<NotificationDto>> GetNotificationById(int id)
        {
            try
            {
                var notification = await _notificationRepo.GetById(id);

                if (notification == null)
                {
                    return new BaseResponse<NotificationDto>("Task not found", ResponseCodes.NOT_FOUND);
                }

                NotificationDto notificationDto= new();
                notificationDto.ConvertToDTO(notification);

                return new BaseResponse<NotificationDto>("Demo returned successfully", notificationDto, ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Demo with {id}.");
                return new BaseResponse<NotificationDto>($"An error occurred while retrieving Demo with {id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<NotificationDto>> UpdateNotification(NotificationDto notificationDto)
        {
            try
            {
                var existingObject = _notificationRepo.SingleOrDefaultAsync(x => x.Id == notificationDto.Id).Result;
                notificationDto.ConvertFromDTO(existingObject);
                if (existingObject == null)
                {
                    return new BaseResponse<NotificationDto>($"Object with {notificationDto.Id} Doesn't Exist", ResponseCodes.NOT_FOUND);
                }
                _notificationRepo.Update(existingObject);
                await _unitOfWork.CommitAsync();
                return new BaseResponse<NotificationDto>("Task updated successfully", ResponseCodes.UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Updating Demo with {notificationDto.Id}.");
                return new BaseResponse<NotificationDto>($"An error occurred while updating Demo with {notificationDto.Id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<NotificationDto>> DeleteNotification(int id)
        {
            try
            {
                var task = await _notificationRepo.GetById(id);
                if (task == null)
                {
                    _logger.LogInformation("Task not found. ID: {ID}", id); 
                    return new BaseResponse<NotificationDto>("Task not found", ResponseCodes.NOT_FOUND);
                }
                var existingObject = DeletePrevalidationChecks(task);
                if (existingObject.Item1 == false)
                {
                    return new BaseResponse<NotificationDto>(existingObject.Item2, ResponseCodes.DUPLICATE_RESOURCE);
                }
                _notificationRepo.Delete(task);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Demo deleted successfully. ID: {ID}", id);
                return new BaseResponse<NotificationDto>("Demo deleted successfully", ResponseCodes.DELETED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting Demo with {id}.");
                return new BaseResponse<NotificationDto>($"An error occurred while deleting Demo with {id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<NotificationDto>> MarkNotificationAsRead(int notificationId)
        {
            try
            {
                var existingObject = _notificationRepo.SingleOrDefaultAsync(x => x.Id ==  notificationId).Result;
                if (existingObject == null)
                {
                    return new BaseResponse<NotificationDto>($"Object with {notificationId} Doesn't Exist", ResponseCodes.NOT_FOUND);
                }
                existingObject.IsRead = true;
                _notificationRepo.Update(existingObject);
                await _unitOfWork.CommitAsync();
                return new BaseResponse<NotificationDto>("Task updated successfully", ResponseCodes.UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Updating Demo with {notificationId}.");
                return new BaseResponse<NotificationDto>($"An error occurred while updating Demo with {notificationId}.", ex.Message);
            }
        }
        public async Task<BaseResponse<NotificationDto>> MarkNotificationAsUnread(int notificationId)
        {
            try
            {
                var existingObject = _notificationRepo.SingleOrDefaultAsync(x => x.Id == notificationId).Result;
                if (existingObject == null)
                {
                    return new BaseResponse<NotificationDto>($"Object with {notificationId} Doesn't Exist", ResponseCodes.NOT_FOUND);
                }
                existingObject.IsRead = false;
                _notificationRepo.Update(existingObject);
                await _unitOfWork.CommitAsync();
                return new BaseResponse<NotificationDto>("Task updated successfully", ResponseCodes.UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Updating Demo with {notificationId}.");
                return new BaseResponse<NotificationDto>($"An error occurred while updating Demo with {notificationId}.", ex.Message);
            }
        }
        public async Task CheckAndSendNotifications()
        {
            int userId = 1;
            DateTime currentDate = DateTime.Now;
            var tasksDueSoon = await _tasksRepo.GetAll(x => x.DueDate
             >= currentDate && x.DueDate <= currentDate
            );
            foreach (var task in tasksDueSoon)
            {
                var notification = new Notification
                {
                    Type = "Due Date Reminder",
                    Message = $"Task '{task.Title}' is due soon on {task.DueDate}.",
                    Timestamp = DateTime.Now,
                    IsRead = false
                };
                await _notificationRepo.Add(notification);
            }

            var completedTasksCreatedByUser = await _tasksRepo.WhereQueryable(
                 task => task.UserId == userId && task.Status == TaskStatus.Completed
            );

            foreach (var task in completedTasksCreatedByUser)
            {
                var notification = new Notification
                {
                    Type = "Task Completed",
                    Message = $"Task '{task.Title}' has been marked as completed.",
                    Timestamp = DateTime.Now,
                    IsRead = false
                };
                await _notificationRepo.Add(notification);
            }

            var newTaskAssignedToUser = await _tasksRepo.WhereQueryable(
                     task => task.UserId == userId && task.Status == TaskStatus.Pending
            );

            foreach (var task in newTaskAssignedToUser)
            {
                var notification = new Notification
                {
                    Type = "New Task Assigned",
                    Message = $"You have been assigned a new task: '{task.Title}'.",
                    Timestamp = DateTime.Now,
                    IsRead = false
                };
                await _notificationRepo.Add(notification);
            }
        }

        private (bool, string) CreatePrevalidationChecks(Notification notification)
        {
            Notification existingNotification = new ();
            existingNotification = _notificationRepo.SingleOrDefaultAsync(x => x.Type == notification.Type).Result;
            if (existingNotification != null)
            {
                return (false, "True already exist");
            }
            return (true, string.Empty);
        }
        private (bool, string) DeletePrevalidationChecks(Notification notification)
        {
            Notification existingNotification = new();
            existingNotification = _notificationRepo.SingleOrDefaultAsync(x => x.Id == notification.Id).Result;
            if (existingNotification == null)
            {
                return (false, $"Object with {notification.Id} has been deleted previously");
            }
            return (true, notification.Id.ToString());
        }

    }
}
