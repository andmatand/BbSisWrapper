using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbSisWrapper {
    public static class Enums {
        public static T Parse<T>(dynamic value) {
            return (T) Enum.Parse(typeof(T), value);
        }
    }
}