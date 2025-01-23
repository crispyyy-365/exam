using System.Runtime.CompilerServices;

namespace Clinic.Areas.Admin.ViewModels
{
    public class GetEmployeeVm
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Profession { get; set; }
        public int Order { get; set; }
    }
}
