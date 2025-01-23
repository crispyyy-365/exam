using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Clinic.ViewModels
{
    public class LoginVm
    {
        [MinLength(6)]
        [MaxLength(256)]
        public string UsernameOrEmail { get; set; }
        [MaxLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
