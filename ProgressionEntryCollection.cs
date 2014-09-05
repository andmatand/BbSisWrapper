using System;
using System.Collections.Generic;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class ProgressionEntryCollection : ICollection<Student.ProgressionEntry> {
        private cEAPromotionSummaries bbCollection;
        private List<Student.ProgressionEntry> wrapperCollection;

        public ProgressionEntryCollection(cEAPromotionSummaries bbSisCollection) {
            bbCollection = bbSisCollection;

            wrapperCollection = new List<Student.ProgressionEntry>();

            // Load each of our studentcourses into a StudentCourse object
            foreach (cEAPromotionSummary bbObject in bbCollection) {
                wrapperCollection.Add(new Student.ProgressionEntry(bbObject));
            }
        }

        public Student.ProgressionEntry Add() {
            cEAPromotionSummary bbObject = bbCollection.Add();
            Student.ProgressionEntry newWrappedObject = new Student.ProgressionEntry(bbObject);
            wrapperCollection.Add(newWrappedObject);

            return newWrappedObject;
        }

        public void Add(Student.ProgressionEntry item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new System.NotImplementedException();
        }

        public bool Contains(Student.ProgressionEntry item) {
            return wrapperCollection.Contains(item);
        }

        public void CopyTo(Student.ProgressionEntry[] array, int arrayIndex) {
            wrapperCollection.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new System.NotImplementedException(); }
        }

        public bool Remove(Student.ProgressionEntry item) {
            int index = wrapperCollection.IndexOf(item);
            bbCollection.Remove(wrapperCollection[index].BbSisObject);
            wrapperCollection.RemoveAt(index);

            return true;
        }

        public IEnumerator<Student.ProgressionEntry> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}