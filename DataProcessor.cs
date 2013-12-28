using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab6
{
    public static class DataProcessor
    {
       public static void PrintStudentsStatistics(DataSource data)
        {
 
            var sortedStudents =
            from student in data.Students
            orderby student.DoneAssignments.Count() descending, student.WorkInProgressAssignments.Count() descending
            select student;
           
            Console.WriteLine("Статистика по студентам");
            Console.WriteLine("{0,-15} {1,5} {2,10} {3,7} {4,4}",
                              "Студент",
                              "Группа",
                              "Отсутствует",
                              "В работе",
                              "Сдано");
 
            foreach (var student in sortedStudents)
            {
                Console.WriteLine("{0,-15} {1,6} {2,10}% {3,7}% {4,4}%",
                                  student.LastName + " " + student.FirstName,
                                  student.Group.Department + "-" + student.Group.Code,
                                  (Int32)Math.Round(((float) student.NoProgressAssignments.Count() / student.AllAssignments.Count()) * 100),
                                  (Int32)Math.Round(((float) student.WorkInProgressAssignments.Count() / student.AllAssignments.Count()) * 100),
                                  (Int32)Math.Round(((float) student.DoneAssignments.Count() / student.AllAssignments.Count()) * 100));
 
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        public static void PrintAssignmentStatistics(DataSource data) //n
        {
            Console.WriteLine("Статистика по лабораторным работам");
            for (Int32 i = 1; i < data.Assignments.Count() + 1; ++i)// 6 7
            {
                var assigmentData = data.Assignments.Where(lab => lab.Id == i)
                                            .Select(x => new
                                            {
                                                Lab=data.Assignments.Single(y=> y.Id==i),
                                                NoProgress = (float)x.NoProgressStudents.Count() / data.Students.Count(),
                                                WorkInProgress = (float)x.WorkInProgressStudents.Count() / data.Students.Count(),
                                                DoneProgress = (float)x.DoneStudents.Count() / data.Students.Count()
                                            });
                foreach (var item in assigmentData)
                {
                    Console.WriteLine("{0,-50} {1,4}% {2,4}% {3,4}% ",
                                      item.Lab.Topic,
                                      (Int32)Math.Round(item.NoProgress * 100),
                                      (Int32)Math.Round(item.WorkInProgress * 100),
                                      (Int32)Math.Round(item.DoneProgress * 100));
                }
            }
            Console.WriteLine();
            Console.WriteLine();


        }
        //
        public static void PrintGroupStatistics(DataSource data)
        {
            var studentsData = data.Students.Select(x => new
                                       {
                                           Group = x.Group,
                                           NoProgressRate = (Single)x.NoProgressAssignments.Count() / x.AllAssignments.Count(),
                                           WorkInProgressRate = (Single)x.WorkInProgressAssignments.Count() / x.AllAssignments.Count(),
                                           DoneProgressRate = (Single)x.DoneAssignments.Count() / x.AllAssignments.Count()
                                       });
            var groupsData = studentsData.GroupBy(student => student.Group, (group, students) => new
                                             {
                                                 GroupName = String.Join("-", group.Department, group.Code),
                                                 NoProgressRate = students.Average(student => student.NoProgressRate),
                                                 WorkInProgressRate = students.Average(student => student.WorkInProgressRate),
                                                 DoneProgressRate = students.Average(student => student.DoneProgressRate)
                                             })
                                          .OrderByDescending(x => x.DoneProgressRate)
                                          .ThenByDescending(x => x.WorkInProgressRate);
            //
            Console.WriteLine(new String('=', 34));
            Console.WriteLine("Статистика по группам");
            Console.WriteLine(new String('=', 34));

            Console.WriteLine("{0,-7} {1,-11} {2,-8} {3,-5}",
                              "Группа",
                              "Отсутствует",
                              "В работе",
                              "Сдано");
            Console.WriteLine(new String('-', 34));
            foreach (var item in groupsData)
            {
                Console.WriteLine("{0,-7} {1,10}% {2,7}% {3,4}%",
                                  item.GroupName,
                                  (Int32)Math.Round(item.NoProgressRate * 100),
                                  (Int32)Math.Round(item.WorkInProgressRate * 100),
                                  (Int32)Math.Round(item.DoneProgressRate * 100));
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void PrintStudentsWithAllAssignmentsDone(DataSource data) //n
        {
            List<Int32> students=new List<Int32>();
            foreach (var item in data.Students)
            {                
                var studentData = data.ProgressData.Where(x => (x.Student.Id == item.Id) && (x.Result == AssignmentResult.Done));
                
                if (studentData.Count() == data.Assignments.Count()) 
                {
                    students.Add(item.Id);
                }
                    
            }

            Console.WriteLine("Студенты без задолжностей");
            Console.WriteLine("{0,-15} {1,6}",
                "Студент",
                "Группа");
            foreach (var item in students)
            {
                var studentData= data.Students.Single(x=> x.Id==item);
                Console.WriteLine("{0,-15} {1,6}",
                                  studentData.LastName+" "+studentData.FirstName,
                                  studentData.Group.Department + " " + studentData.Group.Code);
                        
            }

            Console.WriteLine();
            Console.WriteLine();


        }

        public static void PrintStudents31WithAllAssignmentsDone(DataSource data) 
        {
            var bestStudents =
             from student in data.Students
             where student.DoneAssignments.Count() == student.AllAssignments.Count()
             select student;

            Console.WriteLine("Студенты без долгов");
            Console.WriteLine("{0,-15} {1,6}",
                "Студент",
                "Группа");

            foreach (var student in bestStudents)
            {
                Console.WriteLine("{0,-15} {1,6}",
                                  student.LastName + " " + student.FirstName,
                                  student.Group.Department + "-" + student.Group.Code);
            }

            Console.WriteLine();
            Console.WriteLine();
 
        }
    }
}