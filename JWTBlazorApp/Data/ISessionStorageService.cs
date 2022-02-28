using System;
using System.Threading.Tasks;

namespace JWTBlazorApp.Data
{
    public interface ISessionStorageService
    {
        Task ClearAsync();
        Task<T> GetItemAsync<T>(string key);
        Task<string> KeyAsync(int index);
        Task RemoveItemAsync(string key);
        Task<int> LengthAsync();
        Task SetItemAsync(string key, object data);
        event EventHandler<ChangedEventArgs> Changed;
        event EventHandler<ChangingEventArgs> Changing;
    }
}