using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEAPromotionSummariesFields;

namespace BbSisWrapper {
    public partial class Student {
        public class ProgressionEntry {
            private cEAPromotionSummary sisObject;

            public ProgressionEntry(cEAPromotionSummary sisObject) {
                this.sisObject = sisObject;
            }

            public cEAPromotionSummary BbSisObject {
                get { return sisObject; }
            }

            public int SchoolsId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_SCHOOLSID]);
                }
                set {
                    sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_SCHOOLSID] = value;
                }
            }

            public int AcademicYearId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_EA7ACADEMICYEARSID]);
                }
                set {
                    sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_EA7ACADEMICYEARSID] = value;
                }
            }

            public int AdvisorId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_ADVISORID]);
                }
                set {
                    sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_ADVISORID] = value;
                }
            }

            public string AdvisorName {
                get {
                    return (string) sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_ADVISORNAME];
                }
            }

            public string GradeLevel {
                get {
                    return (string) sisObject.Fields[FIELDS.EAPROMOTIONSUMMARIES_fld_GRADELEVEL];
                }
            }
        }
    }
}
