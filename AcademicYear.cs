using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEAACADEMICYEARSFields;

namespace BbSisWrapper {
    public class AcademicYear {
        private CEAAcademicYear sisObject;

        public AcademicYear(CEAAcademicYear sisObject) {
            this.sisObject = sisObject;
        }

        ~AcademicYear() {
            Close();
        }

        public void Close() {
            // Release our handle on the SIS record
            this.sisObject.CloseDown();
        }

        public CEAAcademicYear SisObject {
            get {
                return this.sisObject;
            }
        }

        public int Ea7AcademicYearsId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[FIELDS.EAACADEMICYEARS_fld_EA7ACADEMICYEARSID], out id);
                return id;
            }
        }

        public DateTime StartDate {
            get {
                return DateTime.Parse((string) sisObject.Fields[FIELDS.EAACADEMICYEARS_fld_STARTDATE]);
            }
        }

        private static CEAAcademicYear LoadSisRecord(int ea7AcademicYearsId, IBBSessionContext context) {
            var record = new CEAAcademicYear();
            record.Init(context);
            record.Load(ea7AcademicYearsId);

            return record;
        }

        public static AcademicYear LoadById(int ea7AcademicYearsId, IBBSessionContext context) {
            var record = LoadSisRecord(ea7AcademicYearsId, context);
            return new AcademicYear(record);
        }

        public static AcademicYear LoadBySchoolAndDescription(int schoolId, string description, IBBSessionContext context) {
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
    }
}