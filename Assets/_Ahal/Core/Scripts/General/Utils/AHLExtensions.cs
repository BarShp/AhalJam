using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AHL.Core.General.Utils
{
    public static class AHLExtensions
    {

        public static bool IsNullOrEmpty<T, T1>(this Dictionary<T, T1> source)
        {
            return source is not {Count: > 0};
        }

        public static T GetRandomItem<T>(this IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            return enumerable1.ElementAt(Random.Range(0, enumerable1.Length));
        }

        public static void SetMaterialColor(this Material material, Color color)
        {
            var id = Shader.PropertyToID("_Color");
            material.SetColor(id, color);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }
        
        public static bool IsNotNullOrEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }
        
        public static IEnumerable<TValue> DistinctBy<TValue, TDistinct>(this IEnumerable<TValue> source, Func<TValue, TDistinct> predicate)
        {
            var set = new HashSet<TDistinct>();
            
            foreach (var value in source)
            {
                if (set.Add(predicate(value)))
                {
                    yield return value;
                }
            }
        }

        public static string ToStringCommaSeperated<K, V>( this Dictionary<K, V> collection, bool separateLines = false )
        {
            if( collection == null )
                return "null";
            if( collection.Count <= 0 )
                return "{}";

            var sb = new StringBuilder();
            
            for( var enumerator = collection.GetEnumerator(); enumerator.MoveNext(); )
            {
                sb.Append( enumerator.Current.Key.ToString() );
                sb.Append( "," );
                sb.Append( enumerator.Current.Value.ToString() );
                sb.Append( ", " );
                if (separateLines)
                {
                    sb.AppendLine();
                }
            }

            sb.Remove( sb.Length - 2, 2 );

            return sb.ToString();
        }

        

        public static int Median(this IEnumerable<int> source)
        {
            var data = source.OrderBy(n => n).ToArray();
            if (data.Length % 2 == 0)
            {
                return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2;
            }
            return data[data.Length / 2];
        }
        
        public static float Median(this IEnumerable<float> source)
        {
            var data = source.OrderBy(n => n).ToArray();
            if (data.Length % 2 == 0)
            {
                return (data[data.Length / 2 - 1] + data[data.Length / 2]) / 2;
            }
            return data[data.Length / 2];
        }
        
        public static bool DoesTypeImplementInterface(this Type type, Type interfaceType)
        {
            return Array.Exists(type.GetInterfaces(), element => element == interfaceType);
        }
    }

    //  In our case, element managers are usually systems which provide access to subscribed tagged elements
}