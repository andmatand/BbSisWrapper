using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class PerformanceRecordCollection :
        ICollection<Student.StudentSession.PerformanceRecord>
    {
        private CEAGradePerformances bbCollection;
        private List<Student.StudentSession.PerformanceRecord> wrapperCollection;

        public PerformanceRecordCollection(CEAGradePerformances bbSisCollection) {
            bbCollection = bbSisCollection;

            wrapperCollection = new List<Student.StudentSession.PerformanceRecord>();

            // Load each CEAGradePerformance into a PerformanceRecord wrapper object
            foreach (CEAGradePerformance bbObject in bbCollection) {
                wrapperCollection.Add(new Student.StudentSession.PerformanceRecord(bbObject));
            }
        }

        public void Add(Student.StudentSession.PerformanceRecord item) {
            throw new NotSupportedException();
        }

        public Student.StudentSession.PerformanceRecord Add() {
            CEAGradePerformance bbObject = bbCollection.Add();
            Student.StudentSession.PerformanceRecord newWrappedObject =
                new Student.StudentSession.PerformanceRecord(bbObject);
            wrapperCollection.Add(newWrappedObject);

            return newWrappedObject;
        }

        public void Clear() {
            while (bbCollection.Count() > 0) {
                CEAGradePerformance firstObject = bbCollection.Item(1);
                bbCollection.Remove(firstObject);
            }

            wrapperCollection.Clear();
        }

        public bool Contains(Student.StudentSession.PerformanceRecord item) {
            throw new NotImplementedException();
        }

        public void CopyTo(Student.StudentSession.PerformanceRecord[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Student.StudentSession.PerformanceRecord item) {
            int index = wrapperCollection.IndexOf(item);
            bbCollection.Remove(wrapperCollection[index].BbSisObject);
            wrapperCollection.RemoveAt(index);

            return true;
        }

        public IEnumerator<Student.StudentSession.PerformanceRecord> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}