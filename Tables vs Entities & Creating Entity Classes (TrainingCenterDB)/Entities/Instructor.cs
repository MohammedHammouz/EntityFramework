using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Entities
{
    public partial class Instructor
    {
        public int InstructorID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly? HireDate { get; set; }
        public float Salary { get; set; }
        public int? ManagerID { get; set; }
        public bool IsActive { get; set; }
        public virtual Instructor? Manager { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        public virtual ICollection<Instructor> InverseManager { get; set; }
        = new List<Instructor>();

    }
}
