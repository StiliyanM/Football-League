namespace FootballLeague.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        public int Points { get; set; }

        public int Won { get; set; }

        public int Draw { get; set; }

        public int Lost { get; set; }

        public ICollection<Match> HomeMatches { get; set; }

        public ICollection<Match> AwayMatches { get; set; }
    }
}
