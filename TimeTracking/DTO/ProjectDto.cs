    namespace DTOs
    {
    /// <summary>
    /// Представляет собой объект передачи данных для проекта, 
    /// содержащий основную информацию о проекте,
    /// </summary>
    public class ProjectDto
        {
            public string Name { get; set; } = string.Empty;
            public string Code { get; set; } = string.Empty;
            public bool IsActive { get; set; } = true;
        }
    }
