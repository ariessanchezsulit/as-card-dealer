#region Header

// Locator.cs
// Aries Sanchez Sulit
// 2023-02-01 at 5:30 PM

#endregion

using System;
using System.Collections.Generic;

namespace PP.Tools.Common
{
    public class Locator
    {
        protected readonly Dictionary<Type, ILocatable> _locatables = new Dictionary<Type, ILocatable>();

        public bool Contains<T>() where T : ILocatable
        {
            var type = typeof(T);
            return _locatables.ContainsKey(type);
        }

        public T Get<T>() where T : ILocatable
        {
            var type = typeof(T);

            if (_locatables.ContainsKey(type)) return (T)_locatables[type];

            return default(T);
        }
        
        public bool TryGet<T>(out T locatable) where T : ILocatable
        {
            var type = typeof(T);

            if (_locatables.ContainsKey(type))
            {
                locatable = (T)_locatables[type];
                return true;
            }

            locatable = default(T);
            return false;
        }

        public void Register<T>(T instance, bool dispose = false) where T : ILocatable
        {
            var type = typeof(T);

            if (_locatables.ContainsKey(type) && dispose)
            {
                _locatables[type].Dispose();
            }

            _locatables[type] = instance;
        }

        private void Clear()
        {
            foreach (var pair in _locatables)
            {
                pair.Value.Dispose();
            }

            _locatables.Clear();
        }
    }
}