using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PG
{
    public static class MyRandom
    {
        /// <summary>
        /// 범위안에서 중복되지않는 수 뽑기  min(inclusive) , max(exclusive)
        /// </summary>
        private static IEnumerable<int> Ranges(int min, int max,int count)
        {
            count = max-min < count ? max-min : count;
            var pick = new List<int>();
            for (var i = 0; i < count;)
            {
                var rand = Random.Range(min, max);
                if (!pick.Contains(rand))
                {
                    pick.Add(rand);
                    i++;
                }
            }
            return pick;
        }
        /// <summary>
        /// 0~100 확률 계산
        /// </summary>
        public static bool IsSuccessH(float val)
        {
            return UnityEngine.Random.Range(0, 100) < val;
        }
        /// <summary>
        /// 0~1 확률 계산
        /// </summary>
        public static bool IsSuccess(float val)
        {
            return UnityEngine.Random.Range(0, 1.0f) < val;
        }
        public static List<T> Shuffle<T>(this IReadOnlyList<T> list)
        {
            var newList = new List<T>(list);
            var n = list.Count;
            var i = 0;
            while (i<n)
            {
                var k = Random.Range(i, n);
                (newList[k], newList[i]) = (newList[i], newList[k]);
                i++;
            }
            return newList;
        }
        public static T PickRandom<T>(this IReadOnlyList<T> list)
        {
            if (list.Count == 0)
                throw new Exception();//empty list Exception
            return list[Random.Range(0, list.Count)];
        }
        public static List<T> PickRandoms<T>(this IReadOnlyList<T> list,int count)
        {
            return Ranges(0, list.Count, count).Select(t => list[t]).ToList();
        }
        public static List<T> PickRandomsNoDuplication<T>(this IReadOnlyList<T> list,int count)
        {
            var listClone = list.ToList();

            
            var returnList = new List<T>();
            while (listClone.Count > 0 && count>0)
            {
                var tempt= listClone.PickRandom();
                returnList.Add(tempt);
                listClone.Remove(tempt);
                count--;
            }
            //Debug.Log(count);
            return returnList;
        }

        
        public static T PickRandom<T>(this IEnumerable<T> datas)
        {
            return PickRandom(datas.ToList());
        }
        public static List<T> PickRandoms<T>(this IEnumerable<T> datas,int count)
        {
            return PickRandoms(datas.ToList(),count);
        }
        public static List<T> UniformlySelect<T>(this IEnumerable<T> enumerable,int select)
        {
            var list = enumerable.ToList();
            var newList = new List<T>();
            var length = list.Count;
            var groupSize = Mathf.CeilToInt((float)length / select);
            for (var i = 0; i < length; i += groupSize)
            {
                newList.Add(list[UnityEngine.Random.Range(i,Mathf.Min(i+groupSize,length))]);
            }
            return newList;
        }

        public static T PickRandomWeighted<T>(this IEnumerable<T> enumerable, float[] weights)
        {
            var newList = new List<T>(enumerable);
            if (weights.Length != newList.Count)
            {
                return PickRandom(newList);
            }
            var randomWeight = Random.Range(0, weights.Sum());
            var index = FindWeightIndex(weights, randomWeight);
            return newList[index];
        }
        public static List<T> PickRandomsWeighted<T>(this IEnumerable<T> enumerable, float[] weights,int select)
        {
            var results = new List<T>();
            var newList = new List<T>(enumerable);
            var weightsList = new List<float>(weights);
            if (weights.Length != newList.Count)
            {
                return PickRandoms(newList,select);
            }
            if (select > newList.Count)
            {
                return newList;
            }
            while (results.Count < select)
            {
                var randomWeight = Random.Range(0, weights.Sum());
                var index = FindWeightIndex(weightsList, randomWeight);
                results.Add(newList[index]);
                newList.RemoveAt(index);
                weightsList.RemoveAt(index);
            }
            return results;
        }
        private static int FindWeightIndex(IReadOnlyList<float> weights, float find)
        {
            var sum = .0f;
            for (var i = 0; i < weights.Count; i++)
            {
                sum += weights[i];
                if (sum >= find)
                    return i;
            }
            return weights.Count - 1;
        }

    }
}