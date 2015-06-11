using Blackbaud.PIA.EA7.BBEEAPI7;
using System;
using FIELDS = Blackbaud.PIA.EA7.BBEEAPI7.EEASTUDENTGRADESFIELDS;

namespace BbSisWrapper {
    public partial class Student {
        public partial class StudentCourse {
            public class Grade {
                private CEAGrade sisObject;

                public Grade(CEAGrade grade) {
                    this.sisObject = grade;
                }

                public string LetterGrade {
                    get {
                        return (string) sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_GRADE];
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_GRADE] = value;
                    }
                }

                public int Ea7TranslationEntriesId {
                    get {
                        return int.Parse((string)
                            sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_EA7TRANSLATIONENTRIESID]);
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_EA7TRANSLATIONENTRIESID] = value;
                    }
                }

                public decimal CreditAttempted {
                    get {
                        return decimal.Parse((string)
                            sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_CREDITATTEMPTED]);
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_CREDITATTEMPTED] = value;
                    }
                }

                public decimal CreditAwarded {
                    get {
                        decimal credit;
                        decimal.TryParse((string) sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_CREDITAWARDED], out credit);
                        return credit;
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_CREDITAWARDED] = value;
                    }
                }

                public static int? LookupMarkingColumnId(string markingColumn, IBBSessionContext context) {
                    int returnId = -1;

                    var markingColumnSets = new cEAMarkingColumnSets();
                    markingColumnSets.Init(context);

                    foreach (cEAMarkingColumnSet set in markingColumnSets) {
                        if ((string) set.Fields[EEAMarkingColumnSetFields.EAMARKINGCOLUMNSET_fld_NAME] == "Midterm and Final") {
                            foreach (cEAMarkingColumn mc in set.MarkingColumns) {
                                if ((string) mc.Fields[EEAMARKINGCOLUMNFIELDS.EAMARKINGCOLUMNS_fld_COLUMNID] == markingColumn) {
                                    int.TryParse((string) mc.Fields[EEAMARKINGCOLUMNFIELDS.EAMARKINGCOLUMNS_fld_EA7MARKINGCOLUMNSID], out returnId);
                                    break;
                                }
                            }
                        }
                    }
                    markingColumnSets.CloseDown();

                    if (returnId == -1) {
                        return null;
                    }
                    else {
                        return returnId;
                    }
                }

                public static int LookupTranslationEntryId(int translationId, string grade, IBBSessionContext context) {
                    int returnId = -1;

                    var translationTables = new cEATranslations();
                    translationTables.Init(context);
                    foreach (cEATranslation translation in translationTables) {
                        if ((string) translation.Fields[EEATRANSLATIONFIELDS.EATRANSLATIONS_fld_EA7TRANSLATIONSID] == translationId.ToString()) {
                            foreach (cEATranslationEntry entry in translation.Entries) {
                                if ((string) entry.Fields[EEATRANSLATIONENTRIESFIELDS.EATRANSLATIONENTRIES_fld_GRADE] == grade) {
                                    int.TryParse((string) entry.Fields[EEATRANSLATIONENTRIESFIELDS.EATRANSLATIONENTRIES_fld_EA7TRANSLATIONENTRIESID], out returnId);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    translationTables.CloseDown();

                    if (returnId == -1) {
                        throw new Exception("No Translation Entry ID was found.");
                    }
                    else {
                        return returnId;
                    }
                }

                public string MarkingColumn {
                    get {
                        return (string) sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_MARKINGCOLUMN];
                    }
                }

                public int MarkingColumnId {
                    get {
                        return int.Parse((string) sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_EA7MARKINGCOLUMNSID]);
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_EA7MARKINGCOLUMNSID] = value;
                    }
                }

                public bool PrintOnTranscripts {
                    get {
                        return ((bbTF) Enum.Parse(typeof(bbTF),
                            (string) sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_PRINTONTRANSCRIPTS])
                            == bbTF.bbTrue);
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_PRINTONTRANSCRIPTS] = (value ? bbTF.bbTrue : bbTF.bbFalse);
                    }
                }

                public bool UseInCalculations {
                    get {
                        return ((bbTF) Enum.Parse(typeof(bbTF),
                            (string) sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_USEINCALCULATIONS])
                            == bbTF.bbTrue);
                    }
                    set {
                        sisObject.Fields[FIELDS.EASTUDENTGRADES_fld_USEINCALCULATIONS] = (value ? bbTF.bbTrue : bbTF.bbFalse);
                    }
                }
            }
        }
    }
}