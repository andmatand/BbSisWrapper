using System;
using System.Collections.Generic;
using Blackbaud.PIA.FE7.AFNInterfaces;

namespace BbSisWrapper {
    public class AttributeCollection : ICollection<Attribute> {
        private IBBAttributesAPI bbCollection;
        private List<Attribute> wrapperCollection;

        public AttributeCollection(IBBAttributesAPI bbSisCollection) {
            this.bbCollection = bbSisCollection;

            wrapperCollection = new List<Attribute>();

            // Load each IBBAttribute object into a wrapper object
            foreach (IBBAttribute bbRecord in bbSisCollection) {
                wrapperCollection.Add(new Attribute(bbRecord));
            }
        }

        public Attribute Add() {
            IBBAttribute bbObject = bbCollection.Add();
            Attribute newWrappedObject = new Attribute(bbObject);
            wrapperCollection.Add(newWrappedObject);

            return newWrappedObject;
        }

        public bool Remove(Attribute item) {
            int index = wrapperCollection.IndexOf(item);
            bbCollection.Remove(wrapperCollection[index].BbSisObject);
            wrapperCollection.RemoveAt(index);

            return true;
        }

        public void Add(Attribute item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            while (bbCollection.Count() > 0) {
                IBBAttribute firstObject = bbCollection.Item(1);
                bbCollection.Remove(firstObject);
            }

            wrapperCollection.Clear();
        }

        public bool Contains(Attribute item) {
            throw new System.NotImplementedException();
        }

        public void CopyTo(Attribute[] array, int arrayIndex) {
            throw new System.NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new System.NotImplementedException(); }
        }

        public IEnumerator<Attribute> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}