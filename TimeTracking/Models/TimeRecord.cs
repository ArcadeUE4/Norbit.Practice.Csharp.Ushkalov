namespace Models
{
    /// <summary>
    /// Представляет учет времени.
    /// </summary>
    public class TimeRecord
    {
        public int Id { get; set; }

        /// <summary>
        /// Дата учета времени.
        /// </summary>
        public DateTime Date {  get; set; }
        /// <summary>
        /// Часы работы.
        /// </summary>
        public decimal Hours { get; set; }

        /// <summary>
        /// Описание затраченной времени работы.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public int WorkTaskId { get; set; }

        /// <summary>
        /// Получает или задает рабочие задачи.
        /// </summary>
        public WorkTask? WorkTask { get; set; }



    }
}
