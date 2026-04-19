    namespace DTOs
    {
    /// <summary>
    /// Данные проекта для передачи через API.
    /// </summary>
    public class ProjectDto
        {
            /// <summary>Наименование проекта.</summary>
            public string Name { get; set; } = string.Empty;

            /// <summary>Код проекта.</summary>
            public string Code { get; set; } = string.Empty;

            /// <summary>Флаг активности.</summary>
            public bool IsActive { get; set; } = true;
        }
    }
