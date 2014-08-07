using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FieldsGrid
{
    public class Student
    {
        public string Name;
        public string Grade;
        public DateTime DOB;
        public bool YES;

        public Student()
        {
            Name = "MyName";
            Grade = "12";
            DOB = new DateTime(2001, 12, 27);

            StudentInfo = new StudentInformation();
            StudentInfo.Name = "NAME";
            StudentInfo.Id = "MyID";
            Scores = new List<Score>();
            MathTest = new Score();
            Ints = new List<int>();

        }

        public StudentInformation StudentInfo;
        public List<Score> Scores;
        public List<int> Ints;
        public Score MathTest;

    }

    public class Score
    {
        public string SubjectName;
        public string Grade;
    }

    public class StudentInformation
    {
        public string Name;
        public string Id;
    }
}
