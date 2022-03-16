﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Perm : IEquatable<Perm>, IComparable<Perm>
    {
        static bool IsIdentity(int[] arr)
        {
            for (int k = 0; k < arr.Length; ++k)
            {
                if (k != arr[k])
                    return false;
            }

            return true;
        }

        static void Compose(int[] arr0, int[] arr1, int[] arr2)
        {
            for (int k = 0; k < arr2.Length; ++k)
                arr2[k] = arr0[arr1[k]];
        }

        static (int, int[]) ComputeOrder(int[] arr)
        {
            int[] arr0 = Enumerable.Range(0, arr.Length).ToArray();
            int[] arr1 = new int[arr.Length];

            int k = 1;
            for(; k < arr.Length; ++k)
            {
                Compose(arr, arr0, arr1);
                if (!IsIdentity(arr1))
                    arr1.CopyTo(arr0, 0);
                else
                    break;
            }

            return (k, arr0);
        }

        static int ComputeSign(int[] arr)
        {
            int Sign = 1;
            for (int i = 1; i < arr.Length - 1; ++i)
                for (int j = i + 1; j < arr.Length; ++j)
                    if (arr[i] > arr[j])
                        Sign *= -1;

            return Sign;
        }

        public static Perm CreatePerm(int[] table0)
        {
            int sgn = ComputeSign(table0);
            var v = ComputeOrder(table0);
            var ord = v.Item1;
            var table1 = v.Item2;
            var p0 = new Perm(table0, sgn, ord);
            var p1 = new Perm(table1, sgn, ord);
            p0.Opp = p1;
            p1.Opp = p0;
            return p0;
        }

        static Perm Op(int[] arr1, int[] arr2)
        {
            var arr = new int[arr1.Length];
            Compose(arr1, arr2, arr);
            return CreatePerm(arr);
        }

        int[] table;
        public int N => table.Length - 1;
        public int Sgn { get; private set; }
        public string SgnStr => Sgn == -1 ? "-" : "+";
        public int Order { get; private set; }
        public Perm Opp { get; private set; }

        public Perm(int n)
        {
            table = Enumerable.Range(0, n + 1).ToArray();
            Order = 1;
            Sgn = 1;
            Opp = this;
        }

        private Perm(int[] t, int sgn, int ord)
        {
            table = t.ToArray();
            Sgn = sgn;
            Order = ord;
        }

        public Perm Op(Perm p) => Op(table, p.table);

        public bool Equals(Perm other) => table.Length == other.table.Length && table.SequenceEqual(other.table);
        public override int GetHashCode() => 1;

        public int CompareTo(Perm other)
        {
            if (other.N != N)
                throw new Exception();

            if (Order != other.Order)
                return Order.CompareTo(other.Order);

            if (Sgn != other.Sgn)
                return Sgn.CompareTo(other.Sgn);

            for (int k = 1; k <= N; ++k)
            {
                var t0 = table[k];
                var t1 = other.table[k];
                if (t0 != t1)
                    return t0.CompareTo(t1);
            }

            return 0;
        }

        public override string ToString() => string.Format("[{2}]<{0}{1}>", Order, SgnStr, string.Join(" ", table.Skip(1).Select(e => $"{e,2}")));

        public void Display(string name = "")
        {
            var nm = "s";
            if (!string.IsNullOrEmpty(name))
                nm = name;

            Console.WriteLine("{0} = {1}", nm, this);
        }
    }

    public class Sn
    {
        public int N { get; private set; }
        readonly HashSet<int> elts;
        public Sn(int n)
        {
            if (n < 1)
                n = 1;

            N = n;
            elts = new HashSet<int>(Enumerable.Range(1, n));
        }

        public Perm Cycle(params int[] cycle)
        {
            if (cycle.Any(c0 => !elts.Contains(c0)))
                return new Perm(N);

            if(cycle.Distinct().Count()!=cycle.Count())
                return new Perm(N);

            var arr = Enumerable.Range(0, N + 1).ToArray();
            var c = arr[cycle[0]];
            for (int k = 0; k < cycle.Length - 1; ++k)
                arr[cycle[k]] = arr[cycle[k + 1]];

            arr[cycle[cycle.Length - 1]] = c;
            return Perm.CreatePerm(arr);
        }

        public Perm Tau(int a) => Cycle(1, a);
        public Perm Tau(int a, int b) => Cycle(a, b);
        public Perm RCycle(int start, int count) => Cycle(Enumerable.Range(start, count).ToArray());
        public Perm PCycle(int count) => RCycle(1, count);

        public static HashSet<Perm> Group(params Perm[] perms)
        {
            if (perms.Select(p => p.N).Distinct().Count() != 1)
                return new HashSet<Perm>() { new Perm(1) };

            var hs = new HashSet<Perm>(perms);
            int sz = 0;
            do
            {
                sz = hs.Count;
                var lt = hs.ToHashSet();
                foreach (var e0 in lt)
                    foreach (var e1 in lt)
                    {
                        var e2 = e0.Op(e1);
                        hs.Add(e2);
                        hs.Add(e2.Opp);
                    }

            } while (hs.Count != sz);

            return hs;
        }

        public static void DisplayGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();
            set.ForEach(p => p.Display());
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void TableGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();
            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void DetailGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();

            Console.WriteLine("|G| = {0} in S{1}", set.Count, perms[0].N);
            if (perms.Length > 7)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).Select(c => $"{c}").ToList();
            if (set.Count > 50)
                word = Enumerable.Range(1, set.Count).Select(a => $"E{a,2:0000}").ToList();
            for (int k = 0; k < set.Count; ++k)
                set[k].Display(word[k].ToString());

            Console.WriteLine();

            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void DetailGroup(Sn sn) => DetailGroup(Enumerable.Range(2, sn.N - 1).Select(a => sn.Tau(a)).ToArray());
        public static void DetailSn(int n) => DetailGroup(new Sn(n));

        static void TableGroup(List<Perm> set)
        {
            if (set.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).ToList();
            Dictionary<char, Perm> ce = new Dictionary<char, Perm>();
            Dictionary<Perm, char> ec = new Dictionary<Perm, char>();
            for (int k = 0; k < set.Count; ++k)
            {
                var c = word[k];
                var e = set.ElementAt(k);
                ce[c] = e;
                ec[e] = c;
            }

            string MyFormat(string c, string g, List<char> l) => string.Format("{0,2}|{1}", c, string.Join(g, l));

            var head = MyFormat("*", " ", word);
            var line = MyFormat("--", "", Enumerable.Repeat('-', word.Count * 2).ToList());
            Console.WriteLine(head);
            Console.WriteLine(line);
            foreach (var e0 in set)
            {
                var v0 = ec[e0].ToString();
                var l0 = set.Select(e1 => ec[e0.Op(e1)]).ToList();
                Console.WriteLine(MyFormat(v0, " ", l0));
            }

            Console.WriteLine();
        }

    }
}