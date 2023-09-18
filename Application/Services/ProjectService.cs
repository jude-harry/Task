
namespace Application.Services
{
    public class ProjectService : IProject
    {
        private readonly ILogger<ProjectService> _logger;
        private readonly IAsyncRepository<Project> _projectRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(ILogger<ProjectService> logger, IAsyncRepository<Project> tasksRepo, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _projectRepo = tasksRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<ProjectDto>> CreateProject(ProjectDto projectDto)
        {
            try
            {
                Project project =projectDto.CreateTaskFromDTO();
                var checkName = CreatePrevalidationChecks(project);
                if (checkName.Item1 == false)
                {
                    return new BaseResponse<ProjectDto>(checkName.Item2, ResponseCodes.DUPLICATE_RESOURCE);
                }
                var createdProject = _projectRepo.Add(project);
                await _unitOfWork.CommitChangesAsync();

                return new BaseResponse<ProjectDto>("Task  Created Successfully", ResponseCodes.CREATED);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Project.");
                return new BaseResponse<ProjectDto>("An error occurred while Project", ex.Message);
            }
        }
        public async Task<BaseResponse<IEnumerable<ProjectDto>>> GetProjects()
        {
            try
            {
                List<ProjectDto> ProjectDtos = new();
                var tasks = await _projectRepo.GetAll();
                foreach (var task in tasks)
                {
                    ProjectDto ProjectDto = new ProjectDto();
                    ProjectDto.ConvertToDTO(task);
                    ProjectDtos.Add(ProjectDto);
                }

                return new BaseResponse<IEnumerable<ProjectDto>>("Retrieved all task successfully", ProjectDtos, ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving list of Task.");
                return new BaseResponse<IEnumerable<ProjectDto>>($"An error occurred while retrieving list of Task. {ex.Message})");
            }
        }
        public async Task<BaseResponse<ProjectDto>> GetProjectById(int id)
        {
            try
            {
                var task = await _projectRepo.GetById(id);

                if (task == null)
                {
                    return new BaseResponse<ProjectDto>("Task not found", ResponseCodes.NOT_FOUND);
                }

                ProjectDto ProjectDto = new();
                ProjectDto.ConvertToDTO(task);

                return new BaseResponse<ProjectDto>("Demo returned successfully", ProjectDto, ResponseCodes.SUCCESS);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving Demo with {id}.");
                return new BaseResponse<ProjectDto>($"An error occurred while retrieving Demo with {id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<ProjectDto>> UpdateProject(ProjectDto ProjectDto)
        {
            try
            {
                var existingObject = _projectRepo.SingleOrDefaultAsync(x => x.Id == ProjectDto.Id).Result;
                ProjectDto.ConvertFromDTO(existingObject);
                if (existingObject == null)
                {
                    return new BaseResponse<ProjectDto>($"Object with {ProjectDto.Id} Doesn't Exist", ResponseCodes.NOT_FOUND);
                }
                _projectRepo.Update(existingObject);
                await _unitOfWork.CommitAsync();
                return new BaseResponse<ProjectDto>("Task updated successfully", ResponseCodes.UPDATED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while Updating Demo with {ProjectDto.Id}.");
                return new BaseResponse<ProjectDto>($"An error occurred while updating Demo with {ProjectDto.Id}.", ex.Message);
            }
        }
        public async Task<BaseResponse<ProjectDto>> DeleteProject(int id)
        {
            try
            {
                var task = await _projectRepo.GetById(id);
                if (task == null)
                {
                    _logger.LogInformation("Task not found. ID: {ID}", id);
                    return new BaseResponse<ProjectDto>("Task not found", ResponseCodes.NOT_FOUND);
                }
                var existingObject = DeletePrevalidationChecks(task);
                if (existingObject.Item1 == false)
                {
                    return new BaseResponse<ProjectDto>(existingObject.Item2, ResponseCodes.DUPLICATE_RESOURCE);
                }
                _projectRepo.Delete(task);
                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Demo deleted successfully. ID: {ID}", id);
                return new BaseResponse<ProjectDto>("Demo deleted successfully", ResponseCodes.DELETED);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting Demo with {id}.");
                return new BaseResponse<ProjectDto>($"An error occurred while deleting Demo with {id}.", ex.Message);
            }
        }
        private (bool, string) CreatePrevalidationChecks(Project Project)
        {
            Project existingProject = new Project();
            existingProject = _projectRepo.SingleOrDefaultAsync(x => x.Name == Project.Name).Result;
            if (existingProject != null)
            {
                return (false, "True already exist");
            }
            return (true, string.Empty);
        }
        private (bool, string) DeletePrevalidationChecks(Project Project)
        {
            Project existingProject = new Project();
            existingProject = _projectRepo.SingleOrDefaultAsync(x => x.Id == Project.Id).Result;
            if (existingProject == null)
            {
                return (false, $"Object with {Project.Id} has been deleted previously");
            }
            return (true, Project.Id.ToString());
        }
    }
}
