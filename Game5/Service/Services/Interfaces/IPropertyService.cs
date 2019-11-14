namespace Game5.Service.Services.Interfaces
{
    public interface IPropertyService
    {
        void SetProperty<T>(string key, T property);
        T GetProperty<T>(string key, T defaultValue = default(T));
        void Save(string file);
        void Load(string file);
    }
}