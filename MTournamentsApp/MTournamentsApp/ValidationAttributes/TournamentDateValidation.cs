using System.ComponentModel.DataAnnotations;

namespace MTournamentsApp.ValidationAttributes
{
    public class TournamentDateValidation : RangeAttribute
    {
        public TournamentDateValidation() : base(typeof(DateTime), DateTime.Today.AddYears(-10).ToString(), DateTime.Today.AddYears(10).ToString())
        {
            ErrorMessage = "Please enter a valid event date";
        }
    }
}
