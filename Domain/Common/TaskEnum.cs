using System.ComponentModel;

public enum TaskPriority
{
    [Description("Low")]
    Low,

    [Description("Medium")]
    Medium,

    [Description("High")]
    High
}

public enum TaskStatus
{
    [Description("Pending")]
    Pending,

    [Description("In Progress")]
    InProgress,

    [Description("Completed")]
    Completed
}

