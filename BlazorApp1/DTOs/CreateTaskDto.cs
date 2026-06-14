namespace BlazorApp1.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public int AssigneeId { get; set; }

        public int TaskPriority_idTP { get; set; }

        public DateTime? DueDate { get; set; }
    }
}