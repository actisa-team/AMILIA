using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace engNet.Extension.Enumeraciones
{

    using System.ComponentModel;
    
    
    public static  class oExtensionEnumeraciones
   {

       public static string descriptionEnum <T> (this object enumValue) where T : struct
        {
	        var type = enumValue.GetType();
	        if (!type.IsEnum) 
		        throw new ArgumentException("enumValue must be an Enum type", "enumValue");

	        //Return the attribute string if found
	        var member = type.GetMember(enumValue.ToString());
	        if (member != null && member.Length > 0)
	        {
		        var attributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

		        if (attributes != null && attributes.Length > 0)
			        return ((DescriptionAttribute)attributes[0]).Description;
	        }
	
	        //else return the enum name
	        return enumValue.ToString();
        }


   }
}
