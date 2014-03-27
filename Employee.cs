using Blackbaud.PIA.FE7.AFNInterfaces;
using PYEmployeesData7;
using System;

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

        public string FirstName {
            get {
                return (string)
                       bbRecord.IndividualNameObject().Fields[ENAMEFields.NAME_fld_FIRSTNAME];
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

        public AddressCollection Addresses {
            get {
                if (addresses == null) {
                    addresses = new AddressCollection(bbRecord.AddressHeaders);
                }

                return addresses;
            }
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