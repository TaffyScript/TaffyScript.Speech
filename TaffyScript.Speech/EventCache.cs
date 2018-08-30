/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaffyScript.Speech
{
    public class EventCache<T>
    {
        private Dictionary<T, List<KeyValuePair<TsDelegate, Delegate>>> _cache = new Dictionary<T, List<KeyValuePair<TsDelegate, Delegate>>>();

        public void Cache(T key, TsDelegate del, Delegate handler)
        {
            if(!_cache.TryGetValue(key, out var list))
            {
                list = new List<KeyValuePair<TsDelegate, Delegate>>();
                _cache.Add(key, list);
            }
            list.Add(new KeyValuePair<TsDelegate, Delegate>(del, handler));
        }

        public bool TryRemove(T key, TsDelegate del, out Delegate handler)
        {
            if (!_cache.TryGetValue(key, out var list))
            {
                handler = null;
                return false;
            }

            foreach(var item in list)
            {
                if (item.Key == del)
                {
                    handler = item.Value;
                    return true;
                }
            }

            handler = null;
            return false;
        }
    }
}
*/