using TaskManagementAPI.ViewModels;
using TaskManagementAPI.Models;
using TaskStatus = TaskManagementAPI.Enums.TaskStatus;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskViewModel>> GetTasks(TaskStatus? status, DateTime? dueDate, int pageNumber, int pageSize);
        Task<TaskViewModel> GetTaskById(int id);
        Task<TaskViewModel> CreateTask(CreateTaskViewModel task);
        Task<bool> UpdateTask(int id, UpdateTaskViewModel task);
        Task<bool> DeleteTask(int id);

        Task<byte[]> ExportTasksAsCsvAsync();

    }
}
