using Blackbaud.PIA.FE7.AFNInterfaces;
using System;
using OPTION_FIELDS = Blackbaud.PIA.FE7.AFNInterfaces.EAddressOptionsFields;

namespace BbSisWrapper {
    public partial class Address {
        private IBBAddressHeader bbAddressHeader;
        private ContactCollection contacts;

        public Address(IBBAddressHeader bbAddressHeader) {
            this.bbAddressHeader = bbAddressHeader;
        }

        public string City {
            get {
                return (string) bbAddressHeader.Address.Fields[EADDRESSFields.ADDRESS_fld_CITY];
            }
        }

        public string Country {
            get {
                return (string) bbAddressHeader.Address.Fields[EADDRESSFields.ADDRESS_fld_COUNTRY];
            }
        }

        public DateTime DateAdded {
            get {
                return DateTime.Parse((string)
                    bbAddressHeader.Fields[EAddressHeaderFields.ADDRESSHEADER_fld_DATEADDED]);
            }
        }

        public string Description {
            get {
                if (ParentRecordType == "EA Record") {
                    return (string) bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_DESCRIPTION];
                }
                else if (ParentRecordType == "PY Employee record") {
                    return (string) bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_PY_DESCRIPTION];
                }
                else {
                    return null;
                }
            }
        }

        public bool IsPrimary {
            get {
                switch (ParentRecordType) {
                    case "EA Record":
                        return ((bbTF) Enum.Parse(typeof(bbTF),
                            (string) bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_PRIMARYADDRESS]) ==
                            bbTF.bbTrue);
                    case "PY Employee record":
                        return ((bbTF) Enum.Parse(typeof(bbTF),
                            (string) bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_PY_PRIMARYADDRESS]) ==
                            bbTF.bbTrue);
                    default:
                        throw new Exception("Parent Record Type is unknown");
                }
            }
            set {
                switch (ParentRecordType) {
                    case "EA Record":
                        bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_PRIMARYADDRESS] = value;
                        break;
                    case "PY Employee record":
                        bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_PY_PRIMARYADDRESS] = value;
                        break;
                    default:
                        throw new Exception("Parent Record Type is unknown");
                }
            }
        }

        public string StreetAddress {
            get {
                return (string) bbAddressHeader.Address.Fields[EADDRESSFields.ADDRESS_fld_ADDRESSBLOCK];
            }
        }

        public string State {
            get {
                return (string) bbAddressHeader.Address.Fields[EADDRESSFields.ADDRESS_fld_STATE];
            }
        }

        public string ParentRecordType {
            get {
                
                return (string)
                    bbAddressHeader.Fields[EAddressHeaderFields.ADDRESSHEADER_fld_PARENTRECORDTYPE];
            }
        }

        public string Type {
            get {
                switch(ParentRecordType) {
                    case "EA Record":
                        return (string)
                            bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_ADDRESSTYPE];
                    case "PY Employee record":
                        return (string)
                            bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_PY_ADDRESSTYPE];
                    default:
                        throw new Exception("Unknown parent record type");
                }
            }
            set {
                switch(ParentRecordType) {
                    case "EA Record":
                        bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_ADDRESSTYPE] = value;
                        break;
                    case "PY Employee record":
                        bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_PY_ADDRESSTYPE] = value;
                        break;
                    default:
                        throw new Exception("Unknown parent record type");
                }
            }
        }

        public DateTime? ValidFrom {
            get {
                string value = (string)
                    bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_VALIDFROM];

                if (value == null) {
                    return null;
                }
                else {
                    return DateTime.Parse(value);
                }
            }
        }

        public DateTime? ValidTo {
            get {
                string value = (string)
                    bbAddressHeader.AddressOptions.Fields[OPTION_FIELDS.ADDRESSOPTIONS_fld_EA_VALIDTO];

                if (value == null) {
                    return null;
                }
                else {
                    return DateTime.Parse(value);
                }
            }
        }

        public string Zip {
            get {
                return (string) bbAddressHeader.Address.Fields[EADDRESSFields.ADDRESS_fld_POSTCODE];
            }
        }

        public ContactCollection Contacts {
            get {
                if (contacts == null) {
                    contacts = new ContactCollection(bbAddressHeader.Phones,
                                                     bbAddressHeader.PhoneHeaders);
                }

                return contacts;
            }
        }

        public bool IsShared {
            get {
                return this.bbAddressHeader.PhoneHeaders.AddressLinked;
            }
        }
    }
}