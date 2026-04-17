namespace Models
{
    /// <summary>
    /// Представляет список проектов.
    /// </summary>
    public class Project
    {
        public int Id { get; set; }

        /// <summary>
        /// Имя проекта.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Код проекта.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Получает или задает значения, обозначая объект активным.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Получает или задает список задач.
        /// </summary>
        public List<WorkTask> Tasks { get; set; } = new();
    }
}
