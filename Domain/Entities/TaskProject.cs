
namespace Domain.Entities;

public  class TaskProject
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public TaskStatus Status { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int? ProjectId { get; set; }
    public Project Project { get; set; }
}

