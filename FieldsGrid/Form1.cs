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
        public Student s = new Student();
        public Form1()
        {
            InitializeComponent();
            MyCustomTypeDescriptor.modifyNonSystemTypes(s.GetType());
            propertyGrid1.SelectedObject = s;
        }

        private void propertyGrid1_Enter(object sender, EventArgs e)
        {
            GridItem gridItem = propertyGrid1.SelectedGridItem;
            if (gridItem == null) 
                return;


        }
    }
}
