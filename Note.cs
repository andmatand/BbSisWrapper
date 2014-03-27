using Blackbaud.PIA.EA7.BBEEAPI7;
using System;

namespace BbSisWrapper {
    public partial class Student {
        public class Note {
            private IBBNotepad sisObject;

            public Note(IBBNotepad sisObject) {
                this.sisObject = sisObject;
            }

            public DateTime Date {
                get {
                    return DateTime.Parse((string)
                        sisObject.Fields[ENotepadFields.NOTEPAD_fld_NotepadDate]);
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
            }

            public string Title {
                get {
                    return (string) sisObject.Fields[ENotepadFields.NOTEPAD_fld_Title];
                }
            }

            public string Type {
                get {
                    return (string) sisObject.Fields[ENotepadFields.NOTEPAD_fld_NotepadType];
                }
            }
        }
    }
}