using Blackbaud.PIA.EA7.BBEEAPI7;
using System.Collections.Generic;

namespace BbSisWrapper {
    public class Class : ITopLevelObject {
        private cEAClass sisObject;
        private IBBSessionContext context;
        private List<Student> students = null;

        public Class(cEAClass sisObject, IBBSessionContext context) {
            this.sisObject = sisObject;
            this.context = context;
        }

        ~Class() {
            this.Close();
        }

        public bool CanBeSaved {
            get { return sisObject.CanBeSaved(); }
        }

        public ReasonRecordCannotBeSaved ReasonRecordCannotBeSaved {
            get {
                bbCantSaveReasons bbCantSaveReason = bbCantSaveReasons.csrObjectVersionOutOfDate;
                string message = null;

                if (!sisObject.CanBeSaved(ref bbCantSaveReason, ref message)) {
                    return new ReasonRecordCannotBeSaved(bbCantSaveReason, message);
                }
                else {
                    return null;
                }
            }
        }

        public int Ea7ClassesId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_EA7CLASSESID], out id);
                return id;
            }
        }

        public int Ea7CoursesId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_EA7COURSESID], out id);
                return id;
            }
        }

        public string Name {
            get {
                return (string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_CLASSNAME];
            }
        }

        public string Section {
            get {
                return (string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_CLASSSECTION];
            }
            set {
                sisObject.Fields[EEACLASSESFields.EACLASSES_fld_CLASSSECTION] = value;
            }
        }

        public string AcademicYear {
            get {
                return (string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_EAACADEMICYEARID];
            }
        }

        public int AcademicYearId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_EAACADEMICYEARSID], out id);
                return id;
            }
        }

        public string SchoolShortName {
            get {
                return (string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_SCHOOLID];
            }
        }

        public int SchoolId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_SCHOOLSID], out id);
                return id;
            }
        }

        public int SessionId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_EA7SESSIONSID], out id);
                return id;
            }
            set {
                sisObject.Fields[EEACLASSESFields.EACLASSES_fld_EA7SESSIONSID] = value;
            }
        }

        public int StartTermId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACLASSESFields.EACLASSES_fld_STARTTERM], out id);
                return id;
            }
            set {
                sisObject.Fields[EEACLASSESFields.EACLASSES_fld_STARTTERM] = value;
                sisObject.LoadTermsForCourse(this.StartTermId);
            }
        }

        public cEAClass SisObject {
            get {
                return this.sisObject;
            }
        }

        public List<Student> Students {
            get {
                // Make sure our list of students is loaded
                LoadStudents();

                return this.students;
            }
        }

        private void LoadStudents() {
            // If we haven't loaded our students yet
            if (this.students == null) {
                this.students = new List<Student>();

                // Load each of our students
                foreach (CEAClassStudent student in this.sisObject.ClassStudents) {
                    int ea7StudentsId;
                    if (int.TryParse((string) student.Fields[EEAClassStudentFields.EACLASSSTUDENT_fld_EA7STUDENTID], out ea7StudentsId)) {
                        this.students.Add(Student.LoadByEa7StudentsId(ea7StudentsId, this.context));
                    }
                }
            }
        }

        public void AddStudent(Student student) {
            // Add a new student to our SIS object's ClassStudents
            CEAClassStudent newStudent = this.sisObject.ClassStudents.Add(this.StartTermId);

            // Set the EA7STUDENTSID of the new student
            newStudent.Fields[EEAClassStudentFields.EACLASSSTUDENT_fld_EA7STUDENTID] = student.Ea7StudentsId;

            // Find the ClassStudentTerm for this class
            foreach (CEAClassStudentTerm cst in newStudent.ClassStudentTerms()) {
                // If this is the ClassStudentTerm for this class
                if ((string) cst.Fields[eEAClassStudentTermsFields.EACLASSSTUDENTTERM_fld_EA7CLASSESID] == this.Ea7ClassesId.ToString()) {
                    // Set the start term of the ClassStudentTerm
                    cst.SetClassTermFromTerm(this.StartTermId);

                    // Mark the ClassStudentTerm as enrolled
                    cst.Fields[eEAClassStudentTermsFields.EACLASSSTUDENTTERM_fld_ENROLLED] = bbTF.bbTrue;

                    // If our list of students is loaded
                    if (this.students != null) {
                        // Add a copy of the student to our list of Student wrapper objects
                        this.students.Add(Student.LoadByEa7StudentsId(student.Ea7StudentsId, context));
                    }

                    break;
                }
            }
        }

        public void Close() {
            // If we have any students loaded
            if (this.students != null) {
                // Close each of our students
                foreach (Student student in this.students) {
                    student.Close();
                }

                // Clear our list of students
                this.students = null;
            }

            // Release our handle on the SIS record
            this.sisObject.CloseDown();
        }

        public void Reload() {
            // Save our Ea7ClassesId
            int ea7ClassesId = this.Ea7ClassesId;

            // Close the SIS record
            this.Close();

            // Load the same student again
            this.sisObject = LoadSisRecord(ea7ClassesId, context);
        }

        public void Save() {
            this.sisObject.Save();
        }

        private static cEAClass LoadSisRecord(int ea7ClassesId, IBBSessionContext context) {
            var record = new cEAClass();
            record.Init(context);
            record.Load(ea7ClassesId);

            return record;
        }

        public static Class LoadByEA7ClassesId(int ea7ClassesId, IBBSessionContext context) {
            var record = LoadSisRecord(ea7ClassesId, context);
            return new Class(record, context);
        }
    }
}