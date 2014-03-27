using Blackbaud.PIA.EA7.BBEEAPI7;
using System;

namespace BbSisWrapper {
    public interface IRelationship {
        int ParentId { get; }
        int RelationId { get; }
    }
}