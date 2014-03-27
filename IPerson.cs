using System;

namespace BbSisWrapper {
    public interface IPerson : ITopLevelObject {
        DateTime DateAdded { get; }
        string IdNumber { get; }
        string FirstName { get; }
        string Nickname { get; }
        string MiddleName { get; }
        string LastName { get; }
        string OnlineUserId { get; set; }
        string OnlinePassword { get; set; }
        AddressCollection Addresses { get; }
    }
}