using System;
using Random = UnityEngine.Random;

namespace Presenter.Misc
{
    public static class ArrayExtensions
    {
        public static T GetRandom<T>(this T[] array)
        {
            var count = array.Length;
            if (count == 0) throw new Exception("bro? empty array");

            var randomNum = Random.Range(0, count);

            return array[randomNum];

        }
    }
}