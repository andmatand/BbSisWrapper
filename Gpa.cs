﻿using Blackbaud.PIA.EA7.BBEEAPI7;

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