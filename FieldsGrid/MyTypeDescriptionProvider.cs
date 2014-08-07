using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;

namespace FieldsGrid
{
    public class MyTypeDescriptionProvider : TypeDescriptionProvider
    {
        private ICustomTypeDescriptor td;

        public MyTypeDescriptionProvider(MyObjectShell myObj)
            : base(TypeDescriptor.GetProvider(myObj.O.GetType()))
        {
            var p = TypeDescriptor.GetProvider(myObj.O.GetType());
        }

        public MyTypeDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(MyObjectShell)))
        {
            var p = TypeDescriptor.GetProvider(typeof(Student));
        }

        public MyTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent)
        {
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (td == null)
            {
                td = base.GetTypeDescriptor(objectType, instance);
                td = new MyCustomTypeDescriptor(td, instance);
            }

            //Set descriptor owner
            //((MyCustomTypeDescriptor)td).SetOwnerInstance(instance);
             
            return td;
        }
    }

    public class MyPropertyDesciptor : PropertyDescriptor
    {
        private FieldInfo _fi;
        public MyPropertyDesciptor(FieldInfo fi, Attribute[] attributes)
            : base(fi.Name, null)
        {
            _fi = fi;
        }

        public override bool SupportsChangeEvents
        {
            get
            {
                return true;
            }
        }

        public override bool IsReadOnly
        {
            
            get { return false; }
        }

        public override void ResetValue(object component) { }

        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        { return true; }
        public override Type ComponentType 
        {
            get 
            { 
                return typeof(Student); 
            } 
        }
        public override Type PropertyType 
        { 
            get 
            { 
                return _fi.FieldType; 
            } 
        }

        public override object GetValue(object component)
        {
            return _fi.GetValue(component);
        }
        public override void SetValue(object component, object value)
        {
            _fi.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

    public class SubPropertyDescriptor : PropertyDescriptor
    {
        private FieldInfo _fi;
        private PropertyDescriptor _parentPD;
        public SubPropertyDescriptor(PropertyDescriptor parentPD, FieldInfo fi, string pdname)
            : base(pdname, null)
        {
            _fi = fi;
            _parentPD = parentPD;
        }

        public override bool IsReadOnly
        {

            get { return false; }
        }

        public override void ResetValue(object component) { }

        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        { return true; }
        public override Type ComponentType
        {
            get
            {
                return _parentPD.ComponentType;
            }
        }
        public override Type PropertyType
        {
            get
            {
                return _fi.FieldType;
            }
        }

        public override object GetValue(object component)
        {
            var val = _parentPD.GetValue(component);
            return _fi.GetValue(val);
        }
        public override void SetValue(object component, object value)
        {
            //_subPD.SetValue(_parentPD.GetValue(component), value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

    public class MyCustomTypeDescriptor : CustomTypeDescriptor
    {
        private object _instance;
        public MyCustomTypeDescriptor(ICustomTypeDescriptor parent, object instance)
            : base(parent)
        {
            _instance = instance;
        }
        public override PropertyDescriptorCollection GetProperties()
        {
            var cols = base.GetProperties();
            var studentPD = cols["StudentInfo"];
            var studentPDChildProperties = studentPD.GetChildProperties();
            PropertyDescriptor[] array = new PropertyDescriptor[2];
            //array[0] = new SubPropertyDescriptor(studentPD, studentPDChildProperties["Name"], "Student Name");
            array[1] = cols["Score"];
            var newcols = new PropertyDescriptorCollection(array);
            return newcols;
        }
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var cols = base.GetProperties();
            var props = new PropertyDescriptorCollection(null);
            

            foreach (FieldInfo fi in _instance.GetType().GetFields())
            {
                var prop = new MyPropertyDesciptor(fi, attributes);
                props.Add(prop);

                if (fi.FieldType.Namespace == "System.Collections.Generic")
                {
                    Type[] args = fi.FieldType.GetGenericArguments();
                    foreach (Type arg in args)
                        modifyNonSystemTypes(arg);
                }
                else
                {
                    modifyNonSystemTypes(fi.FieldType);
                }
                
            }
            // Return the computed properties
            return props;
        }

        public static void modifyNonSystemTypes(Type type)
        {
            if (type.Namespace != "System")
            {
                TypeDescriptor.AddAttributes(type, new TypeConverterAttribute(typeof(MyObjectShellConverter)));
                TypeDescriptor.AddAttributes(type, new TypeDescriptionProviderAttribute(typeof(MyTypeDescriptionProvider)));
            }
        }

        public void SetOwnerInstance(object instance)
        {
            
            
        }
    }


    
}
