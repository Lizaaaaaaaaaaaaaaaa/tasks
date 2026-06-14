using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorApp1.DTOs;

namespace BlazorApp1.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController(
        AppDbContext context)
        : ControllerBase
    {
        private readonly AppDbContext _context =
            context;

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.WorkTasks
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.WorkTasks
                .FirstOrDefaultAsync(t =>
                    t.idT == id);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(
            [FromBody] CreateTaskDto dto)
        {
            var assignee = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.idU == dto.AssigneeId);

            if (assignee == null)
            {
                return BadRequest(
                    "Исполнитель не найден");
            }

            var task = new WorkTask
            {
                Title = dto.Title,
                Description = dto.Description,
                AssigneeId = dto.AssigneeId,
                DueDate = dto.DueDate,
                Departments_idD =
                    assignee.Departments_idD,
                TaskStatus_idTS = 1,
                TaskPriority_idTP =
                    dto.TaskPriority_idTP,
                CreatedAtT = DateTime.Now,
                CreatedById = dto.AssigneeId
            };

            _context.WorkTasks.Add(task);

            await _context.SaveChangesAsync();

            return Ok(task);
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult>
    UpdateTaskStatus(
        int id,
        [FromBody]
        UpdateTaskStatusDto dto)
        {
            var task = await _context.WorkTasks
                .FirstOrDefaultAsync(t =>
                    t.idT == id);

            if (task == null)
            {
                return NotFound(
                    "Задача не найдена");
            }

            var statusExists = await _context
                .WorkTaskStatuses
                .AnyAsync(s =>
                    s.idTS == dto.StatusId);

            if (!statusExists)
            {
                return BadRequest(
                    "Статус не найден");
            }

            task.TaskStatus_idTS =
                dto.StatusId;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Статус обновлён"
            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult>
    DeleteTask(int id)
        {
            // найти задачу
            var task = await _context.WorkTasks
                .FirstOrDefaultAsync(t =>
                    t.idT == id);

            if (task == null)
            {
                return NotFound(
                    "Задача не найдена");
            }

            // найти файлы задачи
            var files = await _context.TaskFiles
                .Where(f => f.Tasks_idT == id)
                .ToListAsync();

            // удалить файлы из папки
            foreach (var file in files)
            {
                if (!string.IsNullOrEmpty(
                    file._FilePath))
                {
                    var physicalPath =
                        Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            file._FilePath
                                .TrimStart('/'));

                    if (System.IO.File.Exists(
                        physicalPath))
                    {
                        System.IO.File.Delete(
                            physicalPath);
                    }
                }
            }

            // удалить записи файлов
            _context.TaskFiles
                .RemoveRange(files);

            // удалить задачу
            _context.WorkTasks
                .Remove(task);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Задача удалена"
            });
        }
    }
}