using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Presenter.Misc
{
    public static class IEnumerableExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            var count = enumerable.Count();
            if (count == 0) throw new Exception("bro? empty enumerable");

            var randomNum = Random.Range(0, count);

            var i = 0;
            foreach (var x1 in enumerable)
            {
                if (i++ == randomNum) return x1;
            }

            throw new Exception("Something wrong with GetRandom");
        }
    }
}