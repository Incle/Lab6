using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab6
{
    public class StudentDescription
    {
        public Int32 Id { get; set; }
        public Int32 GroupId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

        public GroupDescription Group { get; set; }
        public IEnumerable<AssignmentDescription> AllAssignments { get; set; }
        public IEnumerable<AssignmentDescription> NoProgressAssignments { get; set; }
        public IEnumerable<AssignmentDescription> WorkInProgressAssignments { get; set; }
        public IEnumerable<AssignmentDescription> DoneAssignments { get; set; }
    }
}
