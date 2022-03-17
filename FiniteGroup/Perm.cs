using System;
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

            int order = 0;
            while(true)
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

        static int ComputeSign(int[] arr)
        {
            int sgn = 1;
            for (int i = 1; i < arr.Length - 1; ++i)
                for (int j = i + 1; j < arr.Length; ++j)
                    if (arr[i] > arr[j])
                        sgn *= -1;

            return sgn;
        }

        public static Perm CreatePerm(int[] table0)
        {
            int sgn = ComputeSign(table0);
            var (ord, table1) = ComputeOrder(table0);
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

        static int GenHash(int n, int[] m)
        {
            var pow = 1;
            var hash = 0;
            for (int k = 1; k < m.Length; ++k)
            {
                hash += pow * m[k];
                pow *= n;
            }

            return hash;
        }


        int[] table;
        public int N => table.Length - 1;
        public int Sgn { get; private set; }
        public string SgnStr => Sgn == -1 ? "-" : "+";
        public int Order { get; private set; }
        public Perm Opp { get; private set; }
        readonly int hashcode;

        public Perm(int n)
        {
            table = Enumerable.Range(0, n + 1).ToArray();
            Order = 1;
            Sgn = 1;
            Opp = this;
            hashcode = GenHash(N, table);
        }

        private Perm(int[] t, int sgn, int ord)
        {
            table = t.ToArray();
            Sgn = sgn;
            Order = ord;
            hashcode = GenHash(N, table);
        }

        public Perm Op(Perm p) => Op(p.table, table);

        public bool Equals(Perm other) => table.Length == other.table.Length && hashcode == other.hashcode;
        public override int GetHashCode() => hashcode;

        public int CompareTo(Perm other)
        {
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

        public static void Diedral(int n)
        {
            var sn = GroupSn(n);
            foreach (var e0 in sn.Where(a => a.Order == 2))
            {
                foreach (var e1 in sn.Where(a => a.Order == n))
                {
                    var e2 = e0.Op(e1.Op(e0.Opp));
                    if (e2.Equals(e1.Opp))
                    {
                        e0.Display("e0 ");
                        e0.Opp.Display("e0'");
                        e1.Display("e1 ");
                        e1.Opp.Display("e1'");
                        Console.WriteLine("e0 * e1 * e0' = e1'");
                        e2.Display("r  ");
                        Console.WriteLine();
                        DetailGroup(e0, e1);
                        return;
                    }
                }
            }
        }

        public static HashSet<Perm> Group(params Perm[] perms)
        {
            if (perms.Select(p => p.N).Distinct().Count() != 1)
                return new HashSet<Perm>() { new Perm(1) };

            var hs = new HashSet<Perm>(perms);
            int sz = 0;
            HashSet<(int, int)> prevOP = new HashSet<(int, int)>();
            do
            {
                sz = hs.Count;
                var lt = hs.ToHashSet();
                foreach (var e0 in lt)
                    foreach (var e1 in lt)
                    {
                        var tp = (e0.GetHashCode(), e1.GetHashCode());
                        if (prevOP.Contains(tp))
                            continue;
                        prevOP.Add(tp);

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

            DisplayGroup(set);
            Console.WriteLine();
        }

        public static void TableGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();

            TableGroup(set);
            Console.WriteLine();
        }

        public static void DetailGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();

            DisplayGroup(set);
            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static Perm[] GroupSn(Sn sn) => Group(Enumerable.Range(2, sn.N - 1).Select(a => sn.Tau(a)).ToArray()).ToArray();
        public static Perm[] GroupSn(int n) => GroupSn(new Sn(n));
        public static Perm[] GroupAn(int n) => GroupSn(n).Where(e => e.Sgn == 1).ToArray();

        public static void DisplayGroup(Sn sn) => DisplayGroup(GroupSn(sn));
        public static void TableGroup(Sn sn) => TableGroup(GroupSn(sn));
        public static void DetailGroup(Sn sn) => DetailGroup(GroupSn(sn));
        public static void DetailSn(int n) => DetailGroup(new Sn(n));
        public static void TableSn(int n) => TableGroup(new Sn(n));
        public static void DisplaySn(int n) => DisplayGroup(new Sn(n));

        public static void DisplayAn(int n) => DisplayGroup(GroupAn(n));
        public static void TableAn(int n) => TableGroup(GroupAn(n));
        public static void DetailAn(int n) => DetailGroup(GroupAn(n));

        static List<string> GenLetters(int n)
        {
            if (n > 50)
                return Enumerable.Range(1, n).Select(a => $"E{a,2:0000}").ToList();

            return "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(n).Select(c => $"{c}").ToList();
        }

        static void DisplayGroup(List<Perm> set)
        {
            Console.WriteLine("|G| = {0} in S{1}", set.Count, set[0].N);

            if (set.Count > 1000)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(set.Count);
            for (int k = 0; k < set.Count; ++k)
                set[k].Display(word[k].ToString());

            Console.WriteLine();
        }

        static void TableGroup(List<Perm> set)
        {
            Console.WriteLine("|G| = {0} in S{1}", set.Count, set[0].N);

            if (set.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(set.Count).Select(w => w[0]).ToList();
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
                var l0 = set.Select(e1 => ec[e1.Op(e0)]).ToList();
                Console.WriteLine(MyFormat(v0, " ", l0));
            }

            Console.WriteLine();
        }

    }
}