namespace Models
{
    /// <summary>
    /// Представляет рабочую задачу в рамках проекта.
    /// </summary>
    public class WorkTask
    {
        /// <summary>Идентификатор задачи.</summary>
        public int Id { get; set; }

        /// <summary> Наименование задачи.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Флаг активности задачи.</summary>
        public bool IsActive { get; set; }

        /// <summary> Идентификатор связанного проекта.</summary>
        public int ProjectId { get; set; }

        /// <summary> Связанный проект.</summary>
        public Project? Project { get; set; }

        /// <summary> Список записей времени по данной задаче.</summary>
        public List<TimeRecord> TimeRecords { get; set; } = new();


    }
}
