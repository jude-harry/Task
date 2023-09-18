using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Application
{
    public interface IProject
    {
        Task<BaseResponse<ProjectDto>> CreateProject(ProjectDto projectDto);
        Task<BaseResponse<IEnumerable<ProjectDto>>> GetProjects();
        Task<BaseResponse<ProjectDto>> GetProjectById(int id);
        Task<BaseResponse<ProjectDto>> DeleteProject(int id);
        Task<BaseResponse<ProjectDto>> UpdateProject(ProjectDto projectDto);
    }
}
