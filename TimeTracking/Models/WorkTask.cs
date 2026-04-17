namespace Models
{
    /// <summary>
    /// Предаствляет собой единичу работы или задачу, связанную 
    /// с проектом.
    /// </summary>
    /// <remarks>Обычно используется для отслеживания отдельных частей работы в рамках проекта. Он
    /// хранит ссылки на свой родительский проект и любые записи времени, связанные с задачей. Этот класс подходит
    /// для использования в приложениях для управления проектами или учета времени.</remarks>
    public class WorkTask
    {
        
        public int Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Получает или задает значения, обозначая объект активным.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Внешний ключ проекта.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Получается или задает коллекцию проекта.
        /// </summary>
        public Project? Project { get; set; }

        /// <summary>
        /// Получает или задает коллекцию учета времени.
        /// </summary>
        public List<TimeRecord> TimeRecords { get; set; } = new();


    }
}
