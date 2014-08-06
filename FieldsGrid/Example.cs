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
    [TypeDescriptionProvider(typeof(YourTypeDescriptionProvider))]
    internal class YourFieldsClass
    {
        public int IntField;
        public double DoubleField;
        public StudentInformation StudentInfo;
    }

    internal sealed class FieldPropertyDescriptor<TComponent, TField> : PropertyDescriptor
    {
        private readonly FieldInfo fieldInfo;

        public FieldPropertyDescriptor(string name)
            : base(name, null)
        {
            fieldInfo = typeof(TComponent).GetField(Name);
        }

        public override bool IsReadOnly { get { return false; } }
        public override void ResetValue(object component) { }
        public override bool CanResetValue(object component) { return false; }
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get { return typeof(TComponent); }
        }
        public override Type PropertyType
        {
            get { return typeof(TField); }
        }

        public override object GetValue(object component)
        {
            return fieldInfo.GetValue(component);
        }

        public override void SetValue(object component, object value)
        {
            fieldInfo.SetValue(component, value);
            OnValueChanged(component, EventArgs.Empty);
        }
    }

    internal sealed class YourCustomTypeDescriptor : CustomTypeDescriptor
    {
        public YourCustomTypeDescriptor(ICustomTypeDescriptor parent)
            : base(parent)
        {
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return AddItems(base.GetProperties(),
                new FieldPropertyDescriptor<YourFieldsClass, int>("IntField"),
                new FieldPropertyDescriptor<YourFieldsClass, double>("DoubleField"),
                new FieldPropertyDescriptor<YourFieldsClass, StudentInformation>("StudentInfo"));
        }

        private static PropertyDescriptorCollection AddItems(PropertyDescriptorCollection cols, params PropertyDescriptor[] items)
        {
            PropertyDescriptor[] array = new PropertyDescriptor[cols.Count + items.Length];
            cols.CopyTo(array, 0);
            for (int i = 0; i < items.Length; i++)
                array[cols.Count + i] = items[i];
            PropertyDescriptorCollection newcols = new PropertyDescriptorCollection(array);
            return newcols;
        }
    }

    internal sealed class YourTypeDescriptionProvider : TypeDescriptionProvider
    {
        private ICustomTypeDescriptor td;

        public YourTypeDescriptionProvider()
            : this(TypeDescriptor.GetProvider(typeof(YourFieldsClass)))
        {
        }
        public YourTypeDescriptionProvider(TypeDescriptionProvider parent)
            : base(parent)
        {
        }
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return td ?? (td = new YourCustomTypeDescriptor(base.GetTypeDescriptor(objectType, instance)));
        }
    }
}
