using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbaud.PIA.EA7.BBEEAPI7;

namespace BbSisWrapper {
    public class NoteCollection : ICollection<Student.Note> {
        private IBBNotepadsAPI bbCollection;
        private List<Student.Note> wrapperCollection;

        public NoteCollection(IBBNotepadsAPI bbSisCollection) {
            this.bbCollection = bbSisCollection;

            wrapperCollection = new List<Student.Note>();

            foreach (IBBNotepad bbObject in bbCollection) {
                wrapperCollection.Add(new Student.Note(bbObject));
            }
        }

        public void Add(Student.Note item) {
            throw new NotSupportedException();
        }

        public Student.Note Add() {
            // Create a new BBSIS object
            IBBNotepad newBbNotepad = bbCollection.Add();

            // Wrap the BBSIS object and add it to our List
            Student.Note newNote = new Student.Note(newBbNotepad);
            wrapperCollection.Add(newNote);

            // Return the wrapped BBSIS object
            return newNote;
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(Student.Note item) {
            throw new NotImplementedException();
        }

        public void CopyTo(Student.Note[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(Student.Note item) {
            throw new NotImplementedException();
        }

        public IEnumerator<Student.Note> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}