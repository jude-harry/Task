
namespace Application.DTOS
{
    public class ProjectDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public void ConvertToDTO(Project project)
        {
            if (project == null)
                throw new NullReferenceException();

            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
        }
        public void ConvertFromDTO(Project project)
        {
            if (project == null)
                throw new NullReferenceException();

            project.Id = Id;
            project.Name = Name;
            project.Description = Description;
        }
        public Project CreateTaskFromDTO()
        {
            var project = new Project();
            project.Id = Id;
            project.Name = Name;
            project.Description = Description;
            return project;     
        }
    }
}
