using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;

namespace FieldsGrid
{
    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    [TypeConverter(typeof(MyObjectShellConverter))]
    public class MyObjectShell
    {
        [ReadOnly(false)]
        public object O;

        public MyObjectShell(object o)
        {
            this.O = o;
        }
    }

    class MyObjectShellConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(MyObjectShell))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is MyObjectShell)
            {

                MyObjectShell myObjectShell = value as MyObjectShell;

                return "Text: " ;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
