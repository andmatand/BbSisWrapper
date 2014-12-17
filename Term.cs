using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class Term {
        private CEATerm bbObject;

        internal Term(CEATerm bbSisObject) {
            this.bbObject = bbSisObject;
        }

        public int Ea7TermsId {
            get {
                return int.Parse((string) bbObject.Fields[EEATERMSFields.EATERMS_fld_EA7TERMSID]);
            }
        }

        public string Description {
            get {
                return (string) bbObject.Fields[EEATERMSFields.EATERMS_fld_TERM];
            }
        }
    }
}
