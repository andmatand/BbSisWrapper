using System;
using Blackbaud.PIA.FE7.BBAFNAPI7;

namespace BbSisWrapper {
    public class ApiConnection {
        private Blackbaud.PIA.FE7.BBAFNAPI7.FE_API api;
        private CodeTableServer codeTableServer;

        public ApiConnection(string serialNumber, int databaseNumber, string username, string password) {
            api = new FE_API();

            if (!api.Init(serialNumber, username, password, databaseNumber,
                          null, Blackbaud.PIA.FE7.BBAFNAPI7.AppMode.amServer)) {
                throw new Exception("Could not connect to SIS: " + api.LastErrorMessage);
            }
            else {
                api.SignOutOnTerminate = true;
            }
        }

        public void Close() {
            if (api != null) {
                if (codeTableServer != null) {
                    // Close the codeTableServer
                    codeTableServer.Close();
                }

                // Release the API COM Object
                System.Runtime.InteropServices.Marshal.ReleaseComObject(api);
                api = null;
            }
        }

        public CodeTableServer CodeTableServer {
            get {
                if (codeTableServer == null) {
                    codeTableServer = new CodeTableServer((IBBSessionContext) Context);
                }

                return codeTableServer;
            }
        }

        public Blackbaud.PIA.EA7.BBEEAPI7.IBBSessionContext Context {
            get {
                return (Blackbaud.PIA.EA7.BBEEAPI7.IBBSessionContext) api.SessionContext;
            }
        }
    }
}