using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEATRANSLATIONFIELDS;

namespace BbSisWrapper {
    public class TranslationTable : ITopLevelObject {
        private cEATranslation sisObject;
        private List<Entry> entries;

        public TranslationTable(cEATranslation sisObject) {
            this.sisObject = sisObject;
        }

        ~TranslationTable() {
            Close();
        }

        public void Close() {
            sisObject.CloseDown();
        }

        public void Reload() {
            throw new NotImplementedException();
        }

        public void Save() {
            sisObject.Save();
        }

        public string Name {
            get {
                return (string) sisObject.Fields[FIELDS.EATRANSLATIONS_fld_DESCRIPTION];
            }
        }

        public bool IsActive {
            get {
                return
                    (bbTF) Enum.Parse(typeof(bbTF),
                        (string) sisObject.Fields[FIELDS.EATRANSLATIONS_fld_INACTIVE]) ==
                        bbTF.bbFalse;
            }
        }

        public List<Entry> Entries {
            get {
                LoadEntries();
                return entries;
            }
        }

        private void LoadEntries() {
            // If we have not loaded our entries yet
            if (this.entries == null) {
                this.entries = new List<Entry>();

                // Load each of our entries into an Entry object
                foreach (cEATranslationEntry entry in sisObject.Entries) {
                    entries.Add(new Entry(entry));
                }
            }
        }

        private static cEATranslation LoadSisRecord(int ea7TranslationsId, IBBSessionContext context) {
            var record = new cEATranslation();
            record.Init(context);
            record.Load(ea7TranslationsId);

            return record;
        }

        public static TranslationTable LoadById(int ea7TranslationsId, IBBSessionContext context) {
            var sisRecord = LoadSisRecord(ea7TranslationsId, context);
            return new TranslationTable(sisRecord);
        }


        public class Entry {
            private cEATranslationEntry sisObject;

            public Entry(cEATranslationEntry sisObject) {
                this.sisObject = sisObject;
            }

            public string Grade {
                get {
                    return (string)
                        sisObject.Fields[EEATRANSLATIONENTRIESFIELDS.EATRANSLATIONENTRIES_fld_GRADE];
                }
            }
        }
    }
}