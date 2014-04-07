using Blackbaud.PIA.FE7.BBAFNAPI7;

namespace BbSisWrapper {
    public class CodeTable {
        private CCodeTable bbObject;
        private CCodeTablesServer codeTableServer;
        private CodeTableEntryCollection entries;
        private IBBSessionContext context;

        internal CodeTable(CCodeTable bbObject, CCodeTablesServer codeTableServer,
                           IBBSessionContext context) {
            this.bbObject = bbObject;
            this.codeTableServer = codeTableServer;
            this.context = context;
        }

        public CodeTableEntryCollection Entries {
            get {
                if (entries == null) {
                    entries = new CodeTableEntryCollection(CodeTablesId, codeTableServer, context);
                }

                return entries;
            }
        }

        public int CodeTablesId {
            get {
                return int.Parse((string) bbObject.Fields[ECodeTableFields.ctfCODETABLEID]);
            }
        }

        public bool HasShortDescriptions {
            get {
                return codeTableServer.TableHasShortDescription((ECodeTableNumbers) CodeTablesId);
            }
        }


        public static CodeTable LoadByCodeTablesId(int codeTablesId,
                                                   CodeTableServer codeTableServer,
                                                   IBBSessionContext context) {
            foreach (CCodeTable bbCodeTable in codeTableServer.BbObject.CodeTables) {
                if (int.Parse((string) bbCodeTable.Fields[ECodeTableFields.ctfCODETABLEID]) ==
                    codeTablesId) {
                    return new CodeTable(bbCodeTable, codeTableServer.BbObject, context);
                }

                bbCodeTable.CloseDown();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(bbCodeTable);
            }

            return null;
        }

        public static CodeTable
        LoadByName(string name, CodeTableServer codeTableServer) {
            // Find the ID of the code table that has the given name
            int id = -1;
            foreach (CCodeTable bbCodeTable in codeTableServer.BbObject.CodeTables) {
                if (((string) bbCodeTable.Fields[ECodeTableFields.ctfNAME]).ToLower() ==
                    name.ToLower()) {
                    id = int.Parse((string) bbCodeTable.Fields[ECodeTableFields.ctfCODETABLEID]);
                    break;
                }
            }

            // If the name matched a code table
            if (id != -1) {
                return LoadByCodeTablesId(id, codeTableServer, codeTableServer.Context);
            }
            else {
                return null;
            }
        }
    }
}