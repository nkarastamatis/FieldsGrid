using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FieldsGrid
{
    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    class Student
    {
        public string Name;
        public string Grade;
        public DateTime DOB;

        public Student()
        {
            Name = "MyName";
            Grade = "12";
            DOB = new DateTime(2001, 12, 27);

            StudentInfo = new StudentInformation();
            StudentInfo.Name = Name;
            StudentInfo.Id = "MyID";
            Score = new List<Score>();
            

        }

        public StudentInformation StudentInfo{ get; set; }
        public List<Score> Score{get; set;}

    }

    public class Score
    {
        [DisplayName("Subject")]
        public string SubjectName { get; set; }
        [DisplayName("Result")]
        public string Grade { get; set; }
    }

    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    public class StudentInformation
    {
        public string Name { get; set; }
        public string Id { get; set; }
    }
}
