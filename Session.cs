using System;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class Session {
        private CEASession bbObject;
        private TermCollection terms;

        public Session(CEASession bbSisObject) {
            this.bbObject = bbSisObject;
        }

        public int Ea7SessionsId {
            get {
                return int.Parse((string)
                    bbObject.Fields[EEASESSIONSFields.EASESSIONS_fld_EA7SESSIONSID]);
            }
        }

        public string Description {
            get {
                return (string) bbObject.Fields[EEASESSIONSFields.EASESSIONS_fld_SESSION];
            }
        }

        public TermCollection Terms {
            get {
                // If the collection of terms has not been loaded yet
                if (terms == null) {
                    terms = new TermCollection(bbObject.Terms);
                }

                return terms;
            }
        }
    }
}
