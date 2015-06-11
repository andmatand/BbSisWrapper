using System;
using Blackbaud.PIA.EA7.BBEEAPI7;
using FIELD = Blackbaud.PIA.EA7.BBEEAPI7.EEARecordStatusLogsFields;

namespace BbSisWrapper {
    public class RecordStatusLogEntry {
        private cEARecordStatusLog bbObject;

        public RecordStatusLogEntry(cEARecordStatusLog bbSisObject) {
            this.bbObject = bbSisObject;
        }

        public cEARecordStatusLog BbSisObject {
            get {
                return bbObject;
            }
        }

        public virtual DateTime StatusDate {
            get {
                return DateTime.Parse((string)
                    bbObject.Fields[FIELD.EARECORDSTATUSLOGS_fld_STATUSDATE]);
            }
            set {
                throw new NotSupportedException();
            }
        }

        public virtual string Status {
            get {
                return (string) bbObject.Fields[FIELD.EARECORDSTATUSLOGS_fld_STATUS];
            }
            set {
                throw new NotSupportedException();
            }
        }

        public string StatusReason {
            get {
                return (string) bbObject.Fields[FIELD.EARECORDSTATUSLOGS_fld_STATUSREASON];
            }
            set {
                bbObject.Fields[FIELD.EARECORDSTATUSLOGS_fld_STATUSREASON] = value;
            }
        }

        public bool IsCurrent {
            get {
                return
                    Enums.Parse<bbTF>(bbObject.Fields[FIELD.EARECORDSTATUSLOGS_fld_ISCURRENT]) ==
                    bbTF.bbTrue;
            }
        }

        public bool IsOnActiveEnrollment {
            get {
                return
                    Enums.Parse<bbTF>(bbObject.Fields[FIELD.EARECORDSTATUSLOGS_fld_ACTIVE]) ==
                    bbTF.bbTrue;
            }
        }
    }
}