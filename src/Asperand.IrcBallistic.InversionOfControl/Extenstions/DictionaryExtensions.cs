using System.Collections.Generic;
using System.Linq;

namespace Asperand.IrcBallistic.InversionOfControl.Extenstions
{
    public static class DictionaryExtensions
    {
        
        public static void AddCreate<T, TT>(this Dictionary<T, List<TT>> dict, T key, TT value)
        {
            if (dict.ContainsKey(key))
                dict[key].Add(value);
            else
                dict.Add(key, new List<TT> {value});
        }
        
        
        public static void TryRemove<T, TT>(this Dictionary<T, TT> dict, T key)
        {
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
            }
        }

        public static List<TT> FlattenToList<T, TT>(this Dictionary<T, List<TT>> dict)
        {
            return dict.Values.SelectMany(x => x).ToList();
        }
    }
}