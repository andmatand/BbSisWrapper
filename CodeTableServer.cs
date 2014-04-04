using Blackbaud.PIA.FE7.BBAFNAPI7;
using System;

namespace BbSisWrapper {
    public class CodeTableServer {
        private CCodeTablesServer bbObject;

        internal CodeTableServer(IBBSessionContext context) {
            // Create and initialize a BB code table server object
            CCodeTablesServer bbObject = new CCodeTablesServer();
            bbObject.Init(context);
        }

        ~CodeTableServer() {
            Close();
        }

        public CCodeTablesServer BbObject {
            get {
                return bbObject;
            }
        }

        public void Close() {
            if (bbObject != null) {
                // Release our handle on the code table server
                bbObject.CloseDown();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(bbObject);
                bbObject = null;
            }
        }
    }
}