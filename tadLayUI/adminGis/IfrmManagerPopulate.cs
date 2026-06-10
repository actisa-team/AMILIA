using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tadLayUI.adminGis
{
   public interface IfrmManagerPopulate
    {
       bool allowClosed();
       void populate();
    }
}
