using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace FieldsGrid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var s = new Student();
            var fields = typeof(Student).GetFields();
            var obj = new MyObjectShell(s);
            propertyGrid1.SelectedObject = obj;

            var sInfo = new YourFieldsClass();
            sInfo.StudentInfo = new StudentInformation();
           // propertyGrid1.SelectedObject = sInfo;
        }
    }
}
