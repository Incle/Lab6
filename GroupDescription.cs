using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;




//
namespace Lab6
{
    //
    public class GroupDescription
    {
        public Int32 Id { get; set; }
        public String Department { get; set; }
        public String Code { get; set; }
        public IEnumerable<StudentDescription> Students { get; set; }
    }
}
