using System;
using Blackbaud.PIA.EA7.BBEEAPI7;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEAACADEMICYEARSFields;

namespace BbSisWrapper {
    public class AcademicYear : IDisposable {
        private CEAAcademicYear bbObject;

        public AcademicYear(CEAAcademicYear bbSisObject) {
            this.bbObject = bbSisObject;
        }

        public void Close() {
            if (bbObject != null) {
                // Release our handle on the SIS record
                bbObject.CloseDown();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(bbObject);
                bbObject = null;
            }
        }

        public CEAAcademicYear SisObject {
            get {
                return this.bbObject;
            }
        }

        public string Description {
            get {
                return (string) bbObject.Fields[FIELDS.EAACADEMICYEARS_fld_DESCRIPTION];
            }
        }

        public int Ea7AcademicYearsId {
            get {
                return int.Parse((string) 
                    bbObject.Fields[FIELDS.EAACADEMICYEARS_fld_EA7ACADEMICYEARSID]);
            }
        }

        public DateTime StartDate {
            get {
                return DateTime.Parse((string)
                    bbObject.Fields[FIELDS.EAACADEMICYEARS_fld_STARTDATE]);
            }
        }

        private static CEAAcademicYear
        LoadSisRecord(int ea7AcademicYearsId, IBBSessionContext context)
        {
            var record = new CEAAcademicYear();
            record.Init(context);
            record.Load(ea7AcademicYearsId);

            return record;
        }

        public static AcademicYear LoadById(int ea7AcademicYearsId, Context context) {
            return LoadById(ea7AcademicYearsId, context.BbSisContext);
        }

        private static AcademicYear LoadById(int ea7AcademicYearsId, IBBSessionContext context) {
            var record = LoadSisRecord(ea7AcademicYearsId, context);
            return new AcademicYear(record);
        }

        public static AcademicYear
        LoadBySchoolAndDescription(int schoolId, string description, Context context) {
            return LoadBySchoolAndDescription(schoolId, description, context.BbSisContext);
        }

        private static AcademicYear
        LoadBySchoolAndDescription(int schoolId, string description, IBBSessionContext context) {
            var academicYears = new cEAAcademicYears();
            academicYears.Init(context);
            academicYears.FilterObject.CustomFilterProperty[eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                "EA7ACADEMICYEARS.SCHOOLSID = " + schoolId + " and " +
                "EA7ACADEMICYEARS.DESCRIPTION = '" + description.Replace("'", "''") + "'";

            int ea7AcademicYearsId = -1;

            // If there was exactly 1 matching academic year
            if (academicYears.Count() == 1) {
                int.TryParse((string)
                    academicYears.Item(1).Fields[FIELDS.EAACADEMICYEARS_fld_EA7ACADEMICYEARSID],
                    out ea7AcademicYearsId);
            }
            academicYears.CloseDown();

            // If we found an ea7AcademicYearsId
            if (ea7AcademicYearsId != -1) {
                return LoadById((int) ea7AcademicYearsId, context);
            }
            else {
                return null;
            }
        }

        public void Dispose() {
            Close();
        }
    }
}