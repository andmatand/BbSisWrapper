using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbSisWrapper {
    public enum PersonGender {
        Null = -1,
        Male = 1,
        Female = 2
    }

    public static class Enums {

        public static T Parse<T>(dynamic value) {
            return (T) Enum.Parse(typeof(T), value);
        }
    }
}