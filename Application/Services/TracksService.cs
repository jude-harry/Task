
using Application.DTOS;
using Application.Features.Constants;
using Application.Interfaces;
using Application.Interfaces.Application;
using Application.Responses;
using Domain.Entities;

namespace Application.Services;
public class TracksService : ITracksService
{
    private readonly ILogger<TracksService> _logger;
    private readonly IAsyncRepository<TaskProject> _tasksRepo;
    private readonly IUnitOfWork _unitOfWork;

    public TracksService(ILogger<TracksService> logger, IAsyncRepository<TaskProject> tasksRepo, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _tasksRepo = tasksRepo;
        _unitOfWork = unitOfWork;
    }
    public async Task<BaseResponse<TaskDto>> CreateTaskProject(TaskDto taskDTO)
    {
        try
        {
            TaskProject taskProject = taskDTO.CreateTaskFromDTO();
            var checkName = CreatePrevalidationChecks(taskProject);
            if (checkName.Item1 == false)
            {
                return new BaseResponse<TaskDto>(checkName.Item2, ResponseCodes.DUPLICATE_RESOURCE);
            }
            var createdTaskProject = _tasksRepo.Add(taskProject);
            await _unitOfWork.CommitChangesAsync();

            return new BaseResponse<TaskDto>("Task  Created Successfully", ResponseCodes.CREATED);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while TaskProject.");
            return new BaseResponse<TaskDto>("An error occurred while TaskProject", ex.Message);
        }
    }
    public async Task<BaseResponse<IEnumerable<TaskDto>>> GetTasks()
    {
        try
        {
            List<TaskDto> taskDtos = new ();
            var tasks = await _tasksRepo.GetAll();
            foreach (var task in tasks)
            {
                TaskDto taskDTO = new TaskDto();
                taskDTO.ConvertToDTO(task);
                taskDtos.Add(taskDTO);
            }

            return new BaseResponse<IEnumerable<TaskDto>>("Retrieved all task successfully", taskDtos, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving list of Task.");
            return new BaseResponse<IEnumerable<TaskDto>>($"An error occurred while retrieving list of Task. {ex.Message})");
        }
    }
    public async Task<BaseResponse<TaskDto>> GetTaskById(int id)
    {
        try
        {
            var task = await _tasksRepo.GetById(id);

            if (task == null)
            {
                return new BaseResponse<TaskDto>("Task not found", ResponseCodes.NOT_FOUND);
            }

            TaskDto taskDTO = new ();
            taskDTO.ConvertToDTO(task);

            return new BaseResponse<TaskDto>("Demo returned successfully", taskDTO, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving Demo with {id}.");
            return new BaseResponse<TaskDto>($"An error occurred while retrieving Demo with {id}.", ex.Message);
        }
    }
    public async Task<BaseResponse<TaskDto>> UpdateTask(TaskDto taskDTO)
    {
        try
        {
            var existingObject = _tasksRepo.SingleOrDefaultAsync(x => x.Id == taskDTO.Id).Result;
            taskDTO.ConvertFromDTO(existingObject);
            if (existingObject == null)
            {
                return new BaseResponse<TaskDto>($"Object with {taskDTO.Id} Doesn't Exist", ResponseCodes.NOT_FOUND);
            }
             _tasksRepo.Update(existingObject);
            await _unitOfWork.CommitAsync();
            return new BaseResponse<TaskDto>("Task updated successfully", ResponseCodes.UPDATED);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while Updating Demo with {taskDTO.Id}.");
            return new BaseResponse<TaskDto>($"An error occurred while updating Demo with {taskDTO.Id}.", ex.Message);
        }
    }
    public async Task<BaseResponse<TaskDto>> DeleteTask(int id)
    {
        try
        {
            var task= await _tasksRepo.GetById(id);
            if (task == null)
            {
                _logger.LogInformation("Task not found. ID: {ID}", id);
                return new BaseResponse<TaskDto>("Task not found", ResponseCodes.NOT_FOUND);
            }
            var existingObject = DeletePrevalidationChecks(task);
            if (existingObject.Item1 == false)
            {
                return new BaseResponse<TaskDto>(existingObject.Item2, ResponseCodes.DUPLICATE_RESOURCE);
            }
             _tasksRepo.Delete(task);
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("Demo deleted successfully. ID: {ID}", id);
            return new BaseResponse<TaskDto>("Demo deleted successfully",  ResponseCodes.DELETED);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while deleting Demo with {id}.");
            return new BaseResponse<TaskDto>($"An error occurred while deleting Demo with {id}.", ex.Message);
        }
    }
    public async Task<BaseResponse<TaskDto>> AssignTaskToProject(int taskId, int? projectId)
    {
        try
        {
            var existingObject = _tasksRepo.SingleOrDefaultAsync(x => x.Id == taskId).Result;
            if (existingObject == null)
            {
                return new BaseResponse<TaskDto>($"Object with {taskId} Doesn't Exist", ResponseCodes.NOT_FOUND);
            }
            existingObject.ProjectId = projectId;
            _tasksRepo.Update(existingObject);
            await _unitOfWork.CommitAsync();
            return new BaseResponse<TaskDto>("Task updated successfully", ResponseCodes.UPDATED);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while Updating Demo with {taskId}.");
            return new BaseResponse<TaskDto>($"An error occurred while updating Demo with {taskId}.", ex.Message);
        }
    }
    public async Task<BaseResponse<IEnumerable<TaskDto>>> TaskByStatusOrPriority(TaskStatus? status, TaskPriority? priority)
    {
        try
        {
            List<TaskDto> taskDtos = new();
            var tasks = await _tasksRepo.GetAll(x => x.Status == status.Value && x.Priority == priority.Value
            );
            foreach (var task in tasks)
            {
                TaskDto taskDTO = new TaskDto();
                taskDTO.ConvertToDTO(task);
                taskDtos.Add(taskDTO);
            }

            return new BaseResponse<IEnumerable<TaskDto>>("Retrieved all task successfully", taskDtos, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving list of Task.");
            return new BaseResponse<IEnumerable<TaskDto>>($"An error occurred while retrieving list of Task. {ex.Message})");
        }
    }
    public async Task<BaseResponse<IEnumerable<TaskDto>>> GetTasksDueThisWeek()
    {
        DateTime today = DateTime.Today;
        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
        DateTime startOfWeek = today.AddDays(-daysUntilMonday);
        DateTime endOfWeek = startOfWeek.AddDays(7).AddSeconds(-1);

        try
        {
            List<TaskDto> taskDtos = new();
            var tasks = await _tasksRepo.GetAll(x => x.DueDate
             >= startOfWeek && x.DueDate <= endOfWeek
            );
            foreach (var task in tasks)
            {
                TaskDto taskDTO = new TaskDto();
                taskDTO.ConvertToDTO(task);
                taskDtos.Add(taskDTO);
            }

            return new BaseResponse<IEnumerable<TaskDto>>("Retrieved all task successfully", taskDtos, ResponseCodes.SUCCESS);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving list of Task.");
            return new BaseResponse<IEnumerable<TaskDto>>($"An error occurred while retrieving list of Task. {ex.Message})");
        }
    }
    private (bool, string) CreatePrevalidationChecks(TaskProject taskProject)
    {
        TaskProject existingTaskProject = new TaskProject();
        existingTaskProject = _tasksRepo.SingleOrDefaultAsync(x => x.Title == taskProject.Title).Result;
        if (existingTaskProject != null)
        {
            return (false, "True already exist");
        }
        return (true, string.Empty);
    }
    private (bool, string) DeletePrevalidationChecks(TaskProject taskProject)
    {
        TaskProject existingTaskProject = new TaskProject();
        existingTaskProject = _tasksRepo.SingleOrDefaultAsync(x => x.Id == taskProject.Id).Result;
        if (existingTaskProject == null)
        {
            return (false, $"Object with {taskProject.Id} has been deleted previously");
        }
        return (true, taskProject.Id.ToString());
    }

}

