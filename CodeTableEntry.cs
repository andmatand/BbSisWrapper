using Blackbaud.PIA.FE7.BBAFNAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbSisWrapper {
    public class CodeTableEntry {
        private int tableEntriesId;
        private string description;
        private bool isActive;

        internal CodeTableEntry(int tableEntriesId, string description, bool isActive) {
            this.tableEntriesId = tableEntriesId;
            this.description = description;
            this.isActive = isActive;
        }

        internal static CodeTableEntry LoadByTableEntriesId(int tableEntriesId,
                                                            CCodeTablesServer server,
                                                            int codeTablesId) {
            string description = server.GetTableEntryDescription(tableEntriesId,
                                                                 (ECodeTableNumbers) codeTablesId);
            bool isActive = server.GetTableEntryActiveStatus(description,
                                                             (ECodeTableNumbers) codeTablesId);

            // If this Code Table has short descriptions
            if (server.TableHasShortDescription((ECodeTableNumbers) codeTablesId)) {
                string shortDescription = server.GetTableEntryShortDescription(
                    tableEntriesId, (ECodeTableNumbers) codeTablesId);

                return new CodeTableEntryShort(tableEntriesId, description, shortDescription,
                                               isActive);
            }
            else {
                return new CodeTableEntry(tableEntriesId, description, isActive);
            }
        }

        public int TableEntriesId {
            get {
                return tableEntriesId;
            }
        }

        public string Description {
            get {
                return description;
            }
        }

        public bool IsActive {
            get {
                return isActive;
            }
        }
    }
}