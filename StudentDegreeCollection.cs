using Blackbaud.PIA.EA7.BBEEAPI7;
using System.Collections.ObjectModel;

namespace BbSisWrapper {
    public partial class Student {
        public class StudentDegreeCollection : Collection<StudentDegree> {
            private cEAStudentDegrees bbCollection;
            private Student student;
            private IBBSessionContext context;

            public StudentDegreeCollection(Student student, IBBSessionContext context) {
                this.student = student;
                this.context = context;

                // This strange metafield hack is from BB support.  Apparently I found a bug in the
                // API: there is no (normal) way of accessing a student's degrees.
                IBBMetaField metaField = (IBBMetaField) student;

                bbCollection = (cEAStudentDegrees) metaField.ChildObject[
                    (int) EEAStudentChildren.EASTUDENTS_child_DEGREES];

                foreach (cEAStudentDegree sd in bbCollection) {
                    if ((string)
                        sd.Fields[eEASTUDENTDEGREESFields.EASTUDENTDEGREES_fld_DEGREETYPE] ==
                        "StudentDegree") {
                        Add(new StudentDegree(sd, student, context));
                    }
                }
            }

            public void Close() {
                foreach (StudentDegree degree in this) {
                    degree.Close();
                }
            }

            public Student.StudentDegree Add() {
                var newEnrollment = new StudentDegree(bbCollection.Add(), student, context);
                Add(newEnrollment);

                return newEnrollment;
            }

            public new void Remove(StudentDegree item) {
                item.Close();

                // Remove the BB item from the BB collection
                bbCollection.Remove(item.SisObject);

                // Remove the wrapper item from the wrapper collection
                Remove(item);
            }

            public new void RemoveAt(int index) {
                Remove(Items[index]);
            }
        }
    }
}