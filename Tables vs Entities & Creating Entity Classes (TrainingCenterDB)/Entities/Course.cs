using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Entities
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = null!;
        public string Code { get; set; } = null!;//null! means not null
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string Level { get; set; } = null!;
        public int DurationHours { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string Status { get; set; } = null!;
        public int InstructorId { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
       = new List<Enrollment>();
        public virtual Instructor Instructor { get; set; } = null!;
    }
}
