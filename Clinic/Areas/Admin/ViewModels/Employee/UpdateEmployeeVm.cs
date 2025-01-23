using System.ComponentModel.DataAnnotations;

namespace Clinic.Areas.Admin.ViewModels
{
    public class UpdateEmployeeVm
    {
        public string? Photo { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Max 100 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Max 100 characters")]
        public string Surname { get; set; }
        public string Profession { get; set; }
        public string Twitter { get; set; }
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public int Order { get; set; }
    }
}
