using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage.Provider;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.Provider
{
    /// <summary>
    /// Server-side in-memory storage provider.
    /// Uses a static ConcurrentDictionary so data persists across scoped lifetimes
    /// (lost only on application restart).
    /// </summary>
    internal class ServerStorageProvider : IStorageProvider
    {
        private static readonly ConcurrentDictionary<string, string> _storage = new();

        public ValueTask ClearAsync()
        {
            _storage.Clear();
            return ValueTask.CompletedTask;
        }

        public ValueTask<string> GetItemAsync(string key)
        {
            _storage.TryGetValue(key, out var value);
            return ValueTask.FromResult(value ?? string.Empty);
        }

        public ValueTask<string> KeyAsync(int index)
        {
            var keys = _storage.Keys.ToList();
            return ValueTask.FromResult(index >= 0 && index < keys.Count ? keys[index] : null);
        }

        public ValueTask<bool> ContainKeyAsync(string key)
            => ValueTask.FromResult(_storage.ContainsKey(key));

        public ValueTask<int> LengthAsync()
            => ValueTask.FromResult(_storage.Count);

        public ValueTask RemoveItemAsync(string key)
        {
            _storage.TryRemove(key, out _);
            return ValueTask.CompletedTask;
        }

        public ValueTask SetItemAsync(string key, string data)
        {
            _storage[key] = data;
            return ValueTask.CompletedTask;
        }

        public void Clear()
            => _storage.Clear();

        public string GetItem(string key)
        {
            _storage.TryGetValue(key, out var value);
            return value ?? string.Empty;
        }

        public string Key(int index)
        {
            var keys = _storage.Keys.ToList();
            return index >= 0 && index < keys.Count ? keys[index] : null;
        }

        public bool ContainKey(string key)
            => _storage.ContainsKey(key);

        public int Length()
            => _storage.Count;

        public void RemoveItem(string key)
            => _storage.TryRemove(key, out _);

        public void SetItem(string key, string data)
            => _storage[key] = data;
    }
}