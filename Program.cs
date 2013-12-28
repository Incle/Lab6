using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;






namespace Lab6
{
    class Program
    {
        public static void Main(String[] args)
        {
            DataSource data = new DataSource("groups.tsv", "assignments.tsv", "progress.tsv");
            DataProcessor.PrintStudentsStatistics(data);
            DataProcessor.PrintAssignmentStatistics(data);
            DataProcessor.PrintGroupStatistics(data);

            DataProcessor.PrintStudentsWithAllAssignmentsDone(data);
           
            DataProcessor.PrintStudents31WithAllAssignmentsDone(data);
        }
    }
}
