using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.CustomAtributos
{
    using System.ComponentModel;
    using System.Reflection;


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class BindingInfoAttribute : Attribute
    {

        private bool visible;
        private int sortIndex;

        public BindingInfoAttribute()
        {
            this.visible = true;
        }

        public BindingInfoAttribute(bool visible)
        {
            this.visible = visible;
        }

   
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

       
        public int SortIndex
        {
            get { return sortIndex; }
            set { sortIndex = value; }
        }
    }



    public class SortablePropertyDescriptor : IComparable
    {
        public SortablePropertyDescriptor(PropertyDescriptor descriptor, int sortIndex)
        {
            this.descriptor = descriptor;
            this.sortIndex = sortIndex;
        }

        private PropertyDescriptor descriptor;
        public PropertyDescriptor Descriptor
        {
            get { return descriptor; }
        }

        private int sortIndex;
        public int SortIndex
        {
            get { return sortIndex; }
        }

        public int CompareTo(object obj)
        {
            SortablePropertyDescriptor other = obj as SortablePropertyDescriptor;
            if (other == null) throw new ArgumentException(
              "The given object is not of type SortablePropertyDescriptor.", "obj");
            return this.SortIndex - other.SortIndex;
        }
    }


    public class PropertyDescriptorCollectionHelper
    {
        public PropertyDescriptorCollectionHelper()
        {
            cache = new Dictionary<Type, PropertyDescriptorCollection>();
        }

        private static PropertyDescriptorCollectionHelper current;
        public static PropertyDescriptorCollectionHelper Current
        {
            get
            {
                if (current == null)
                    current = new PropertyDescriptorCollectionHelper();
                return current;
            }
            set { current = value; }
        }

        private Dictionary<Type, PropertyDescriptorCollection> cache;

        public PropertyDescriptorCollection GetPropertyCollection(Type type,
          bool doSort)
        {
            return GetPropertyCollection(type, doSort, false);
        }

        public PropertyDescriptorCollection GetPropertyCollection(Type type)
        {
            return GetPropertyCollection(type, true, false);
        }

        public PropertyDescriptorCollection GetPropertyCollection(Type type,
          bool doSort, bool skipCache)
        {
            PropertyDescriptorCollection result = skipCache ? null :
              cache.ContainsKey(type) ? cache[type] : null;

            if (result == null)
            {
                PropertyDescriptorCollection origProperties =
                  TypeDescriptor.GetProperties(type);
                List<PropertyDescriptor> resultList = doSort ?
                  null : new List<PropertyDescriptor>();
                List<SortablePropertyDescriptor> sortList = doSort ?
                  new List<SortablePropertyDescriptor>() : null;

                foreach (PropertyDescriptor descriptor in origProperties)
                {
                    BindingInfoAttribute bindingInfo = descriptor.Attributes[
                      typeof(BindingInfoAttribute)] as BindingInfoAttribute;
                    if (bindingInfo == null || bindingInfo.Visible)
                    {
                        if (doSort)
                            sortList.Add(new SortablePropertyDescriptor(descriptor,
                              bindingInfo != null ? bindingInfo.SortIndex : int.MaxValue));
                        else
                            resultList.Add(descriptor);
                    }
                }

                if (doSort)
                {
                    sortList.Sort();
                    resultList = ReduceSortableList(sortList);
                }

                result = new PropertyDescriptorCollection(resultList.ToArray());

                cache[type] = result;
            }

            return result;
        }

        private List<PropertyDescriptor> ReduceSortableList(
          List<SortablePropertyDescriptor> sortList)
        {
            List<PropertyDescriptor> resultList = new List<PropertyDescriptor>();
            foreach (SortablePropertyDescriptor desc in sortList)
                resultList.Add(desc.Descriptor);
            return resultList;
        }
    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly PropertyInfo nameProperty;




        public LocalizedDisplayNameAttribute(string displayNameKey, Type resourceType = null)
            : base(displayNameKey)
        {


            if (resourceType != null)
            {
                nameProperty = resourceType.GetProperty(base.DisplayName, BindingFlags.Static | BindingFlags.Public);

            }

        }

        public override string DisplayName
        {
            get
            {
                if (nameProperty == null)
                {
                    return base.DisplayName;
                }

                return (string)nameProperty.GetValue(nameProperty.DeclaringType, null);
            }
        }


    }


    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class DisplayIndexAttribute : Attribute
    {
        private int index;

        public DisplayIndexAttribute(int index)
        {
            this.index = index;
        }

        public int Index
        {
            get { return this.index; }
        }
    } 


}
