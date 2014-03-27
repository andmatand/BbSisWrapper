using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.eEASTUDENTDEGREESFields;

namespace BbSisWrapper {
    public partial class Student : IPerson {
        public class StudentDegree {
            private cEAStudentDegree bbObject;
            private Student student;
            private IBBSessionContext context;
            private List<Major> majors;
            private List<Minor> minors;
            private List<Honor> honors;

            public StudentDegree(cEAStudentDegree sisObject, Student student, IBBSessionContext context) {
                this.bbObject = sisObject;
                this.student = student;
                this.context = context;
            }

            ~StudentDegree() {
                Close();
            }

            public void Close() {
                if (majors != null) {
                    foreach (Major major in majors) {
                        major.Close();
                    }
                }

                if (minors != null) {
                    foreach (Minor minor in minors) {
                        minor.Close();
                    }
                }
            }

            //public void Delete() {
            //    // This strange metafield hack is from BB support.  Apparently I found a bug in the
            //    // API: there is no (normal) way of accessing a student's degrees.
            //    IBBMetaField metaField = (IBBMetaField) student.SisObject;

            //    cEAStudentDegrees studentDegrees;
            //    studentDegrees = metaField.ChildObject[
            //        (int) EEAStudentChildren.EASTUDENTS_child_DEGREES];

            //    studentDegrees.Remove(SisObject);
            //    SisObject = null;

            //    // Remove the degree from the student's list of degrees
            //    student.DeleteDegree(this);
            //}

            public cEAStudentDegree SisObject {
                get { return bbObject; }
            }

            public DateTime DateConferred {
                get {
                    return DateTime.Parse((string)
                        bbObject.Fields[FIELDS.EASTUDENTDEGREES_fld_CONFERREDON]);
                }
            }

            public DateTime DateCompleted {
                get {
                    return DateTime.Parse((string)
                        bbObject.Fields[FIELDS.EASTUDENTDEGREES_fld_COMPLETEDON]);
                }
            }

            public string DisplayName {
                get {
                    return (string) bbObject.Fields[FIELDS.EASTUDENTDEGREES_fld_CUSTOMIZABLEDISPLAYNAME];
                }
            }

            public int Ea7StudentDegreesId {
                get {
                    return int.Parse((string)
                        bbObject.Fields[FIELDS.EASTUDENTDEGREES_fld_EA7STUDENTDEGREESID]);
                }
            }

            private void LoadHonors() {
                if (honors == null) {
                    honors = new List<Honor>();

                    string category = (string) bbObject.Fields[
                        FIELDS.EASTUDENTDEGREES_fld_HONORSEARNEDCATEGORY];
                    string level = (string) bbObject.Fields[
                        FIELDS.EASTUDENTDEGREES_fld_HONORSEARNEDLEVEL];

                    honors.Add(new Honor(category, level));
                }
            }

            private void LoadMajors() {
                if (majors == null) {
                    majors = new List<Major>();

                    // Load all major/minors
                    var allMajors = new cEAMajorMinors();
                    allMajors.Init(context);

                    // Filter to majors of this degree
                    allMajors.FilterObject.CustomFilterProperty[
                        eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMFROM] =
                        "EA7STUDENTDEGREES " +
                        "join EA7DEGREES " +
                        "    on EA7DEGREES.EA7DEGREESID = EA7STUDENTDEGREES.EA7DEGREESID";
                    allMajors.FilterObject.CustomFilterProperty[
                        eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                        "EA7STUDENTDEGREES.PARENTDEGREEID = " + Ea7StudentDegreesId + " and " +
                        "EA7STUDENTDEGREES.DEGREETYPE = 2";

                    // Add each major in the filtered collection
                    foreach (cEAMajorMinor major in allMajors) {
                        majors.Add(new Major(major));
                    }

                    // Release our handle on the collection
                    allMajors.CloseDown();
                }
            }

            private void LoadMinors() {
                if (minors == null) {
                    minors = new List<Minor>();

                    // Load all major/minors
                    var allMinors = new cEAMajorMinors();
                    allMinors.Init(context);

                    // Filter to minors of this degree
                    allMinors.FilterObject.CustomFilterProperty[
                        eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMFROM] =
                        "EA7STUDENTDEGREES " +
                        "join EA7DEGREES " +
                        "    on EA7DEGREES.EA7DEGREESID = EA7STUDENTDEGREES.EA7DEGREESID";
                    allMinors.FilterObject.CustomFilterProperty[
                        eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                        "EA7STUDENTDEGREES.PARENTDEGREEID = " + Ea7StudentDegreesId + " and " +
                        "EA7STUDENTDEGREES.DEGREETYPE = 3";

                    // Add each major in the filtered collection
                    foreach (cEAMajorMinor minor in allMinors) {
                        minors.Add(new Minor(minor));
                    }

                    // Release our handle on the collection
                    allMinors.CloseDown();
                }
            }

            public List<Honor> Honors {
                get {
                    LoadHonors();
                    return honors;
                }
            }

            public List<Major> Majors {
                get {
                    LoadMajors();
                    return majors;
                }
            }

            public List<Minor> Minors {
                get {
                    LoadMinors();
                    return minors;
                }
            }


            public class Honor {
                private string category;
                private string level;

                public Honor(string category, string level) {
                    this.category = category;
                    this.level = level;
                }

                public string Category {
                    get {
                        return category;
                    }
                }

                public string Level {
                    get {
                        return level;
                    }
                }
            }


            public class MajorOrMinor {
                private cEAMajorMinor bbObject;

                public MajorOrMinor(cEAMajorMinor sisObject) {
                    this.bbObject = sisObject;
                }

                ~MajorOrMinor() {
                    Close();
                }

                public void Close() {
                    bbObject.CloseDown();
                }

                public string DisplayName {
                    get {
                        return (string) bbObject.Fields[eEAMajorMinorsFields.EAMAJORMINORS_fld_NAMEFORDISPLAY];
                    }
                }
            }

            public class Major : MajorOrMinor {
                public Major(cEAMajorMinor sisObject) : base(sisObject) {
                }
            }

            public class Minor : MajorOrMinor {
                public Minor(cEAMajorMinor sisObject) : base(sisObject) {
                }
            }
        }
    }
}