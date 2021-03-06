﻿using System;
using System.Collections.Generic;
using System.Linq;
using Blackbaud.PIA.EA7.BBEEAPI7;
using FIELD = Blackbaud.PIA.EA7.BBEEAPI7.EEASTUDENTSFields;
using FILTERTYPE = Blackbaud.PIA.EA7.BBEEAPI7.eDataFilterCustomTypes;
using IBBAddressHeaders = Blackbaud.PIA.FE7.AFNInterfaces.IBBAddressHeaders;
using IBBAttributesAPI = Blackbaud.PIA.FE7.AFNInterfaces.IBBAttributesAPI;

namespace BbSisWrapper {
    public partial class Student : IPerson, IDisposable {
        private cEAStudent bbRecord;
        protected IBBSessionContext context;
        private AddressCollection addresses = null;
        private AttributeCollection attributes = null;
        private StudentDegreeCollection degrees = null;
        private EnrollmentCollection enrollments;
        private NoteCollection notes = null;
        private ProgressionEntryCollection progressionEntries = null;
        private StudentCourseCollection studentCourses = null;
        private List<StudentSession> studentSessions = null;
        private RelationshipCollection relationships = null;
        private RecordStatusLogEntryCollection statusLog = null;

        public Student(cEAStudent bbRecord, Context context) {
            this.bbRecord = bbRecord;
            this.context = context.BbSisContext;
        }

        private Student(cEAStudent bbRecord, IBBSessionContext context) {
            this.bbRecord = bbRecord;
            this.context = context;
        }

        public AddressCollection Addresses {
            get {
                if (addresses == null) {
                    addresses = new AddressCollection((IBBAddressHeaders) bbRecord.Address);
                }

                return addresses;
            }
        }

        public AttributeCollection Attributes {
            get {
                if (attributes == null) {
                    attributes = new AttributeCollection((IBBAttributesAPI) bbRecord.Attributes);
                }

                return attributes;
            }
        }

        public bool CanBeSaved {
            get { return bbRecord.CanBeSaved(); }
        }

        public ReasonRecordCannotBeSaved ReasonRecordCannotBeSaved {
            get {
                bbCantSaveReasons bbCantSaveReason = bbCantSaveReasons.csrObjectVersionOutOfDate;
                string message = null;

                if (!bbRecord.CanBeSaved(ref bbCantSaveReason, ref message)) {
                    return new ReasonRecordCannotBeSaved(bbCantSaveReason, message);
                }
                else {
                    return null;
                }
            }
        }

        public DateTime DateAdded {
            get {
                return DateTime.Parse((string) bbRecord.Fields[FIELD.EASTUDENTS_fld_DATEADDED]);
            }
        }

        public StudentDegreeCollection Degrees {
            get {
                if (degrees == null) {
                    degrees = new StudentDegreeCollection(this, context);
                }

                return degrees;
            }
        }

        public int Ea7RecordsId {
            get {
                return int.Parse((string) bbRecord.Fields[FIELD.EASTUDENTS_fld_EA7RECORDSID]);
            }
        }

        public EnrollmentCollection Enrollments {
            get {
                if (enrollments == null) {
                    enrollments = new EnrollmentCollection(bbRecord.Enrollments);
                }

                return enrollments;
            }
        }

        public int Ea7StudentsId {
            get {
                return int.Parse((string) bbRecord.Fields[FIELD.EASTUDENTS_fld_EA7STUDENTSID]);
            }
        }

        public string FirstName {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_FIRSTNAME];
            }
        }

        public PersonGender Gender {
            get {
                switch ((string) bbRecord.Fields[FIELD.EASTUDENTS_fld_GENDER]) {
                    case "Male":
                        return PersonGender.Male;
                    case "Female":
                        return PersonGender.Female;
                    default:
                        return PersonGender.Null;
                }
            }
        }

        public string GradeLevel {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_GRADELEVEL];
            }
        }

        public string IdNumber {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_USERDEFINEDID];
            }
        }

        public string LastName {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_LASTNAME];
            }
        }

        public string Nickname {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_NICKNAME];
            }
        }

        public string School {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_SCHOOL];
            }
        }

        public RecordStatusLogEntryCollection StatusLog {
            get {
                if (statusLog == null) {
                    statusLog = new RecordStatusLogEntryCollection(bbRecord.StatusLogs);
                }

                return statusLog;
            }
        }

        public string MiddleName {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_MIDDLENAME];
            }
        }

        public NoteCollection Notes {
            get {
                // If we have not yet loaded our notes
                if (notes == null) {
                    notes = new NoteCollection(bbRecord.NotePads);
                }

                return notes;
            }
        }

        public string OnlinePassword {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_ONLINEPASSWORD];
            }
            set {
                bbRecord.Fields[FIELD.EASTUDENTS_fld_ONLINEPASSWORD] = value;
            }
        }

        public string OnlineUserId {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_ONLINEUSERID];
            }
            set {
                bbRecord.Fields[FIELD.EASTUDENTS_fld_ONLINEUSERID] = value;
            }
        }

        public ProgressionEntryCollection ProgressionEntries {
            get {
                // Make sure our progression entries are loaded
                LoadProgressionEntries();

                return progressionEntries;
            }
        }

        public RelationshipCollection Relationships {
            get {
                if (relationships == null) {
                    relationships = new RelationshipCollection(bbRecord.RelationShips);
                }

                return relationships;
            }
        }

        public string SSN {
            get {
                return (string) bbRecord.Fields[FIELD.EASTUDENTS_fld_SSN];
            }
            set {
                bbRecord.Fields[FIELD.EASTUDENTS_fld_SSN] = value;
            }
        }

        public StudentCourseCollection StudentCourses {
            get {
                // If we haven't loaded our studentcourses yet
                if (studentCourses == null) {
                    cEAStudentCourses bbCollection = new cEAStudentCourses();
                    bbCollection.Init(context);
                    bbCollection.FilterObject.FilterProperties[
                    EEASTUDENTCOURSESFilterProperties.EASTUDENTCOURSES_FilterProp_EA7StudentsID] =
                        Ea7StudentsId;

                    studentCourses = new StudentCourseCollection(bbCollection, context);

                    //studentCourses = new StudentCourseCollection(
                    //    (cEAStudentCourses) bbRecord.StudentCourses, context);
                }

                return studentCourses;
            }
        }

        public List<StudentSession> StudentSessions {
            get {
                // If we haven't loaded our student sessions yet
                if (studentSessions == null) {
                    studentSessions = new List<StudentSession>();

                    cEAStudentSessions bbObjects = new cEAStudentSessions();
                    bbObjects.Init(context);
                    bbObjects.FilterObject.FilterProperties[
                        EEASTUDENTSESSIONSFilterProperties.EASTUDENTSESSIONS_FilterProp_STUDENTID]
                        = Ea7StudentsId;

                    // Load each of our student sessions into a StudentSession object
                    foreach (CEAStudentSession ss in bbObjects) {
                        studentSessions.Add(new StudentSession(ss));
                    }

                    bbObjects.CloseDown();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(bbObjects);
                    bbObjects = null;
                }

                return studentSessions;
            }
        }

        public cEAStudent BbSisObject {
            get {
                return bbRecord;
            }
        }

        public CurrentStatusLogEntry CurrentStatus {
            get {
                cEARecordStatusLog readOnlyObjectFromStatusLogs = StatusLog.First(
                    stat => stat.IsCurrent && stat.IsOnActiveEnrollment).BbSisObject;

                return new CurrentStatusLogEntry(bbRecord, readOnlyObjectFromStatusLogs);
            }
        }

        public ProgressionEntry AddProgressionEntry() {
            cEAPromotionSummary sisProg = bbRecord.PromotionSummaries.Add();

            // Create a new ProgressionEntry wrapper object
            var newProgressionEntry = new ProgressionEntry(sisProg);

            // Make sure our list of ProgressionEntry objects is loaded
            LoadProgressionEntries();

            // Add the new ProgressionEntry to our list
            progressionEntries.Add(newProgressionEntry);

            return newProgressionEntry;
        }

        private void LoadProgressionEntries() {
            // If we haven't loaded our progression entries yet
            if (progressionEntries == null) {
                progressionEntries = new ProgressionEntryCollection(bbRecord.PromotionSummaries);
            }
        }

        public void Reload() {
            // Save our Ea7StudentsId
            int ea7StudentsId = Ea7StudentsId;

            // Close the SIS record
            Close();

            // Load the same student again
            bbRecord = LoadSisRecord(ea7StudentsId, context);
        }

        public void Save() {
            bbRecord.Save();
        }

        public void Close() {
            if (bbRecord == null) return;

            // If we have studentCourses loaded
            if (studentCourses != null) {
                // Close each of our student courses
                foreach (StudentCourse sc in studentCourses) {
                    sc.Close();
                }

                // Close the collection
                studentCourses.Close();

                // Clear our list of student courses
                studentCourses = null;
            }

            // If we have addresses loaded
            if (addresses != null) {
                // Clear our list of addresses
                addresses = null;
            }

            // If we have attributes loaded
            if (attributes != null) {
                // Clear our list of addresses
                attributes = null;
            }

            // If we have notes loaded
            if (notes != null) {
                // Clear our list of notes
                notes = null;
            }

            // If we have progression entries loaded
            if (progressionEntries != null) {
                // Clear our list of progression entries
                progressionEntries = null;
            }

            // If we have relationships loaded
            if (relationships != null) {
                // Clear our list of relationships
                relationships = null;
            }

            // If we have degrees loaded
            if (degrees != null) {
                degrees.Close();

                // Clear our list of degrees
                degrees = null;
            }

            // If we have enrollments loaded
            if (enrollments != null) {
                //enrollments.Close();

                // Clear our list of enrollments
                enrollments = null;
            }

            // Release our handle on the SIS student record
            bbRecord.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbRecord);
            bbRecord = null;
        }

        ~Student() {
            Close();
        }

        private static cEAStudent LoadSisRecord(int ea7StudentsId, IBBSessionContext context) {
            var record = new cEAStudent();
            record.Init(context);
            record.Load(ea7StudentsId);

            return record;
        }

        public static Student LoadByEa7StudentsId(int ea7StudentsId, Context context) {
            return LoadByEa7StudentsId(ea7StudentsId, context.BbSisContext);
        }

        internal static Student LoadByEa7StudentsId(int ea7StudentsId, IBBSessionContext context) {
            var sisRecord = LoadSisRecord(ea7StudentsId, context);
            return new Student(sisRecord, context);
        }

        public static Student LoadByEa7RecordsId(int ea7RecordsId, Context context) {
            return LoadByEa7RecordsId(ea7RecordsId, context.BbSisContext);
        }

        private static Student LoadByEa7RecordsId(int ea7RecordsId, IBBSessionContext context) {
            var records = new cEAStudents();
            records.Init(context);
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMFROM] =
                "EA7STUDENTS " +
                "join EA7RECORDS " +
                "    on EA7RECORDS.EA7RECORDSID = EA7STUDENTS.EA7RECORDSID";
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                "EA7RECORDS.EA7RECORDSID = " + ea7RecordsId.ToString();
            
            int matchId = -1;

            // If there was exactly one matching record
            if (records.Count() == 1) {
                // Store the matching record's ID
                matchId = int.Parse((string)
                    records.Item(1).Fields[FIELD.EASTUDENTS_fld_EA7STUDENTSID]);
            }

            // Release our handle on the record collection
            records.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(records);
            records = null;

            // If we found a matching ID to load
            if (matchId != -1) {
                return LoadByEa7StudentsId(matchId, context);
            }
            else {
                return null;
            }
        }

        public static IEnumerable<Student>
        LoadCollection(
            Context context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            return LoadCollection(context.BbSisContext, sqlFrom, sqlWhere, sqlOrderBy);
        }

        private static IEnumerable<Student>
        LoadCollection(
            IBBSessionContext context,
            string sqlFrom = null,
            string sqlWhere = null,
            string sqlOrderBy = null)
        {
            cEAStudents bbCollection = new cEAStudents();
            bbCollection.Init(context);

            if (sqlFrom != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMFROM] = sqlFrom;
            }
            if (sqlWhere != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMWHERE] = sqlWhere;
            }
            if (sqlOrderBy != null) {
                bbCollection.FilterObject.CustomFilterProperty[
                    FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMORDERBY] = sqlOrderBy;
            }

            foreach (cEAStudent bbObject in bbCollection) {
                yield return new Student(bbObject, context);
            }

            bbCollection.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(bbCollection);
            bbCollection = null;
        }

        public static Student
        LoadByUserDefinedId(string userDefinedId, Context context) {
            return LoadByUserDefinedId(userDefinedId, context.BbSisContext);
        }

        private static Student
        LoadByUserDefinedId(string userDefinedId, IBBSessionContext context) {
            userDefinedId = userDefinedId.Replace("'", "''").Trim();

            var records = new cEAStudents();
            records.Init(context);
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMFROM] =
                "EA7STUDENTS " +
                "join EA7RECORDS " +
                "    on EA7RECORDS.EA7RECORDSID = EA7STUDENTS.EA7RECORDSID";
            records.FilterObject.CustomFilterProperty[FILTERTYPE.CUSTOMFILTERTYPE_CUSTOMWHERE] =
                "EA7RECORDS.USERDEFINEDID = '" + userDefinedId + "'";
            
            int matchId = -1;

            // If there was exactly one matching record
            if (records.Count() == 1) {
                // Store the matching record's ID
                matchId = int.Parse((string)
                    records.Item(1).Fields[FIELD.EASTUDENTS_fld_EA7STUDENTSID]);
            }

            // Release our handle on the record collection
            records.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(records);
            records = null;

            // If we found a matching ID to load
            if (matchId != -1) {
                return LoadByEa7StudentsId(matchId, context);
            }
            else {
                return null;
            }
        }
    
        public void Dispose() {
            Close();
        }
    }
}