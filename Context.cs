using System;
using Blackbaud.PIA.FE7.BBAFNAPI7;

namespace BbSisWrapper {
    public class Context {
        private IBBSessionContext bbContext;

        internal Context(IBBSessionContext bbSisContext) {
            this.bbContext = bbSisContext;
        }

        public Blackbaud.PIA.EA7.BBEEAPI7.IBBSessionContext BbSisContext {
            get { return (Blackbaud.PIA.EA7.BBEEAPI7.IBBSessionContext) bbContext; }
        }
    }
}