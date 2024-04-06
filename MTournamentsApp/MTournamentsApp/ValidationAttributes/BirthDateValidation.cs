using System.ComponentModel.DataAnnotations;

namespace MTournamentsApp.ValidationAttributes
{
    public class BirthDateValidation : RangeAttribute
    {
        public BirthDateValidation() : base(typeof(DateTime), DateTime.Today.AddYears(-200).ToString(), DateTime.Today.Date.ToString())
        {
            ErrorMessage = "Please enter a valid birth date";
        }
    }
}
