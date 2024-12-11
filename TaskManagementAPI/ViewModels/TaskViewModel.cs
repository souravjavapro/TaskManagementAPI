using TaskManagementAPI.Enums;

namespace TaskManagementAPI.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
    }

    public class CreateTaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;
        public DateTime DueDate { get; set; }
    }

    public class UpdateTaskViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public DateTime DueDate { get; set; }
    }
}
