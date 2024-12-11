using TaskManagementAPI.Enums;
using TaskStatus = TaskManagementAPI.Enums.TaskStatus;

namespace TaskManagementAPI.Models
{
   

    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public DateTime DueDate { get; set; }
    }
}
