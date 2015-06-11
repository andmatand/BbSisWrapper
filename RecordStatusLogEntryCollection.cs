using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class RecordStatusLogEntryCollection : ICollection<RecordStatusLogEntry> {
        private cEARecordStatusLogs bbCollection;
        private List<RecordStatusLogEntry> wrapperCollection;

        public RecordStatusLogEntryCollection(cEARecordStatusLogs bbSisCollection) {
            this.bbCollection = bbSisCollection;

            wrapperCollection = new List<RecordStatusLogEntry>();
            foreach (cEARecordStatusLog bbObject in bbCollection) {
                wrapperCollection.Add(new RecordStatusLogEntry(bbObject));
            }
        }

        public void Add(RecordStatusLogEntry item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(RecordStatusLogEntry item) {
            throw new NotImplementedException();
        }

        public void CopyTo(RecordStatusLogEntry[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(RecordStatusLogEntry item) {
            throw new NotImplementedException();
        }

        public IEnumerator<RecordStatusLogEntry> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}