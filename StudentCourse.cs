using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using FIELD = Blackbaud.PIA.EA7.BBEEAPI7.EEASTUDENTCOURSESFields;

namespace BbSisWrapper {
    public partial class Student {
        public partial class StudentCourse {
            private IBBSessionContext context;
            private cEAStudentCourse sisObject;
            private List<Grade> grades;

            public StudentCourse(cEAStudentCourse sisObject, IBBSessionContext context) {
                this.sisObject = sisObject;
                this.context = context;
            }

            ~StudentCourse() {
                Close();
            }

            public int AcademicYearId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELD.EASTUDENTCOURSES_fld_EA7ACADEMICYEARSID]);
                }
            }

            public cEAStudentCourse BbSisObject {
                get {
                    return sisObject;
                }
            }

            public void Close() {
                sisObject.CloseDown();
            }

            public string ClassSection {
                get {
                    return (string) sisObject.Fields[FIELD.EASTUDENTCOURSES_fld_CLASSSECTION];
                }
            }

            public Course Course {
                get {
                    return Course.LoadByEA7CoursesId(Ea7CoursesId, context);
                }
            }

            public int Ea7CoursesId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELD.EASTUDENTCOURSES_fld_EA7COURSESID]);
                }
            }

            public List<Grade> Grades {
                get {
                    // Make sure we've loaded our grades
                    LoadGrades();

                    return grades;
                }
            }

            private void LoadGrades() {
                // If we haven't loaded our grades yet
                if (grades == null) {
                    grades = new List<Grade>();

                    // Load each of our grades into Grade objects
                    foreach (CEAGrade sisGrade in sisObject.Grades) {
                        grades.Add(new Grade(sisGrade));
                    }
                }
            }

            public void Save() {
                sisObject.Save();
            }

            public int SessionId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELD.EASTUDENTCOURSES_fld_EA7SESSIONSID]);
                }
            }

            public int StartTermId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[FIELD.EASTUDENTCOURSES_fld_STARTTERM]);
                }
            }

            public Grade AddGrade() {
                CEAGrade sisGrade = (CEAGrade) sisObject.Grades.Add();

                var newGrade = new Grade(sisGrade);

                // Make sure we've loaded our grade list
                LoadGrades();

                // Add the new grade to our grade list
                grades.Add(newGrade);

                return newGrade;
            }

            public void AddGrade(int markingColumnId, int translationEntryId,
                                 decimal? creditAwarded = null, decimal? creditAttempted = null) {
                CEAGrade newGrade = (CEAGrade) sisObject.Grades.Add();
                newGrade.Fields[EEASTUDENTGRADESFIELDS.EASTUDENTGRADES_fld_EA7MARKINGCOLUMNSID] = markingColumnId;
                newGrade.Fields[EEASTUDENTGRADESFIELDS.EASTUDENTGRADES_fld_EA7TRANSLATIONENTRIESID] = translationEntryId;
                if (creditAwarded != null) newGrade.Fields[EEASTUDENTGRADESFIELDS.EASTUDENTGRADES_fld_CREDITAWARDED] = creditAwarded;
                if (creditAttempted != null) newGrade.Fields[EEASTUDENTGRADESFIELDS.EASTUDENTGRADES_fld_CREDITATTEMPTED] = creditAttempted;

                // Make sure we've loaded our grade list
                LoadGrades();

                grades.Add(new Grade(newGrade));
            }

            public void AddGrade(string markingColumn, string grade, decimal? creditAwarded = null,
                                 decimal? creditAttempted = null) {
                // Lookup the markingColumnId
                int? markingColumnId = Grade.LookupMarkingColumnId(markingColumn, context);
                if (markingColumnId == null) {
                    throw new Exception("Marking column '" + markingColumn + "' was not found.");
                }

                // Find the translation table ID for this academic year and session
                int? translationTableId = null;
                foreach (Course.GradingInfo gradingInfo in Course.GradingInfos) {
                    if (gradingInfo.AcademicYearId == AcademicYearId &&
                        gradingInfo.SessionId == SessionId) {
                        foreach (Course.GradingInfo.GradeSetting gradeSetting in gradingInfo.GradeSettings) {
                            if (gradeSetting.MarkingColumnId == (int) markingColumnId) {
                                translationTableId = gradeSetting.TranslationTableId;
                                break;
                            }
                        }
                        break;
                    }
                }

                // If no translation table ID was found
                if (translationTableId == null) {
                    throw new Exception("No translation table ID was found for marking column " +
                                        markingColumn + ".");
                }

                // Lookup the translationEntryId for the given letter grade
                int? translationEntryId = Grade.LookupTranslationEntryId((int) translationTableId, grade, context);

                if (translationEntryId == null) {
                    throw new Exception("No translation entry ID was found for grade " + grade + " in " +
                                        "translation table " + translationTableId);
                }

                AddGrade((int) markingColumnId, (int) translationEntryId, creditAwarded, creditAttempted);
            }

            public void AddGrade(CEAGrade sisGrade) {
                grades.Add(new Grade(sisGrade));
            }
        }
    }
}
