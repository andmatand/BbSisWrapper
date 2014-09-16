using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class ReasonRecordCannotBeSaved {
        public enum ReasonCategory {
            None,
            OpenedByAnotherUser,
            Security,
            ReadOnlyMode,
            VersionOutOfDate
        }

        private string message;
        private bbCantSaveReasons bbReason;

        internal ReasonRecordCannotBeSaved(bbCantSaveReasons bbReason, string message) {
            this.bbReason = bbReason;
            this.message = message;
        }

        private ReasonCategory ConvertCategory(bbCantSaveReasons bbReason) {
            switch (bbReason) {
                case bbCantSaveReasons.csrRecordIsLocked:
                    return ReasonCategory.OpenedByAnotherUser;
                case bbCantSaveReasons.csrObjectInReadOnlyMode:
                    return ReasonCategory.ReadOnlyMode;
                case bbCantSaveReasons.csrSecurity:
                    return ReasonCategory.Security;
                case bbCantSaveReasons.csrObjectVersionOutOfDate:
                    return ReasonCategory.VersionOutOfDate;
            }

            return ReasonCategory.None;
        }

        public ReasonCategory Category {
            get {
                return ConvertCategory(bbReason);
            }
        }

        public string Message {
            get {
                return message;
            }
        }
    }
}