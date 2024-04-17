using System.ComponentModel.DataAnnotations;

namespace MTournamentsApp.Entities
{
    public class Address
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the Street Name (i.e 123 Some St)")]
        public string? StreetAddress { get; set; }

        [Required(ErrorMessage = "Please enter the City (i.e Toronto)")]
        public string? TournamentCity { get; set; }

        [Required(ErrorMessage = "Please enter the Country (i.e Canada)")]
        public string? TournamentCountry { get; set; }

        [Required(ErrorMessage = "Please enter a Postal Code (i.e H0H 0H0)")]
        public string? TournamentPostalCode { get; set; }

        override
        public string ToString()
        {
            return StreetAddress + ", " + TournamentCity + ", " + TournamentCountry + ", " + TournamentPostalCode;
        }
    }
}
