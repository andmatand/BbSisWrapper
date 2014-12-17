using System;
using System.Collections.Generic;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class SessionCollection : ICollection<Session> {
        private CEASessions bbCollection;
        private List<Session> wrapperCollection;

        public SessionCollection(CEASessions bbSisCollection) {
            this.bbCollection = bbSisCollection;

            wrapperCollection = new List<Session>();

            // Load each CEASession into a Session wrapper object
            foreach (CEASession bbObject in bbCollection) {
                wrapperCollection.Add(new Session(bbObject));
            }
        }

        public void Add(Session item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(Session item) {
            throw new NotImplementedException();
        }

        public void CopyTo(Session[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Session item) {
            throw new NotImplementedException();
        }

        public IEnumerator<Session> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}
