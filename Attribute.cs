using Blackbaud.PIA.FE7.AFNInterfaces;

namespace BbSisWrapper {
    public class Attribute {
        private IBBAttribute bbRecord;

        public Attribute(IBBAttribute bbRecord) {
            this.bbRecord = bbRecord;
        }

        public IBBAttribute SisObject {
            get {
                return bbRecord;
            }
        }

        public string Description {
            get {
                return (string) bbRecord.Fields[EattributeFields.Attribute_fld_VALUE];
            }
        }

        public string Type {
            get {
                return (string) bbRecord.Fields[EattributeFields.Attribute_fld_ATTRIBUTETYPENAME];
            }
        }
    }
}