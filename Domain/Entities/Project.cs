﻿
namespace Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TaskProject> Tasks { get; set; }
}

