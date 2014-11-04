using System;
using System.Collections.Generic;
using Blackbaud.PIA.FE7.AFNInterfaces;
using PYEmployeesData7;

namespace BbSisWrapper {
    public class Employee : IPerson {
        private CPYEmployee bbRecord;
        private AddressCollection addresses;
        private AttributeCollection attributes;

        public Employee(CPYEmployee bbRecord) {
            this.bbRecord = bbRecord;
        }

        public AttributeCollection Attributes {
            get {
                if (attributes == null) {
                    attributes = new AttributeCollection(bbRecord.Attributes);
                }

                return attributes;
            }
        }

        public bool CanBeSaved {
            get { return bbRecord.CanBeSaved(); }
        }

        public ReasonRecordCannotBeSaved ReasonRecordCannotBeSaved {
            get {
                bbCantSaveReasons bbCantSaveReason = bbCantSaveReasons.csrObjectVersionOutOfDate;
                string message = null;

                if (!bbRecord.CanBeSaved(ref bbCantSaveReason, ref message)) {
                    return new ReasonRecordCannotBeSaved(
                        (Blackbaud.PIA.EA7.BBEEAPI7.bbCantSaveReasons) bbCantSaveReason,
                        message);
                }
                else {
                    return null;
                }
            }
        }

        public DateTime DateAdded {
            get {
                return DateTime.Parse((string)
                    bbRecord.Fields[EPYEMPLOYEESFields.PYEMPLOYEES_fld_DATEADDED]);
            }
        }

        public string IdNumber {
            get {
                return (string) bbRecord.Fields[EPYEMPLOYEESFields.PYEMPLOYEES_fld_USERDEFINEDID];
            }
        }

        public int Py7EmployeesId {
            get {
                return int.Parse((string)
                    bbRecord.Fields[EPYEMPLOYEESFields.PYEMPLOYEES_fld_PY7EMPLOYEESID]);
            }
        }

        public string FirstName {
            get {
                return (string)
                       bbRecord.IndividualNameObject().Fields[ENAMEFields.NAME_fld_FIRSTNAME];
            }
        }

        public PersonGender Gender {
            get {
                string gender =
                    (string) bbRecord.IndividualNameObject().Fields[ENAMEFields.NAME_fld_GENDER];

                switch (gender) {
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
                return (string)
                       bbRecord.IndividualNameObject().Fields[ENAMEFields.NAME_fld_NICKNAME];
            }
        }

        public string MiddleName {
            get {
                return (string)
                       bbRecord.IndividualNameObject().Fields[ENAMEFields.NAME_fld_MIDDLENAME];
            }
        }

        public string LastName {
            get {
                return (string)
                       bbRecord.IndividualNameObject().Fields[ENAMEFields.NAME_fld_KEYNAME];
            }
        }

        public string OnlineUserId {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string OnlinePassword {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public string SSN {
            get {
                return (string) bbRecord.Fields[EPYEMPLOYEESFields.PYEMPLOYEES_fld_EMPLOYEESSN];
            }
        }

        public AddressCollection Addresses {
            get {
                if (addresses == null) {
                    addresses = new AddressCollection(bbRecord.AddressHeaders);
                }

                return addresses;
            }
        }

        public static IEnumerable<Employee>
        LoadCollection(
            Context context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            return LoadCollection((IBBSessionContext) context.BbSisContext,
                                  sqlFrom,
                                  sqlWhere,
                                  sqlOrderBy);
        }

        internal static IEnumerable<Employee>
        LoadCollection(
            IBBSessionContext context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            CPYEmployees bbCollection = new CPYEmployees();
            bbCollection.Init(context);

            if (sqlFrom != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMFROM] = sqlFrom;
            }
            if (sqlWhere != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMWHERE] = sqlWhere;
            }
            if (sqlOrderBy != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMORDERBY] = sqlOrderBy;
            }

            foreach (CPYEmployee bbObject in bbCollection) {
                yield return new Employee(bbObject);
            }

            bbCollection.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbCollection);
            bbCollection = null;
        }

        public void Close() {
            if (bbRecord == null) return;

            bbRecord.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbRecord);
            bbRecord = null;
        }

        public void Reload() {
            throw new NotImplementedException();
        }

        public void Save() {
            bbRecord.Save();
        }
    }
}