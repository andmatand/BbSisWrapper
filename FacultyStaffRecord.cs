using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using IBBAddressHeaders = Blackbaud.PIA.FE7.AFNInterfaces.IBBAddressHeaders;
using FILTERTYPE = Blackbaud.PIA.EA7.BBEEAPI7.eDataFilterCustomTypes;
using FIELD = Blackbaud.PIA.EA7.BBEEAPI7.EEAFACULTYFIELDS;

namespace BbSisWrapper {
    public class FacultyStaffRecord : IPerson {
        private CEAFacultyRecord bbRecord;
        private AddressCollection addresses;

        public FacultyStaffRecord(CEAFacultyRecord bbRecord) {
            this.bbRecord = bbRecord;
        }

        public DateTime DateAdded {
            get {
                return DateTime.Parse((string) bbRecord.Fields[FIELD.EAFACULTY_fld_DATEADDED]);
            }
        }

        public string FirstName {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_FIRSTNAME];
            }
        }

        public PersonGender Gender {
            get {
                switch ((string) bbRecord.Fields[FIELD.EAFACULTY_fld_GENDER]) {
                    case "Male":
                        return PersonGender.Male;
                    case "Female":
                        return PersonGender.Female;
                    default:
                        return PersonGender.Null;
                }
            }
        }

        public string Nickname {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_NICKNAME];
            }
        }

        public string IdNumber {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_USERDEFINEDID];
            }
        }

        public string LastName {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_LASTNAME];
            }
        }

        public string MiddleName {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_MIDDLENAME];
            }
        }

        public bool IsCurrentTeacher {
            get {
                return ((bbTF) Enum.Parse(typeof(bbTF),
                    (string) bbRecord.Fields[FIELD.EAFACULTY_fld_ISCURRENTTEACHER]) ==
                    bbTF.bbTrue);
            }
            set {
                bbRecord.Fields[FIELD.EAFACULTY_fld_ISCURRENTTEACHER] = value;
            }
        }

        public string OnlinePassword {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_ONLINEPASSWORD];
            }
            set {
                bbRecord.Fields[FIELD.EAFACULTY_fld_ONLINEPASSWORD] = value;
            }
        }

        public string OnlineUserId {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_ONLINEUSERID];
            }
            set {
                bbRecord.Fields[FIELD.EAFACULTY_fld_ONLINEUSERID] = value;
            }
        }

        public string Status {
            get {
                return (string) bbRecord.Fields[FIELD.EAFACULTY_fld_STATUS];
            }
        }

        public AddressCollection Addresses {
            get {
                if (addresses == null) {
                    addresses = new AddressCollection((IBBAddressHeaders) bbRecord.Address);
                }

                return addresses;
            }
        }

        public void Reload() {
            throw new NotImplementedException();
        }

        public void Save() {
            bbRecord.Save();
        }

        public void Close() {
            if (bbRecord == null) return;

            // If we have addresses loaded
            if (addresses != null) {
                // Clear our list of addresses
                addresses = null;
            }

            // Release our handle on the SIS record
            bbRecord.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbRecord);
            bbRecord = null;
        }

        ~FacultyStaffRecord() {
            Close();
        }

        public static FacultyStaffRecord
        LoadByEA7FacultyId(int ea7FacultyId, IBBSessionContext context) {
            var record = new CEAFacultyRecord();
            record.Init(context);
            record.Load(ea7FacultyId);
            return new FacultyStaffRecord(record);
        }

        public static FacultyStaffRecord
        LoadByEA7RecordsId(int ea7RecordsId, IBBSessionContext context) {
            var records = new CEAFacultyRecords();
            records.Init(context);
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMFROM] =
                "EA7FACULTY " +
                "join EA7RECORDS " +
                "    on EA7RECORDS.EA7RECORDSID = EA7FACULTY.EA7RECORDSID";
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                "EA7RECORDS.EA7RECORDSID = " + ea7RecordsId.ToString();

            int matchId = -1;

            // If there was exactly one matching record
            if (records.Count() == 1) {
                // Store the matching record's ID
                matchId = int.Parse((string)
                    records.Item(1).Fields[FIELD.EAFACULTY_fld_EA7FACULTYID]);
            }

            // Release our handle on the record collection
            records.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(records);
            records = null;

            // If we found a matching ID to load
            if (matchId != -1) {
                return LoadByEA7FacultyId(matchId, context);
            }
            else {
                return null;
            }
        }

        public static FacultyStaffRecord
        LoadByUserDefinedId(string ncid, IBBSessionContext context) {
            ncid = ncid.Replace("'", "''").Trim();

            var records = new CEAFacultyRecords();
            records.Init(context);
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMFROM] =
                "EA7FACULTY " +
                "join EA7RECORDS " +
                "    on EA7RECORDS.EA7RECORDSID = EA7FACULTY.EA7RECORDSID";
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                "EA7RECORDS.USERDEFINEDID = '" + ncid + "'";
            
            int matchId = -1;

            // If there was exactly one matching record
            if (records.Count() == 1) {
                // Store the matching record's ID
                matchId = int.Parse((string)
                    records.Item(1).Fields[FIELD.EAFACULTY_fld_EA7FACULTYID]);
            }

            // Release our handle on the record collection
            records.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(records);
            records = null;

            // If we found a matching ID to load
            if (matchId != -1) {
                return LoadByEA7FacultyId(matchId, context);
            }
            else {
                return null;
            }
        }

        public static IEnumerable<FacultyStaffRecord>
        LoadCollection(
            Context context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            return LoadCollection(context.BbSisContext, sqlFrom, sqlWhere, sqlOrderBy);
        }

        internal static IEnumerable<FacultyStaffRecord>
        LoadCollection(
            IBBSessionContext context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            CEAFacultyRecords bbCollection = new CEAFacultyRecords();
            bbCollection.Init(context, true);

            if (sqlFrom != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMFROM] = sqlFrom;
            }
            if (sqlWhere != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMWHERE] = sqlWhere;
            }
            if (sqlOrderBy != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMORDERBY] = sqlOrderBy;
            }

            foreach (CEAFacultyRecord bbObject in bbCollection) {
                yield return new FacultyStaffRecord(bbObject);
            }

            bbCollection.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbCollection);
            bbCollection = null;
        }
    }
}