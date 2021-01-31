namespace FootballLeague.InputModels
{
    using System.ComponentModel.DataAnnotations;

    public class TeamInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}
