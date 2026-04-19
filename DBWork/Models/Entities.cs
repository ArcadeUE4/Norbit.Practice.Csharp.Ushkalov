namespace GameStoreConsole.Models
{
    public class Genre
    {
        public Guid GenreID { get; set; }
        public string GenreName { get; set; } = string.Empty;
    }

    public class Studio
    {
        public Guid StudioID { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? FoundationDate { get; set; }
        public decimal? AnnualRevenue { get; set; }
        public bool IsIndependent { get; set; }
        public List<Game> Games { get; set; } = new();

        public ICollection<Genre> Genres { get; set; }
    }

    public class Game
    {
        public Guid GameID { get; set; }
        public string? Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? Price { get; set; }
        public bool? IsMultiplayer { get; set; }
        public Guid? StudioID { get; set; }
        public Studio Studio { get; set; }
    }

    public class Player
    {
        public Guid PlayerID { get; set; }
        public string NickName { get; set; } = string.Empty;
        public decimal? Balance { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool? IsBanned { get; set; }
    }
}