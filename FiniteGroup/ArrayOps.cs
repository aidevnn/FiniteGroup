using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public static class ArrayOps
    {
        public static int GenHash(int[] n, int[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < m.Length; ++k)
            {
                hash += pow * m[k];
                pow *= n[k];
            }

            return hash;
        }

        public static int GenHash(int n, int[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 0; k < m.Length; ++k)
            {
                hash += pow * m[k];
                pow *= n;
            }

            return hash;
        }

        public static bool IsZero(int[] a) => a.All(e => e == 0);

        public static bool IsIdentity(int[] arr)
        {
            for (int k = 0; k < arr.Length; ++k)
            {
                if (k != arr[k])
                    return false;
            }

            return true;
        }

        public static void Compose(int[] arr0, int[] arr1, int[] arr2)
        {
            for (int k = 0; k < arr2.Length; ++k)
                arr2[k] = arr0[arr1[k]];
        }

        public static (int, int[]) ComputeOrder(int[] arr)
        {
            int[] arr0 = Enumerable.Range(0, arr.Length).ToArray();
            int[] arr1 = new int[arr.Length];

            int order = 0;
            while (true)
            {
                ++order;
                Compose(arr, arr0, arr1);
                if (!IsIdentity(arr1))
                    arr1.CopyTo(arr0, 0);
                else
                    break;
            }

            return (order, arr0);
        }

        public static int ComputeSign(int[] arr)
        {
            int sgn = 1;
            for (int i = 1; i < arr.Length - 1; ++i)
                for (int j = i + 1; j < arr.Length; ++j)
                    if (arr[i] > arr[j])
                        sgn *= -1;

            return sgn;
        }

        public static void AddMod(int[] n, int[] m0, int[] m1)
        {
            for (int k = 0; k < m0.Length; ++k)
                m1[k] = (m0[k] + m1[k]) % n[k];
        }

        public static void OppMod(int[] n, int[] m0, int[] m1)
        {
            for (int k = 0; k < n.Length; ++k)
            {
                var n0 = n[k];
                m1[k] = (2 * n0 - m0[k]) % n0;
            }
        }

        public static int[][] AllPermutation(int order)
        {
            var pool = Enumerable.Range(1, order).ToList();
            var acc = new List<List<int>>() { new List<int>() };
            var tmpPool = new Queue<int>(pool);
            while (tmpPool.Count != 0)
            {
                var p = tmpPool.Dequeue();
                var tmpAcc = new List<List<int>>();
                foreach (var l0 in acc)
                {
                    for (int k = 0; k <= l0.Count; ++k)
                    {
                        var l1 = l0.ToList();
                        l1.Insert(k, p);
                        tmpAcc.Add(l1);
                    }
                }

                acc = tmpAcc;
            }

            foreach (var e in acc)
                e.Insert(0, 0);

            return acc.Select(a => a.ToArray()).ToArray();
        }

        public static int[] Canonic(int dim, int rank)
        {
            int[] table = new int[dim];
            table[rank] = 1;
            return table;
        }
    }
}
