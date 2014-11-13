using System;
using System.Collections.Generic;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class GpaCollection : ICollection<Student.StudentSession.Gpa> {
        private CEAGradeGPAs bbCollection;
        private List<Student.StudentSession.Gpa> wrapperCollection;

        public GpaCollection(CEAGradeGPAs bbSisCollection) {
            bbCollection = bbSisCollection;

            wrapperCollection = new List<Student.StudentSession.Gpa>();

            // Load each CEAGradeGPA into a Gpa wrapper object
            foreach (CEAGradeGPA bbObject in bbCollection) {
                wrapperCollection.Add(new Student.StudentSession.Gpa(bbObject));
            }
        }

        public Student.StudentSession.Gpa Add() {
            CEAGradeGPA bbObject = bbCollection.Add();
            Student.StudentSession.Gpa newWrappedObject = new Student.StudentSession.Gpa(bbObject);
            wrapperCollection.Add(newWrappedObject);

            return newWrappedObject;
        }

        public void Add(Student.StudentSession.Gpa item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            while (bbCollection.Count() > 0) {
                CEAGradeGPA firstObject = bbCollection.Item(1);
                bbCollection.Remove(firstObject);
            }

            wrapperCollection.Clear();
        }

        public bool Contains(Student.StudentSession.Gpa item) {
            throw new NotImplementedException();
        }

        public void CopyTo(Student.StudentSession.Gpa[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Student.StudentSession.Gpa item) {
            int index = wrapperCollection.IndexOf(item);
            bbCollection.Remove(wrapperCollection[index].BbSisObject);
            wrapperCollection.RemoveAt(index);

            return true;
        }

        public IEnumerator<Student.StudentSession.Gpa> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}