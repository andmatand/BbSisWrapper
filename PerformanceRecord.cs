using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public partial class Student {
        public partial class StudentSession {
            public class PerformanceRecord {
                private CEAGradePerformance bbObject;

                public PerformanceRecord(CEAGradePerformance bbSisObject) {
                    bbObject = bbSisObject;
                }

                public CEAGradePerformance BbSisObject {
                    get {
                        return bbObject;
                    }
                }
            }
        }
    }
}