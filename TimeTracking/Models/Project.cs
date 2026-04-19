namespace Models
{
    /// <summary>
    /// Представляет проект в системе.
    /// </summary>
    public class Project
    {
        /// <summary> Идентификатор номер.</summary>
        public int Id { get; set; }

        /// <summary> Имя проекта.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary> Уникальный код проекта.</summary>
        public string Code { get; set; } = string.Empty;

        /// <summary> Флаг активности проекта.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary> Список задач, связанных с проектом.</summary>
        public List<WorkTask> Tasks { get; set; } = new();
    }
}
