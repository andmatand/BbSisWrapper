using System;
using System.Collections.Generic;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class StudentCourseCollection : ICollection<Student.StudentCourse>, IDisposable {
        private IBBSessionContext context;
        private cEAStudentCourses bbCollection;
        private List<Student.StudentCourse> wrapperCollection;

        public
        StudentCourseCollection(cEAStudentCourses bbSisCollection, IBBSessionContext context) {
            bbCollection = bbSisCollection;
            this.context = context;

            wrapperCollection = new List<Student.StudentCourse>();

            // Load each of our studentcourses into a StudentCourse object
            foreach (cEAStudentCourse sc in bbCollection) {
                wrapperCollection.Add(new Student.StudentCourse(sc, context));
            }
        }

        public Student.StudentCourse Add() {
            cEAStudentCourse bbObject = bbCollection.Add();
            Student.StudentCourse newWrappedObject = new Student.StudentCourse(bbObject, context);
            wrapperCollection.Add(newWrappedObject);

            return newWrappedObject;
        }

        public void Add(Student.StudentCourse item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            while (bbCollection.Count() > 0) {
                cEAStudentCourse firstObject = bbCollection.Item(1);
                bbCollection.Remove(firstObject);
            }

            wrapperCollection.Clear();
        }

        public void Close() {
            bbCollection.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbCollection);
            bbCollection = null;
        }

        public bool Contains(Student.StudentCourse item) {
            return wrapperCollection.Contains(item);
        }

        public void CopyTo(Student.StudentCourse[] array, int arrayIndex) {
            wrapperCollection.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Student.StudentCourse item) {
            int index = wrapperCollection.IndexOf(item);
            bbCollection.Remove(wrapperCollection[index].BbSisObject);
            wrapperCollection.RemoveAt(index);

            return true;
        }

        public void Save() {
            bbCollection.Save();
        }

        public IEnumerator<Student.StudentCourse> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        public void Dispose() {
            Close();
        }
    }
}