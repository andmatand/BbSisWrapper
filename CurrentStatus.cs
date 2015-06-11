using System;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public partial class Student {
        public class CurrentStatusLogEntry : RecordStatusLogEntry {
            private cEAStudent student;

            internal CurrentStatusLogEntry(
                cEAStudent student,
                cEARecordStatusLog readOnlyObjectFromStatusLogs) :
                base(readOnlyObjectFromStatusLogs)
            {
                this.student = student;
            }

            public override string Status {
                get {
                    return base.Status;
                }
                set {
                    student.Fields[EEASTUDENTSFields.EASTUDENTS_fld_STATUS] = value;
                }
            }

            public override DateTime StatusDate {
                get {
                    return base.StatusDate;
                }
                set {
                    student.Fields[EEASTUDENTSFields.EASTUDENTS_fld_STATUSDATE] = value;
                }
            }
        }
    }
}