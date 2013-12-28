using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Lab6
{
    public class AssignmentDescription
    {
        public Int32 Id { get; set; }
        public String Topic { get; set; }
        public IEnumerable<StudentDescription> AllStudents { get; set; }
        public IEnumerable<StudentDescription> NoProgressStudents { get; set; }
        public IEnumerable<StudentDescription> WorkInProgressStudents { get; set; }
        public IEnumerable<StudentDescription> DoneStudents { get; set; }
    }
}
