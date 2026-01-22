using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Common.Context
{
    public static class AsyncStorage<T> where T : new()
    {
        private static readonly AsyncLocal<T> _asyncLocal = new AsyncLocal<T>();

        public static T Store(T val)
        {
            _asyncLocal.Value = val;
            return _asyncLocal.Value;
        }

        public static T? Retrieve()
        {
            return _asyncLocal.Value;
        }
    }
}
