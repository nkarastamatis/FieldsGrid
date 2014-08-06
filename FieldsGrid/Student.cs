using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FieldsGrid
{
    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    [ReadOnly(false)]
    [TypeConverter(typeof(MyObjectShellConverter))]
    class Student
    {
        [ReadOnly(false)]
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
            StudentInfo.Name = Name;
            StudentInfo.Id = "MyID";
            Score = new List<Score>();
            

        }

        public StudentInformation StudentInfo;
        public List<Score> Score;

    }

    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    public class Score
    {
        //[DisplayName("Subject")]
        public string SubjectName;
        //[DisplayName("Result")]
        public string Grade;
    }

    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    public class StudentInformation
    {
        public string Name;
        public string Id;
    }
}
