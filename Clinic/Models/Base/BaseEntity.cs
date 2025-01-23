using Microsoft.AspNetCore.Components.Web;

namespace Clinic.Models.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set; }
        public BaseEntity()
        {
            CreatedBy = "admin";
        }
    }
}
