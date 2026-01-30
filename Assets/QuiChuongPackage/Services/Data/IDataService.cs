public interface IDataService
{
    GameData Data { get; }

    void Load();
    void Save();
    void Reset();
}