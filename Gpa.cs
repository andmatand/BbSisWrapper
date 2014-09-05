using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbSisWrapper {
    public partial class Student {
        public partial class StudentSession {
            public class Gpa {
                private CEAGradeGPA bbObject;

                public Gpa(CEAGradeGPA bbSisObject) {
                    bbObject = bbSisObject;
                }

                public CEAGradeGPA BbSisObject {
                    get {
                        return bbObject;
                    }
                }
            }
        }
    }
}