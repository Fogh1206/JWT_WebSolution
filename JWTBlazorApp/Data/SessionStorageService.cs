using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace JWTBlazorApp.Data
{
    public class SessionStorageService : ISessionStorageService
    {

        private readonly IJSRuntime _jsRuntime;

        public SessionStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public event EventHandler<ChangedEventArgs> Changed;
        public event EventHandler<ChangingEventArgs> Changing;
        
        // Session clear items.

        public async Task ClearAsync() => await _jsRuntime.InvokeAsync<object>("sessionStorage.clear");

        // Session get item.
        
        public async Task<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var serialisedData = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", key);

            return serialisedData is null ? default(T) : JsonSerializer.Deserialize<T>(serialisedData);
        }
        
        // Session key

        public async Task<string> KeyAsync(int index) => await _jsRuntime.InvokeAsync<string>("sessionStorage.key", index);

        public async Task<int> LengthAsync() => await _jsRuntime.InvokeAsync<int>("eval", "sessionStorage.length");

        public async Task RemoveItemAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            
            await _jsRuntime.InvokeAsync<object>("sessionStorage.removeItem", key);
            
        }

        public async Task SetItemAsync(string key, object data)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }
            var e = RaiseOnChangingSync(key, data);
            if (e.Cancel) return;
            
            await  _jsRuntime.InvokeAsync<object>("sessionStorage.setItem", key, JsonSerializer.Serialize(data));
            
        }
        
        private ChangingEventArgs RaiseOnChangingSync(string key, object data)
        {
            var e = new ChangingEventArgs
            {
                Key = key,
                NewValue = data
            };
            Changing?.Invoke(this, e);
            return e;
        }

    }
}