using Blackbaud.PIA.EA7.BBEEAPI7;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEAAPPLICATIONSENROLLMENTSFields; 
using System;

namespace BbSisWrapper {
    public partial class Student {
        public class Enrollment {
            private cEAAppEnroll bbRecord;

            public Enrollment(cEAAppEnroll bbRecord) {
                this.bbRecord = bbRecord;
            }

            public DateTime? GraduationDate {
                get {
                    DateTime date;

                    if (DateTime.TryParse((string)
                        bbRecord.Fields[FIELDS.EAAPPLICATIONSENROLLMENTS_fld_DATEOFGRADUATION],
                        out date)) {
                        return date;
                    }
                    else {
                        return null;
                    }
                }
            }

            public bool IsActive {
                get {
                    bbTF tf = (bbTF) Enum.Parse(typeof(bbTF), (string)
                        bbRecord.Fields[FIELDS.EAAPPLICATIONSENROLLMENTS_fld_ACTIVEENROLLMENT]);

                    return tf == bbTF.bbTrue;
                }
            }
        }
    }
}