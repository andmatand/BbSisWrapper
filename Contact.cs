using Blackbaud.PIA.FE7.AFNInterfaces;
using System;

namespace BbSisWrapper {
    public partial class Address {
        public class Contact {
            private IBBPhone bbPhone;
            private ContactSharingComponent sharingComponent;

            public Contact(IBBPhoneHeader bbPhoneHeader) {
                bbPhone = bbPhoneHeader.Phone;
                sharingComponent = new ContactSharingComponent(bbPhoneHeader);
            }

            public Contact(IBBPhone bbPhone) {
                this.bbPhone = bbPhone;
            }

            public IBBPhone SisObject {
                get {
                    return bbPhone;
                }
            }

            public ContactSharingComponent SharingComponent {
                get {
                    return sharingComponent;
                }
            }

            public string Number {
                get {
                    return (string) bbPhone.Fields[2];
                }
                set {
                    bbPhone.Fields[2] = value;
                }
            }

            public string Type {
                get {
                    return (string) bbPhone.Fields[3];
                }
                set {
                    bbPhone.Fields[3] = value;
                }
            }


            public class ContactSharingComponent {
                private IBBPhoneHeader bbPhoneHeader;

                public ContactSharingComponent(IBBPhoneHeader bbPhoneHeader) {
                    this.bbPhoneHeader = bbPhoneHeader;
                }

                public IBBPhoneHeader SisObject {
                    get {
                        return bbPhoneHeader;
                    }
                }

                public void DeleteAllLinks() {
                    bbPhoneHeader.DeleteAllLinks();
                }

                public bool CanBeShared {
                    get {
                        return
                            ((bbTF) Enum.Parse(typeof(bbTF),
                             (string) bbPhoneHeader.Fields[EPhoneHeaderFields.PHONEHEADER_fld_ALLOWTOSHARE])
                              == bbTF.bbTrue);
                    }
                    set {
                        bbPhoneHeader.Fields[EPhoneHeaderFields.PHONEHEADER_fld_ALLOWTOSHARE] =
                            (value ? bbTF.bbTrue : bbTF.bbFalse);
                    }
                }
            }
        }
    }
}