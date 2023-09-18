
namespace Application.DTOS
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
        public TaskStatus Status { get; set; }
        public int UserId { get; set; }
        public int? ProjectId { get; set; }

        public void ConvertToDTO(TaskProject taskProject)
        {
            if (taskProject == null)
                throw new NullReferenceException();

            Id = taskProject.Id;
            Title = taskProject.Title;
            Description = taskProject.Description;
            DueDate = taskProject.DueDate;
            Priority = taskProject.Priority;
            Status = taskProject.Status;
            UserId = taskProject.UserId;
            ProjectId = taskProject.ProjectId;
        }
        public void ConvertFromDTO(TaskProject taskProject)
        {
            if (taskProject == null)
                throw new NullReferenceException();

            taskProject.Id = Id;
            taskProject.Title = Title;
            taskProject.Description = Description;
            taskProject.DueDate = DueDate;
            taskProject.Priority = Priority;
            taskProject.Status = Status;
            taskProject.UserId = UserId;
            taskProject.ProjectId = ProjectId;
        }
        public TaskProject CreateTaskFromDTO()
        {
            var taskProject = new TaskProject();

            taskProject.Id = Id;
            taskProject.Title = Title;
            taskProject.Description = Description;
            taskProject.DueDate = DueDate;
            taskProject.Priority = Priority;
            taskProject.Status = Status;
            taskProject.UserId = UserId;
            taskProject.ProjectId = ProjectId;
            return taskProject;
        }
     
    }
}
