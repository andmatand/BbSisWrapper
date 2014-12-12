using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BbSisWrapper {
    public class Course : ITopLevelObject {
        private CEACourse sisObject;
        private IBBSessionContext context;
        private List<AllowedGradeLevel> allowedGradeLevels = null;
        private List<Class> classes = null;
        private List<GradingInfo> gradingInfos = null;
        private List<Restriction> restrictions = null;

        public Course(CEACourse sisObject, IBBSessionContext context) {
            this.sisObject = sisObject;
            this.context = context;
        }

        ~Course() {
            this.Close();
        }

        public GradingInfo AddGradingInfo() {
            var sisGradingInfo = this.sisObject.GradingInfos.Add();
            sisGradingInfo.Fields[EEACOURSEGRADINGINFOFields.EACOURSEGRADINGINFO_fld_EA7COURSESID] = Ea7CoursesId;

            var newGradingInfo = new GradingInfo(sisGradingInfo);

            // Make sure our grading info list has been loaded
            LoadGradingInfos();

            // Add the new grading info to our list
            this.gradingInfos.Add(newGradingInfo);

            return newGradingInfo;
        }

        public Restriction AddRestriction() {
            //bool? oldIsNoLongerOffered = null;

            //if (this.IsNoLongerOffered) {
            //    // Save the old value of the "course is no longer offered" field
            //    oldIsNoLongerOffered = this.IsNoLongerOffered;
            //    this.IsNoLongerOffered = false;
            //}

            var sisRestriction = this.sisObject.Restrictions.Add();
            sisRestriction.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_EA7COURSESID] = Ea7CoursesId;

            // If we temporarily changed the "course is no longer offered" field
            //if (oldIsNoLongerOffered != null) {
            //    // Change the "course is no longer offered" field back to what it was
            //    this.IsNoLongerOffered = (bool) oldIsNoLongerOffered;
            //}

            var newRestriction = new Restriction(sisRestriction);

            // Make sure our restrictions have been loaded
            LoadRestrictions();

            // Add the new restriction to our list of restrictions
            this.restrictions.Add(newRestriction);

            return newRestriction;
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

        public List<Class> Classes {
            get {
                // Make sure our classes have been loaded
                LoadClasses();

                return this.classes;
            }
        }

        public string Category {
            get {
                return (string) sisObject.Fields[EEACOURSESFields.EACOURSES_fld_CATEGORY];
            }
        }

        public string CourseId {
            get {
                return (string) sisObject.Fields[EEACOURSESFields.EACOURSES_fld_COURSEID];
            }
            set {
                sisObject.Fields[EEACOURSESFields.EACOURSES_fld_COURSEID] = value;
            }
        }

        public int Ea7CoursesId {
            get {
                int id;
                int.TryParse((string) sisObject.Fields[EEACOURSESFields.EACOURSES_fld_EA7COURSESID], out id);
                return id; 
            }
        }

        private void LoadGradingInfos() {
            // If we have not loaded our grading infos yet
            if (this.gradingInfos == null) {
                this.gradingInfos = new List<GradingInfo>();

                // Load each of our grading infos into a GradingInfo object
                foreach (CEACourseGradingInfo gi in this.sisObject.GradingInfos) {
                    this.gradingInfos.Add(new GradingInfo(gi));
                }
            }
        }

        public List<GradingInfo> GradingInfos {
            get {
                // Make sure our grading infos have been loaded
                LoadGradingInfos();

                return this.gradingInfos;
            }
        }

        public bool IsNoLongerOffered {
            get {
                return ((bbTF) Enum.Parse(typeof(bbTF),
                    (string) sisObject.Fields[EEACOURSESFields.EACOURSES_fld_NOLONGEROFFERED]) ==
                    bbTF.bbTrue);
            }
            set {
                sisObject.Fields[EEACOURSESFields.EACOURSES_fld_NOLONGEROFFERED] = (value ?
                                                                                    bbTF.bbTrue :
                                                                                    bbTF.bbFalse);
            }
        }

        private void LoadAllowedGradeLevels() {
            // If we have not yet loaded our allowed grade levels
            if (this.allowedGradeLevels == null) {
                this.allowedGradeLevels = new List<AllowedGradeLevel>();

                // Load each of our allowed grade levels
                foreach (CEACourseGradeLevel gl in sisObject.GradeLevels) {
                    this.allowedGradeLevels.Add(new AllowedGradeLevel(gl));
                }
            }
        }

        private void LoadRestrictions() {
            // If we have not loaded our restrictions yet
            if (this.restrictions == null) {
                this.restrictions = new List<Restriction>();

                // Load each of our restrictions into a Restriction object
                foreach (CEACourseRestriction cr in this.sisObject.Restrictions) {
                    this.restrictions.Add(new Restriction(cr));
                }
            }
        }

        public string Name {
            get {
                return (string) sisObject.Fields[EEACOURSESFields.EACOURSES_fld_COURSENAME];
            }
            set {
                sisObject.Fields[EEACOURSESFields.EACOURSES_fld_COURSENAME] = value;
            }
        }

        public List<Restriction> Restrictions {
            get {
                // Make sure our restrictions have been loaded
                LoadRestrictions();

                return this.restrictions;
            }
        }

        public int SchoolId {
            get {
                return int.Parse((string)
                    sisObject.Fields[EEACOURSESFields.EACOURSES_fld_SCHOOLSID]);
            }
            set {
                sisObject.Fields[EEACOURSESFields.EACOURSES_fld_SCHOOLSID] = value;
            }
        }

        public int TargetClassSize {
            get {
                return int.Parse((string)
                    sisObject.Fields[EEACOURSESFields.EACOURSES_fld_TARGETCLASSSIZE]);
            }
            set {
                sisObject.Fields[EEACOURSESFields.EACOURSES_fld_TARGETCLASSSIZE] = value;
            }
        }

        public int TargetClassesPerTerm {
            get {
                return int.Parse((string)
                    sisObject.Fields[EEACOURSESFields.EACOURSES_fld_TARGETCLASSESPERTERM]);
            }
            set {
                sisObject.Fields[EEACOURSESFields.EACOURSES_fld_TARGETCLASSESPERTERM] = value;
            }
        }

        public Class AddClass() {
            // Create a new SIS class object
            var sisClass = new cEAClass();
            sisClass.Init(context);
            sisClass.Fields[EEACLASSESFields.EACLASSES_fld_EA7COURSESID] = this.Ea7CoursesId;

            // Create a new Class wrapper object
            var newClass = new Class(sisClass, context);

            // Make sure our Class list has been loaded
            LoadClasses();

            // Add the new class to our list
            this.classes.Add(newClass);
            
            return newClass;
        }

        //public Class AddClass(int sessionId, int startTermId) {
        //    // If we have a restriction (in Restrictions 2 tab) for this session
        //    //bool sessionIsAvailable = false;
        //    //foreach (CEACourseRestriction cr in this.BbSisObject.Restrictions) {
        //    //    if (cr.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_EA7SESSIONSID] == sessionId.ToString()) {
        //    //        sessionIsAvailable = true;
        //    //        break;
        //    //    }
        //    //}
        //    //if (!sessionIsAvailable) {
        //    //    throw new Exception("Error: A class cannot be added because " + CourseId + " has no " +
        //    //                        "course restriction for session " + sessionId + ".");
        //    //}

        //    // Create a new SIS class object
        //    var newClass = new cEAClass();
        //    newClass.Init(context);

        //    newClass.Fields[EEACLASSESFields.EACLASSES_fld_EA7COURSESID] = this.Ea7CoursesId;
        //    newClass.Fields[EEACLASSESFields.EACLASSES_fld_EA7SESSIONSID] = sessionId;
        //    newClass.Fields[EEACLASSESFields.EACLASSES_fld_STARTTERM] = startTermId;

        //    newClass.LoadTermsForCourse(startTermId);

        //    // Make sure our classes have been loaded
        //    LoadClasses();

        //    // Create a new Class wrapper object and add it to our list
        //    this.classes.Add(new Class(newClass, context));
        //    
        //    // Return the new Class wrapper object
        //    return this.classes.Last();
        //}

        private void LoadClasses() {
            // If our classes have not yet been loaded
            if (this.classes == null) {
                this.classes = new List<Class>();

                // Find all of our classes
                var bbClasses = new cEAClasses();
                bbClasses.Init(context);
                bbClasses.FilterObject.CustomFilterProperty[eDataFilterCustomTypes.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                    "EA7COURSESID = " + this.Ea7CoursesId;

                // Load each SIS Class into a Class wrapper object
                foreach (cEAClass bbClass in bbClasses) {
                    int ea7ClassesId;

                    // If we successfully parse an EA7CLASSESID from the SIS Class
                    if (int.TryParse((string) bbClass.Fields[EEACLASSESFields.EACLASSES_fld_EA7CLASSESID], out ea7ClassesId)) {
                        // Release our handle on this SIS Class
                        bbClass.CloseDown();

                        // Load the class (by its ID) into a Class object wrapper, and add it to our list
                        this.classes.Add(Class.LoadByEA7ClassesId(ea7ClassesId, this.context));
                    }
                }

                // Release our handle on the SIS Class collection
                bbClasses.CloseDown();
            }
        }

        public void Reload() {
            // Save our Ea7CoursesId
            int ea7CoursesId = this.Ea7CoursesId;

            // Close the SIS record
            this.Close();

            // Load the same record again
            this.sisObject = LoadSisRecord(ea7CoursesId, context);
        }

        public void Save() {
            try {
                this.sisObject.Save();
            }
            catch (Exception ex) {
                Console.WriteLine("Error saving Course record " + CourseId + " \"" + Name + "\": " +
                                  ex.Message);
            }
        }

        public void Close() {
            // If we have classes loaded
            if (this.classes != null) {
                // Close each of our classes
                foreach (Class c in this.classes) {
                    c.Close();
                }

                // Clear our list of classes
                this.classes = null;
            }

            // If we have any allowed grade levels loaded
            if (this.allowedGradeLevels != null) {
                // Clear our list of allowed grade levels
                this.allowedGradeLevels = null;
            }

            // If we have course restrictions loaded
            if (this.restrictions != null) {
                // Clear our list of restrictions
                this.restrictions = null;
            }

            // If we have grading infos loaded
            if (this.gradingInfos != null) {
                // Clear our list of grading infos
                this.gradingInfos = null;
            }

            // Release our handle on the SIS record
            this.sisObject.CloseDown();
        }

        private static CEACourse LoadSisRecord(int ea7CoursesId, IBBSessionContext context) {
            var record = new CEACourse();
            record.Init(context);
            record.Load(ea7CoursesId);

            return record;
        }

        public static Course LoadByEA7CoursesId(int ea7CoursesId, IBBSessionContext context) {
            var sisRecord = LoadSisRecord(ea7CoursesId, context);
            return new Course(sisRecord, context);
        }

        public AllowedGradeLevel AddAllowedGradeLevel() {
            var sisObject = this.sisObject.GradeLevels.Add();
            sisObject.Fields[EEACOURSEGRADELEVELSFields.EACOURSEGRADELEVELS_fld_EA7COURSESID] = this.Ea7CoursesId;

            var newLevel = new AllowedGradeLevel(sisObject);

            // Make sure our list of allowed gradelevels is loaded
            LoadAllowedGradeLevels();

            this.allowedGradeLevels.Add(newLevel);

            return newLevel;
        }

        public class AllowedGradeLevel {
            private CEACourseGradeLevel sisObject;

            public AllowedGradeLevel(CEACourseGradeLevel sisObject) {
                this.sisObject = sisObject;
            }

            public string GradeLevel {
                get {
                    return (string) sisObject.Fields[EEACOURSEGRADELEVELSFields.EACOURSEGRADELEVELS_fld_GRADELEVEL];
                }
                set {
                    sisObject.Fields[EEACOURSEGRADELEVELSFields.EACOURSEGRADELEVELS_fld_GRADELEVEL] = value;
                }
            }
        }

        public class GradingInfo {
            private CEACourseGradingInfo sisObject;
            private List<GpaWeight> gpaWeights = null;
            private List<GradeSetting> gradeSettings = null;

            public GradingInfo(CEACourseGradingInfo sisObject) {
                this.sisObject = sisObject;
            }

            public int AcademicYearId {
                get {
                    int id;
                    int.TryParse((string) sisObject.Fields[EEACOURSEGRADINGINFOFields.EACOURSEGRADINGINFO_fld_EA7ACADEMICYEARSID], out id);
                    return id;
                }
                set {
                    sisObject.Fields[EEACOURSEGRADINGINFOFields.EACOURSEGRADINGINFO_fld_EA7ACADEMICYEARSID] = value;
                }
            }

            public GpaWeight AddGpaWeight() {
                var sisObject = this.sisObject.GPAWeights.Add();
                var newGpaWeight = new GpaWeight(sisObject);

                // Make sure our GPA Weight list has been loaded
                LoadGpaWeights();

                // Add the new GpaWeight object to our list of GpaWeight objects
                this.gpaWeights.Add(newGpaWeight);

                return newGpaWeight;
            }

            public List<GpaWeight> GpaWeights {
                get {
                    // Make sure our GPA Weights have been loaded
                    LoadGpaWeights();

                    return this.gpaWeights;
                }
            }

            public List<GradeSetting> GradeSettings {
                get {
                    // Make sure our grade settings have been loaded
                    LoadGradeSettings();

                    return this.gradeSettings;
                }
            }

            private void LoadGpaWeights() {
                // If we have not yet loaded our GPA weights
                if (this.gpaWeights == null) {
                    this.gpaWeights = new List<GpaWeight>();

                    // Load each of our CEACourseGPAWeight objects in to GpaWeight wrapper objects
                    foreach (CEACourseGPAWeight cgw in sisObject.GPAWeights) {
                        this.gpaWeights.Add(new GpaWeight(cgw));
                    }
                }
            }

            private void LoadGradeSettings() {
                // If we have not yet loaded our grade settings
                if (this.gradeSettings == null) {
                    this.gradeSettings = new List<GradeSetting>();

                    // Load each of our CEACourseGradeInfoGrade objects into GradeSettings
                    // wrapper objects
                    foreach (CEACourseGradeInfoGrade cgig in sisObject.Grades) {
                        this.gradeSettings.Add(new GradeSetting(cgig));
                    }
                }
            }

            public int SessionId {
                get {
                    int id;
                    int.TryParse((string) sisObject.Fields[EEACOURSEGRADINGINFOFields.EACOURSEGRADINGINFO_fld_EA7SESSIONSID], out id);
                    return id;
                }
                set {
                    sisObject.Fields[EEACOURSEGRADINGINFOFields.EACOURSEGRADINGINFO_fld_EA7SESSIONSID] = value;
                }
            }

            public class GpaWeight {
                private CEACourseGPAWeight sisObject;

                public GpaWeight(CEACourseGPAWeight sisObject) {
                    this.sisObject = sisObject;
                }

                public string Gpa {
                    get {
                        return (string) sisObject.Fields[EEACOURSEGPAWEIGHTSFields.EACOURSEGPAWEIGHTS_fld_GPA];
                    }
                    set {
                        sisObject.Fields[EEACOURSEGPAWEIGHTSFields.EACOURSEGPAWEIGHTS_fld_GPA] = value;
                    }
                }

                public int Weight {
                    get {
                        int weight;
                        int.TryParse((string) sisObject.Fields[EEACOURSEGPAWEIGHTSFields.EACOURSEGPAWEIGHTS_fld_WEIGHT], out weight);
                        return weight;
                    }
                    set {
                        sisObject.Fields[EEACOURSEGPAWEIGHTSFields.EACOURSEGPAWEIGHTS_fld_WEIGHT] = value;
                    }
                }
            }

            public class GradeSetting {
                private CEACourseGradeInfoGrade sisObject;

                public GradeSetting(CEACourseGradeInfoGrade sisObject) {
                    this.sisObject = sisObject;
                }

                public decimal Credits {
                    get {
                        decimal credits;
                        decimal.TryParse((string) sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_ATTEMPTEDCREDITS], out credits);
                        return credits;
                    }
                    set {
                        sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_ATTEMPTEDCREDITS] = value;
                    }
                }

                public bool AwardsCredit {
                    get {
                        return ((bbTF) Enum.Parse(typeof(bbTF),
                            (string) sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_AWARDCREDITSIN])
                            == bbTF.bbTrue);
                    }
                    set {
                        sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_AWARDCREDITSIN] = (value ? bbTF.bbTrue : bbTF.bbFalse);
                    }
                }

                public bool IsGraded {
                    get {
                        return ((bbTF) Enum.Parse(typeof(bbTF),
                            (string) sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_USEMARKINGCOLUMN])
                            == bbTF.bbTrue);
                    }
                    set {
                        sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_USEMARKINGCOLUMN] = (value ? bbTF.bbTrue : bbTF.bbFalse);
                    }
                }

                public int MarkingColumnId {
                    get {
                        int id;
                        int.TryParse((string) sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_EA7MARKINGCOLUMNSID], out id);
                        return id;
                    }
                    set {
                        sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_EA7MARKINGCOLUMNSID] = value;
                    }
                }

                public int TranslationTableId {
                    get {
                        int id;
                        int.TryParse((string) sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_EA7TRANSLATIONSID], out id);
                        return id;
                    }
                    set {
                        sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_EA7TRANSLATIONSID] = value;
                    }
                }

                public string ValuesAllowed {
                    get {
                        return (string) sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_VALUESALLOWED];
                    }
                    set {
                        sisObject.Fields[EEACOURSEGRADINGINFOGRADESFields.EACOURSEGRADINGINFOGRADES_fld_VALUESALLOWED] = value;
                    }
                }
            }
        }

        public class Restriction {
            private CEACourseRestriction sisObject;

            public Restriction(CEACourseRestriction sisObject) {
                this.sisObject = sisObject;
            }

            public int AcademicYearId {
                get {
                    int id;
                    int.TryParse((string) sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_EA7ACADEMICYEARSID], out id);
                    return id;
                }
                set {
                    sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_EA7ACADEMICYEARSID] = value;
                }
            }

            public int LengthInTerms {
                get {
                    int length;
                    int.TryParse((string) sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_LENGTHINTERMS], out length);
                    return length;
                }
                set {
                    sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_LENGTHINTERMS] = value;
                }
            }

            public string RestrictBy {
                get {
                    return (string) sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_RESTRICTBY];
                }
                set {
                    sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_RESTRICTBY] = value;
                }
            }

            public void AddMeeting(int numberOfMeetings, int lengthInMinutes) {
                CEACourseRestrictMtg newMeeting = sisObject.Meetings.Add();
                newMeeting.Fields[EEACOURSERESTRICTIONSMEETINGSFields.EACOURSERESTRICTIONSMEETINGS_fld_NUMBEROFMEETINGS] = numberOfMeetings;
                newMeeting.Fields[EEACOURSERESTRICTIONSMEETINGSFields.EACOURSERESTRICTIONSMEETINGS_fld_LENGTHINMINUTES] = lengthInMinutes;
            }

            public void AddStartTerm(int termId) {
                CEACourseRestrictStrTrm newTerm = sisObject.StartTerms.Add();
                newTerm.Fields[EEACOURSERESTRICTIONSSTARTTERMSFields.EACOURSERESTRICTIONSSTARTTERMS_fld_EA7TERMSID] = termId;
            }

            public int SessionId {
                get {
                    return int.Parse((string)
                        sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_EA7SESSIONSID]);
                }
                set {
                    sisObject.Fields[EEACOURSERESTRICTIONSFields.EACOURSERESTRICTIONS_fld_EA7SESSIONSID] = value;
                }
            }
        }
    }
}
