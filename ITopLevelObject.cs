using System;

namespace BbSisWrapper {
    public interface ITopLevelObject : IDisposable {
        void Close();
        void Save();
        void Reload();

        bool CanBeSaved { get; }
        ReasonRecordCannotBeSaved ReasonRecordCannotBeSaved { get; }
    }
}