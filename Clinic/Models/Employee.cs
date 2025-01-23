using Clinic.Models.Base;

namespace Clinic.Models
{
    public class Employee : BaseEntity
    {
        public string? Image { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Profession { get; set; }
        public string Twitter { get; set; }
        public string FaceBook { get; set; }
        public string Instagram { get; set; }
        public int Order { get; set; }
    }
}
