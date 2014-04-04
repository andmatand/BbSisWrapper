using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbSisWrapper {
    public class CodeTableEntryShort : CodeTableEntry {
        private string shortDescription;

        public CodeTableEntryShort(int tableEntriesId, string description, string shortDescription,
                                   bool isActive) : base(tableEntriesId, description, isActive) {
            this.shortDescription = shortDescription;
        }

        public string LongDescription {
            get {
                return Description;
            }
        }

        public string ShortDescription {
            get {
                return shortDescription;
            }
        }
    }
}