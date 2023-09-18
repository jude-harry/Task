using Application.DTOS;
using Application.Responses;

namespace Application.Interfaces.Application
{
    public interface ITracksService
    {
        Task<BaseResponse<TaskDto>> CreateTaskProject(TaskDto taskDTO);
        Task<BaseResponse<IEnumerable<TaskDto>>> GetTasks(); 
        Task<BaseResponse<TaskDto>> GetTaskById(int id);
        Task<BaseResponse<TaskDto>> DeleteTask(int id);
        Task<BaseResponse<TaskDto>> UpdateTask(TaskDto taskDTO);
        Task<BaseResponse<IEnumerable<TaskDto>>>  TaskByStatusOrPriority(TaskStatus? status, TaskPriority? priority);
        Task<BaseResponse<IEnumerable<TaskDto>>> GetTasksDueThisWeek();
        Task<BaseResponse<TaskDto>> AssignTaskToProject(int taskId, int? projectId);
    }
}