using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TaskManagementAPI.Enums;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;
using TaskManagementAPI.ViewModels;
using TaskStatus = TaskManagementAPI.Enums.TaskStatus;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskStatus? status, [FromQuery] DateTime? dueDate, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var tasks = await _taskService.GetTasks(status, dueDate, pageNumber, pageSize);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching tasks.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var task = await _taskService.GetTaskById(id);
                if (task == null)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found.", id);
                    return NotFound($"Task with ID {id} not found.");
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the task with ID {TaskId}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdTask = await _taskService.CreateTask(taskViewModel);
                return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a task.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var isUpdated = await _taskService.UpdateTask(id, taskViewModel);
                if (!isUpdated)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found for update.", id);
                    return NotFound($"Task with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the task with ID {TaskId}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var isDeleted = await _taskService.DeleteTask(id);
                if (!isDeleted)
                {
                    _logger.LogWarning("Task with ID {TaskId} not found for deletion.", id);
                    return NotFound($"Task with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the task with ID {TaskId}.", id);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



      
        [HttpGet("export")]
        public async Task<IActionResult> ExportTasksAsCsv()
        {
            var csvBytes = await _taskService.ExportTasksAsCsvAsync();

            // Return the CSV file as a downloadable file
            return File(csvBytes, "text/csv", "tasks.csv");
        }

    }
}