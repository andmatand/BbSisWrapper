using Blackbaud.PIA.EA7.BBEEAPI7;
using System;

namespace BbSisWrapper {
    public partial class Student {
        public class Note {
            private IBBNotepad sisObject;

            public Note(IBBNotepad bbSisObject) {
                this.sisObject = bbSisObject;
            }

            public DateTime Date {
                get {
                    return DateTime.Parse((string)
                        sisObject.Fields[ENotepadFields.NOTEPAD_fld_NotepadDate]);
                }
                set {
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_NotepadDate] = value;
                }
            }

            public string Description {
                get {
                    return (string) sisObject.Fields[ENotepadFields.NOTEPAD_fld_Description];
                }
            }

            public string Notes {
                get {
                    return (string) sisObject.Fields[ENotepadFields.NOTEPAD_fld_ActualNotes];
                }
                set {
                    // Set the "Notes" field to preserve linebreaks
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_Notes] = value;

                    // Also set the "ActualNotes" field so it shows in the "Notes" column on the
                    // notes tab of the student record
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_ActualNotes] = value;
                }
            }

            public bool PrintOnReportCard {
                get {
                    dynamic value = sisObject.Fields[ENotepadFields.NOTEPAD_fld_XtraBoolean2];
                    return (Enums.Parse<bbTF>(value) == bbTF.bbTrue);
                }
                set {
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_XtraBoolean2] = (
                        value ? bbTF.bbTrue : bbTF.bbFalse);
                }
            }

            public bool PrintOnTranscript {
                get {
                    dynamic value = sisObject.Fields[ENotepadFields.NOTEPAD_fld_XtraBoolean1];
                    return (Enums.Parse<bbTF>(value) == bbTF.bbTrue);
                }
                set {
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_XtraBoolean1] = (
                        value ? bbTF.bbTrue : bbTF.bbFalse);
                }
            }

            public string Title {
                get {
                    return (string) sisObject.Fields[ENotepadFields.NOTEPAD_fld_Title];
                }
                set {
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_Title] = value;
                }
            }

            public string Type {
                get {
                    return (string) sisObject.Fields[ENotepadFields.NOTEPAD_fld_NotepadType];
                }
                set {
                    sisObject.Fields[ENotepadFields.NOTEPAD_fld_NotepadType] = value;
                }
            }
        }
    }
}