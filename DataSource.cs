using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab6
{
    public class DataSource
    {
        public IEnumerable<GroupDescription> Groups { get; private set; }
        public IEnumerable<StudentDescription> Students { get; private set; }
        public IEnumerable<AssignmentDescription> Assignments { get; private set; }
        public IEnumerable<ProgressItem> ProgressData { get; set; }

        public DataSource(String groupDescriptionsFilename, String assignmentDescriptionsFilename, String studentsProgressFilename)
        {
            LoadGroupDescriptionsData(groupDescriptionsFilename);
            LoadAssignmentDescriptionsData(assignmentDescriptionsFilename);
            LoadStudentsProgressData(studentsProgressFilename);
            FixupRelationships();
        }

        private void LoadGroupDescriptionsData(String groupDescriptionsFilename)
        {
            StreamReader reader = new StreamReader(groupDescriptionsFilename);
            List<GroupDescription> groups = new List<GroupDescription>();

            String line = reader.ReadLine();
            while (!String.IsNullOrWhiteSpace(line))
            {
                String[] tokens = line.Split(new Char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(x => x.Trim())
                                      .ToArray();

                GroupDescription group = new GroupDescription()
                {
                    Id         = Int32.Parse(tokens[0]),
                    Department = tokens[1],
                    Code       = tokens[2]
                };
                groups.Add(group);

                line = reader.ReadLine();
            }

            this.Groups = groups;
        }

        private void LoadAssignmentDescriptionsData(String assignmentDescriptionsFilename)
        {
            StreamReader reader = new StreamReader(assignmentDescriptionsFilename);
            List<AssignmentDescription> assignments = new List<AssignmentDescription>();

            String line = reader.ReadLine();
            while (!String.IsNullOrWhiteSpace(line))
            {
                Int32 index = line.IndexOf('\t');
                AssignmentDescription assignment = new AssignmentDescription()
                {
                    Id    = Int32.Parse(line.Substring(0, index).Trim()),
                    Topic = line.Substring(index + 1, line.Length - index - 1).Trim()
                };
                assignments.Add(assignment);

                line = reader.ReadLine();
            }

            this.Assignments = assignments;
        }

        private void LoadStudentsProgressData(String studentsProgressFilename)
        {
            StreamReader reader = new StreamReader(studentsProgressFilename);
            List<StudentDescription> students = new List<StudentDescription>();
            List<ProgressItem> progress = new List<ProgressItem>();

            String line = reader.ReadLine();
            while (!String.IsNullOrWhiteSpace(line))
            {
                String[] tokens = line.Split(new Char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(x => x.Trim())
                                      .ToArray();

                StudentDescription student = new StudentDescription()
                {
                    Id        = Int32.Parse(tokens[0]),
                    LastName  = tokens[1],
                    FirstName = tokens[2],
                    GroupId   = Int32.Parse(tokens[3])
                };
                students.Add(student);




                //
                Int32 assignmentId = 1;
                IEnumerable<ProgressItem> items = tokens.Skip(4)
                                                        .Select(t => new ProgressItem()
                                                            {
                                                                AssignmentId = assignmentId++,
                                                                StudentId = student.Id,
                                                                Result = ParseAssignmentResult(t)
                                                            });
                progress.AddRange(items);
                
                line = reader.ReadLine();
            }

            Int32 progressItemId = 1;
            progress.ForEach(x => x.Id = progressItemId++);

            this.Students = students;
            this.ProgressData = progress;
        }


        //
        private AssignmentResult ParseAssignmentResult(String result)
        {
            switch (result)
            {
                case "-":
                    return AssignmentResult.None;
                case "*":
                    return AssignmentResult.WorkInProgress;
                case "+":
                    return AssignmentResult.Done;
                default:
                    throw new FormatException();
            }
        }

        private void FixupRelationships()
        {
            foreach (GroupDescription item in this.Groups)
            {
                GroupDescription group = item;

                group.Students = this.Students.Where(x => x.GroupId == group.Id);
            }

            foreach (StudentDescription item in this.Students)
            {
                StudentDescription student = item;

                student.Group = this.Groups.Single(x => x.Id == student.GroupId);

                IEnumerable<ProgressItem> progressItems = this.ProgressData.Where(x => x.StudentId == student.Id);
                student.AllAssignments = progressItems.Select(x => x.Assignment)
                                                                 .Distinct();
                student.NoProgressAssignments = progressItems.Where(x => x.Result == AssignmentResult.None)
                                                                 .Select(x => x.Assignment);
                student.WorkInProgressAssignments = progressItems.Where(x => x.Result == AssignmentResult.WorkInProgress)
                                                                 .Select(x => x.Assignment);
                student.DoneAssignments = progressItems.Where(x => x.Result == AssignmentResult.Done)
                                                                 .Select(x => x.Assignment);
            }

            foreach (AssignmentDescription item in this.Assignments)
            {
                AssignmentDescription assignment = item;

                IEnumerable<ProgressItem> progressItems = this.ProgressData.Where(x => x.AssignmentId == assignment.Id);
                assignment.AllStudents = progressItems.Select(x => x.Student)
                                                                 .Distinct();
                assignment.NoProgressStudents = progressItems.Where(x => x.Result == AssignmentResult.None)
                                                                 .Select(x => x.Student);
                assignment.WorkInProgressStudents = progressItems.Where(x => x.Result == AssignmentResult.WorkInProgress)
                                                                 .Select(x => x.Student);
                assignment.DoneStudents = progressItems.Where(x => x.Result == AssignmentResult.Done)
                                                                 .Select(x => x.Student);
            }

            //Fixed ProgressItem relationships in ProgressData collection
            foreach (ProgressItem item in ProgressData) 
            {
                ProgressItem progressItem = item;
                progressItem.Student = this.Students.Single(x => x.Id == progressItem.StudentId);
                progressItem.Assignment = this.Assignments.Single(x => x.Id == progressItem.AssignmentId);

                    
            }
        }

    }
}
