using Blackbaud.PIA.FE7.BBAFNAPI7;

namespace BbSisWrapper {
    public class CodeTableServer {
        private CCodeTablesServer bbObject;
        private IBBSessionContext context;

        internal CodeTableServer(IBBSessionContext context) {
            this.context = context;

            // Create and initialize a BB code table server object
            bbObject = new CCodeTablesServer();
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

        internal IBBSessionContext Context {
            get {
                return context;
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