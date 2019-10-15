using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLITOP {
    class AplitopProperties {
        public const Environment environment = Environment.DEVELOPMENT;

        public bool isDevelopment() {
            if (environment.Equals(Environment.DEVELOPMENT)) {
                return true;
            }
            return false;
        }
    }

    enum Environment {
        DEVELOPMENT,
        RELEASE
    }
}
