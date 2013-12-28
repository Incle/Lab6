using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab6
{
    public class ProgressItem
    {
        public Int32 Id { get; set; }
        public Int32 StudentId { get; set; }
        public Int32 AssignmentId { get; set; }
        public AssignmentResult Result { get; set; }

        public StudentDescription Student { get; set; }
        public AssignmentDescription Assignment { get; set; }
    }
}
