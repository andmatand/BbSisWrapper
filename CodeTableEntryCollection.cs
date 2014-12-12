using System;
using System.Collections.Generic;
using Blackbaud.PIA.FE7.BBAFNAPI7;

namespace BbSisWrapper {
    public class CodeTableEntryCollection : ICollection<CodeTableEntry> {
        private int codeTableId;
        private CCodeTablesServer server;
        private IBBSessionContext context;
        private List<CodeTableEntry> wrapperCollection;

        internal CodeTableEntryCollection(int codeTablesId, CCodeTablesServer codeTableServer,
                                          IBBSessionContext context) {
            this.codeTableId = codeTablesId;
            this.server = codeTableServer;
            this.context = context;

            int entryCount = server.GetTableEntryCount((ECodeTableNumbers) codeTableId);
            dynamic[,] tableData = (dynamic[,])
                server.CodeTableGetDataArray((ECodeTableNumbers) codeTableId, false, false);

            wrapperCollection = new List<CodeTableEntry>();

            // Wrap each entry in a CodeTableEntry object
            for (int i = 0; i < entryCount; i++) {
                int entryId = (int) tableData[0, i];
                wrapperCollection.Add(
                    CodeTableEntry.LoadByTableEntriesId(entryId, server, codeTablesId));
            }
        }

        public CodeTableEntry Add(string description, bool isActive = true) {
            if (HasShortDescriptions) {
                throw new Exception("This CodeTable has short descriptions.  Use the other " +
                                    "Add() method.");
            }

            CodeTableEntry newItem = AddEntry(null, description, isActive);
            wrapperCollection.Add(newItem);

            return newItem;
        }

        public CodeTableEntry
        Add(string longDescription, string shortDescription, bool isActive = true) {
            if (!HasShortDescriptions) {
                throw new Exception("This CodeTable does not have short descriptions.  Use the " +
                                    "other Add() method.");
            }

            CodeTableEntry newItem = AddEntry(shortDescription, longDescription, isActive);
            wrapperCollection.Add(newItem);

            return newItem;
        }


        private CodeTableEntry
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

        private bool HasShortDescriptions {
            get {
                return server.TableHasShortDescription((ECodeTableNumbers) codeTableId);
            }
        }

        public void Add(CodeTableEntry item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains(CodeTableEntry item) {
            throw new NotSupportedException();
        }

        public void CopyTo(CodeTableEntry[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return wrapperCollection.Count; }
        }

        public bool IsReadOnly {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(CodeTableEntry item) {
            throw new NotImplementedException();
        }

        public IEnumerator<CodeTableEntry> GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return wrapperCollection.GetEnumerator();
        }
    }
}