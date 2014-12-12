using Blackbaud.PIA.FE7.AFNInterfaces;

namespace BbSisWrapper {
    public class Attribute {
        private IBBAttribute bbRecord;

        public Attribute(IBBAttribute bbRecord) {
            this.bbRecord = bbRecord;
        }

        public IBBAttribute BbSisObject {
            get {
                return bbRecord;
            }
        }

        public string Description {
            get {
                return (string) bbRecord.Fields[EattributeFields.Attribute_fld_VALUE];
            }
            set {
                bbRecord.Fields[EattributeFields.Attribute_fld_VALUE] = value;
            }
        }

        public string Type {
            get {
                return (string) bbRecord.Fields[EattributeFields.Attribute_fld_ATTRIBUTETYPENAME];
            }
        }

        public int TypeId {
            get {
                return int.Parse((string)
                    bbRecord.Fields[EattributeFields.Attribute_fld_ATTRIBUTETYPESID]);
            }
            set {
                bbRecord.Fields[EattributeFields.Attribute_fld_ATTRIBUTETYPESID] = value;
            }
        }
    }
}