using System;
using System.Collections.Generic;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class TermCollection : ICollection<Term> {
        private CEATerms bbCollection;
        private List<Term> wrapperCollection;

        public TermCollection(CEATerms bbSisCollection) {
            this.bbCollection = bbSisCollection;

            wrapperCollection = new List<Term>();

            // Load each CEATerm into a Term wrapper object
            foreach (CEATerm bbObject in bbCollection) {
                wrapperCollection.Add(new Term(bbObject));
            }
        }

        public void Add(Term item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(Term item) {
            throw new NotImplementedException();
        }

        public void CopyTo(Term[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Term item) {
            throw new NotImplementedException();
        }

        public IEnumerator<Term> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}
