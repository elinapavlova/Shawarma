using System.ComponentModel.DataAnnotations;

namespace Models.Account
{
    public enum ActualShawarmaEnum
    {
        [Display(Name ="Да")]
        True = 1,
        [Display(Name = "Нет")]
        False = 0
    }
}