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
        public MyTypeDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(Student)))
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
                td = new MyCustomTypeDescriptor(td);
            }

            //Set descriptor owner
            ((MyCustomTypeDescriptor)td).SetOwnerInstance(instance);
             
            return td;
        }
    }

    public class MyPropertyDesciptor : PropertyDescriptor
    {
        private FieldInfo _fi;
        public MyPropertyDesciptor(FieldInfo fi, Attribute[] attributes)
            : base(fi.Name, attributes)
        {
            _fi = fi;
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override void ResetValue(object component) { }

        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        { return true; }
        public override Type ComponentType { get { return typeof(Student); } }
        public override Type PropertyType { get { return typeof(Student); } }

        public override object GetValue(object component)
        {
            return _fi.GetValue(component);
        }
        public override void SetValue(object component, object value)
        {
            //_subPD.SetValue(_parentPD.GetValue(component), value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

    public class MyCustomTypeDescriptor : CustomTypeDescriptor
    {
        public MyCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {
        }
        public override PropertyDescriptorCollection GetProperties()
        {
            var cols = base.GetProperties();
            var studentPD = cols["StudentInfo"];
            var studentPDChildProperties = studentPD.GetChildProperties();
            PropertyDescriptor[] array = new PropertyDescriptor[2];
            array[0] = new SubPropertyDescriptor(studentPD, studentPDChildProperties["Name"], "Student Name");
            array[1] = cols["Score"];
            var newcols = new PropertyDescriptorCollection(array);
            return newcols;
        }
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            var props = new PropertyDescriptorCollection(null);
            foreach (FieldInfo fi in typeof(Student).GetFields())
            {
                props.Add(new MyPropertyDesciptor(fi, attributes));
            }
            // Return the computed properties
            return props;
        }

        public void SetOwnerInstance(object instance)
        {
            
            
        }
    }

    public class SubPropertyDescriptor : PropertyDescriptor
    {
        private PropertyDescriptor _subPD;
        private PropertyDescriptor _parentPD;
        public SubPropertyDescriptor(PropertyDescriptor parentPD, PropertyDescriptor subPD, string pdname)
            : base(pdname, null)
        {
            _subPD = subPD;
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
        public override Type ComponentType { get { return _parentPD.ComponentType; } }
        public override Type PropertyType { get { return _subPD.PropertyType; } }

        public override object GetValue(object component)
        {
            return _subPD.GetValue(_parentPD.GetValue(component));
        }
        public override void SetValue(object component, object value)
        {
            _subPD.SetValue(_parentPD.GetValue(component), value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }
}
