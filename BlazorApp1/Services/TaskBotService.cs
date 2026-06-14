using BlazorApp1.Data;
using BlazorApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Services
{
    public class TaskBotService
    {
        private readonly AppDbContext _context;

        public TaskBotService(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task<string>
            HandleMessage(string text)
        {
            text = text.Trim();

            if (text.ToLower() == "/tasks")
            {
                return await GetTasks();
            }

            if (text.StartsWith("/create"))
            {
                return await CreateTask(text);
            }

            if (text.StartsWith("/done"))
            {
                return await CompleteTask(text);
            }

            return
                "Команды:\n" +
                "/tasks\n" +
                "/create Название | Описание\n" +
                "/done ID";
        }

        private async Task<string>
            GetTasks()
        {
            var tasks = await _context
                .WorkTasks
                .Take(10)
                .ToListAsync();

            if (!tasks.Any())
            {
                return "Задач нет";
            }

            var result =
                "Список задач:\n\n";

            foreach (var task in tasks)
            {
                result +=
                    $"{task.idT}. " +
                    $"{task.Title}\n";
            }

            return result;
        }

        private async Task<string>
            CreateTask(string text)
        {
            try
            {
                var command =
                    text.Replace("/create", "")
                        .Trim();

                var parts =
                    command.Split('|');

                if (parts.Length < 2)
                {
                    return
                        "Формат:\n" +
                        "/create Название | Описание";
                }

                var title =
                    parts[0].Trim();

                var description =
                    parts[1].Trim();

                var firstStatus =
                    await _context
                        .WorkTaskStatuses
                        .FirstOrDefaultAsync();

                var firstPriority =
                    await _context
                        .WorkTaskPriorities
                        .FirstOrDefaultAsync();

                if (firstStatus == null ||
                    firstPriority == null)
                {
                    return
                        "Не найдены статусы " +
                        "или приоритеты";
                }

                var task =
                    new WorkTask
                    {
                        Title = title,
                        Description =
                            description,

                        TaskStatus_idTS =
                            firstStatus.idTS,

                        TaskPriority_idTP =
                            firstPriority.idTP,

                        CreatedAtT =
                            DateTime.Now,

                        CreatedById = 1,
                        AssigneeId = 1,
                        Departments_idD = 1
                    };

                _context.WorkTasks
                    .Add(task);

                await _context
                    .SaveChangesAsync();

                return
                    $"✅ Задача создана:\n" +
                    $"{task.idT}. " +
                    $"{task.Title}";
            }
            catch
            {
                return
                    "Ошибка создания задачи";
            }
        }

        private async Task<string>
            CompleteTask(string text)
        {
            try
            {
                var command =
                    text.Replace("/done", "")
                        .Trim();

                if (!int.TryParse(
                    command,
                    out int taskId))
                {
                    return
                        "Формат:\n" +
                        "/done ID";
                }

                var task =
                    await _context
                        .WorkTasks
                        .FirstOrDefaultAsync(t =>
                            t.idT == taskId);

                if (task == null)
                {
                    return
                        "Задача не найдена";
                }

                var completedStatus =
                    await _context
                        .WorkTaskStatuses
                        .FirstOrDefaultAsync(s =>
                            s.NameTS ==
                            "Завершенные");

                if (completedStatus == null)
                {
                    return
                        "Статус " +
                        "'Завершенные' " +
                        "не найден";
                }

                task.TaskStatus_idTS =
                    completedStatus.idTS;

                await _context
                    .SaveChangesAsync();

                return
                    $"✅ Задача завершена:\n" +
                    $"{task.idT}. " +
                    $"{task.Title}";
            }
            catch
            {
                return
                    "Ошибка изменения задачи";
            }
        }
    }
}