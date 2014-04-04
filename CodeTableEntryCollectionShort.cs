using Blackbaud.PIA.FE7.BBAFNAPI7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbSisWrapper {
    public class CodeTableEntryCollectionShort : CodeTableEntryCollection {
        internal CodeTableEntryCollectionShort(int codeTablesId, CCodeTablesServer codeTableServer,
                                               IBBSessionContext context) :
                 base(codeTablesId, codeTableServer, context) {
        }

        public CodeTableEntry Add(string longDescription, string shortDescription, bool isActive) {
            CodeTableEntry newItem = AddEntry(shortDescription, longDescription, isActive);
            Items.Add(newItem);

            return newItem;
        }
    }
}