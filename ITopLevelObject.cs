namespace BbSisWrapper {
    public interface ITopLevelObject {
        void Close();
        void Save();
        void Reload();


        bool CanBeSaved { get; }
        ReasonRecordCannotBeSaved ReasonRecordCannotBeSaved { get; }
    }
}