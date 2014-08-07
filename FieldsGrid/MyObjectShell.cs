using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;

namespace FieldsGrid
{
    [TypeDescriptionProvider(typeof(MyTypeDescriptionProvider))]
    //[TypeConverter(typeof(MyObjectShellConverter))]
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
            if (destinationType == typeof(System.String) && value.GetType().Namespace != typeof(System.String).Namespace)
            {

                MyObjectShell myObjectShell = value as MyObjectShell;

                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            //var cols = base.GetProperties();
            var props = new PropertyDescriptorCollection(null);


            foreach (FieldInfo fi in value.GetType().GetFields())
            {
                var prop = new MyPropertyDesciptor(fi, attributes);
                props.Add(prop);
                if (fi.FieldType.Namespace == "System.Collections.Generic")
                {
                    Type[] args = fi.FieldType.GetGenericArguments();
                    foreach (Type arg in args)
                        MyCustomTypeDescriptor.modifyNonSystemTypes(arg);
                }
                

                {
                    MyCustomTypeDescriptor.modifyNonSystemTypes(fi.FieldType);
                }
            }

            if (props.Count > 0)
                return props;

            return base.GetProperties(context, value, attributes);
        }
    }
}
