
using TaskManagementAPI.ViewModels;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using AutoMapper;
using Task = TaskManagementAPI.Models.TaskEntity;
using TaskStatus = TaskManagementAPI.Enums.TaskStatus;
using System.Text; // Alias your custom Task to avoid conflict

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(TaskDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskViewModel>> GetTasks(TaskStatus? status, DateTime? dueDate, int pageNumber, int pageSize)
        {
            var query = _context.Tasks.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status);
            }

            if (dueDate.HasValue)
            {
                query = query.Where(t => t.DueDate <= dueDate);
            }

            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskViewModel>>(tasks);
        }

        public async Task<TaskViewModel> GetTaskById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return null; // You can throw an exception or return a default error view model.
            }

            return _mapper.Map<TaskViewModel>(task);
        }

        public async Task<TaskViewModel> CreateTask(CreateTaskViewModel task)
        {
            var taskEntity = _mapper.Map<Task>(task);
            _context.Tasks.Add(taskEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskViewModel>(taskEntity);
        }

        public async Task<bool> UpdateTask(int id, UpdateTaskViewModel task)
        {
            var existingTask = await _context.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return false;
            }

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.Status = task.Status;
            existingTask.DueDate = task.DueDate;

            _context.Tasks.Update(existingTask);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<byte[]> ExportTasksAsCsvAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();

            // Create CSV header
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id,Title,Description,Status,DueDate");

            // Add task data
            foreach (var task in tasks)
            {
                csvBuilder.AppendLine($"{task.Id},{task.Title},{task.Description},{task.Status},{task.DueDate.ToString("yyyy-MM-ddTHH:mm:ss")}");
            }

            // Convert the CSV data to a byte array
            return Encoding.UTF8.GetBytes(csvBuilder.ToString());
        }
    }
}
