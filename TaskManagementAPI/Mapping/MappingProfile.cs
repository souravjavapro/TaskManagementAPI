using AutoMapper;
using TaskManagementAPI.Models;
using TaskManagementAPI.ViewModels;

namespace TaskManagementAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to ViewModel
            CreateMap<TaskEntity, TaskViewModel>();

            // ViewModel to Entity
            CreateMap<CreateTaskViewModel, TaskEntity>();
            CreateMap<UpdateTaskViewModel, TaskEntity>();
        }
    }
}
