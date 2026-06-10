using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ables_vs_Entities___Creating_Entity_Classes__TrainingCenterDB_.Entities
{
    public partial class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public decimal ProgressPercent { get; set; }
        public decimal? FinalGrade { get; set; }
        public string Status { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
