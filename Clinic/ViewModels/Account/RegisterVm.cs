using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Clinic.ViewModels
{
    public class RegisterVm
    {
        [MinLength(6)]
        [MaxLength(256)]
        public string Username { get; set; }
        [MinLength(6)]
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
