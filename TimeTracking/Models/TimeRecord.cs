namespace Models
{
    /// <summary>
    /// Представляет запись о затраченном времени.
    /// </summary>
    public class TimeRecord
    {
        /// <summary> Идентификатор записи.</summary>
        public int Id { get; set; }

        /// <summary> Дата выполнения работ.</summary>
        public DateTime Date {  get; set; }

        /// <summary>Количество отработанных часов.</summary>
        public decimal Hours { get; set; }

        /// <summary> Описание выполненной работы.</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary> Идентификатор связанной задачи.</summary>
        public int WorkTaskId { get; set; }

        /// <summary> Связанная рабочая задача.</summary>
        public WorkTask? WorkTask { get; set; }



    }
}
