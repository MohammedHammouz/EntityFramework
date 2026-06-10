using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Entities
{
    public partial class Student
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string? PhoneNumber { get; set; }
        public string Status { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrolments { get; set; } = new List<Enrollment>();
        public virtual StudentProfile? StudentProfile { get; set; }
    }
}
