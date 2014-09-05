using System;
using System.Collections.Generic;
using Blackbaud.PIA.EA7.BBEEAPI7;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEASTUDENTSESSIONSFields;

namespace BbSisWrapper {
    public partial class Student {
        public partial class StudentSession : IDisposable {
            private CEAStudentSession bbObject;
            private GpaCollection gpas;

            public StudentSession(CEAStudentSession bbSisObject) {
                bbObject = bbSisObject;
            }

            private static CEAStudentSession
            LoadSisRecord(int ea7StudentSessionsId, IBBSessionContext context) {
                var record = new CEAStudentSession();
                record.Init(context);
                record.Load(ea7StudentSessionsId);

                return record;
            }

            public int AcademicYearId {
                get {
                    return int.Parse((string)
                        bbObject.Fields[FIELDS.EASTUDENTSESSIONS_fld_EA7ACADEMICYEARSID]);
                }
            }

            public void Close() {
                if (bbObject != null) {
                    bbObject.CloseDown();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(bbObject);
                    bbObject = null;
                }
            }

            public void Save() {
                bbObject.Save();
            }

            public int SessionId {
                get {
                    return int.Parse((string)
                        bbObject.Fields[FIELDS.EASTUDENTSESSIONS_fld_EA7SESSIONSID]);
                }
            }

            public int Id {
                get {
                    return int.Parse((string)
                        bbObject.Fields[FIELDS.EASTUDENTSESSIONS_fld_EA7STUDENTSESSIONSID]);
                }
            }

            public StudentSession LoadById(int ea7StudentSessionsId, IBBSessionContext context) {
                var record = LoadSisRecord(ea7StudentSessionsId, context);
                return new StudentSession(record);
            }

            public GpaCollection Gpas {
                get {
                    // If we haven't loaded our GPAs yet
                    if (gpas == null) {
                        gpas = new GpaCollection(bbObject.GPAs);
                    }

                    return gpas;
                }
            }

            public void Dispose() {
                Close();
            }
        }
    }
}