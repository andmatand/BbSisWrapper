using Blackbaud.PIA.FE7.BBAFNAPI7;
using System;
using System.Collections.ObjectModel;

namespace BbSisWrapper {
    public class CodeTableEntryCollection : Collection<CodeTableEntry> {
        private int codeTableId;
        private CCodeTablesServer server;
        private IBBSessionContext context;

        internal CodeTableEntryCollection(int codeTablesId, CCodeTablesServer codeTableServer,
                                          IBBSessionContext context) {
            this.codeTableId = codeTablesId;
            this.server = codeTableServer;
            this.context = context;

            int entryCount = server.GetTableEntryCount((ECodeTableNumbers) codeTableId);
            dynamic[][] tableData = (dynamic[][])
                server.CodeTableGetDataArray((ECodeTableNumbers) codeTableId, false, false);

            for (int i = 0; i < entryCount; i++) {
                int entryId = (int) tableData[0][i];
                Items.Add(CodeTableEntry.LoadByTableEntriesId(entryId, server, codeTablesId));
            }

            //for (int i = 0; i < entryCount; i++) {
            //    int id = (int) tableData[0][i];
            //    string description = tableData[1][i];

            //    bool isActive = server.GetTableEntryActiveStatus(description,
            //                                                     (ECodeTableNumbers) codeTableId);

            //    string shortDescription = null;

            //    // If this table has short descriptions
            //    if (HasShortDescriptions) {
            //        shortDescription = tableData[2][i];

            //        if (string.IsNullOrWhiteSpace(shortDescription)) {
            //            shortDescription = null;
            //        }

            //        Items.Add(new CodeTableEntryShort(id, description, shortDescription,
            //                                          isActive));
            //    }
            //    else {
            //        Items.Add(new CodeTableEntry(id, description, isActive));
            //    }
            //}
        }

        //public bool HasShortDescriptions {
        //    get {
        //        return server.TableHasShortDescription((ECodeTableNumbers) codeTableId);
        //    }
        //}

        public CodeTableEntry Add(string description, bool isActive = true) {
            CodeTableEntry newItem = AddEntry(null, description, isActive);
            Items.Add(newItem);

            return newItem;
        }

        protected CodeTableEntry
        AddEntry(string shortDescription, string description, bool isActive) {
            var tableLookupHandler = new CTableLookupHandler();
            tableLookupHandler.Init(context);
            tableLookupHandler.ReadOnly = false;

            CodeTableEntry newEntry = null;

            if (tableLookupHandler.AddEntry(true, codeTableId, shortDescription, description)) {
                // Get the ID of the item with the description we just added
                int entryId = server.GetTableEntryID(description, (ECodeTableNumbers) codeTableId);

                // Create a wrapper CodeTableEntry object for the new entry
                newEntry = CodeTableEntry.LoadByTableEntriesId(entryId, server, codeTableId);
            }

            tableLookupHandler.CloseDown();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(tableLookupHandler);
            tableLookupHandler = null;

            return newEntry;
        }
    }
}