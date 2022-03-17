using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class ModuloTuple : IEquatable<ModuloTuple>, IComparable<ModuloTuple>
    {
        static bool IsZero(int[] a) => a.All(e => e == 0);
        static void AddMod(int[] n, int[] m0, int[] m1)
        {
            for(int k = 0; k < m0.Length; ++k)
                m1[k] = (m0[k] + m1[k]) % n[k];
        }

        static void OppMod(int[] n, int[] m0, int[] m1)
        {
            for (int k = 0; k < n.Length; ++k)
            {
                var n0 = n[k];
                m1[k] = (2 * n0 - m0[k]) % n0;
            }
        }

        public static ModuloTuple CreateModuloTuple(int[] n, int[] m0)
        {
            var m1 = new int[n.Length];
            OppMod(n, m0, m1);
            var mt0 = new ModuloTuple(n, m0);
            var mt1 = new ModuloTuple(n, m1);
            mt0.Opp = mt1;
            mt1.Opp = mt0;
            return mt0;
        }

        static int GenHash(int[] n, int[] m)
        {
            var pow = 1;
            var hash = 0;
            for(int k = 0; k < n.Length; ++k)
            {
                hash += pow * m[k];
                pow *= n[k];
            }

            return hash;
        }

        public static ModuloTuple Canonic(int[] N, int rank)
        {
            var m = new int[N.Length];
            m[rank] = 1;
            return CreateModuloTuple(N, m);
        }

        public int[] N { get; private set; }
        public int Sgn => 1;
        public string SgnStr => "";
        public int Order { get; private set; }
        public ModuloTuple Opp { get; private set; }

        int[] M { get; set; }
        readonly int hashcode, nhash;

        private ModuloTuple(int[] n, int[] m)
        {
            N = n.ToArray();
            M = new int[n.Length];
            AddMod(n, m, M);

            Order = 0;
            var m1 = new int[m.Length];
            while (true)
            {
                ++Order;
                AddMod(n, m, m1);
                if (IsZero(m1))
                    break;
            }

            hashcode = GenHash(N, M);
            nhash = GenHash(n, Enumerable.Repeat(1, n.Length).ToArray());
        }


        public ModuloTuple Op(ModuloTuple m1)
        {
            var m0 = M.ToArray();
            AddMod(N, m1.M, m0);
            return CreateModuloTuple(N, m0);
        }

        public override int GetHashCode() => hashcode;
        public override string ToString() => string.Format("({1})<{0}>", Order, string.Join(" ,", M));
        public void Display(string name = "")
        {
            var nm = "m";
            if (!string.IsNullOrEmpty(name))
                nm = name;

            Console.WriteLine("{0} = {1}", nm, this);
        }

        public bool Equals(ModuloTuple other) => nhash == other.nhash && hashcode == other.hashcode;

        public int CompareTo(ModuloTuple other)
        {
            if (Order != other.Order)
                return Order.CompareTo(other.Order);

            if (Sgn != other.Sgn)
                return Sgn.CompareTo(other.Sgn);

            return hashcode.CompareTo(other.hashcode);
        }
    }

    public class ZxZ
    {
        public int[] N { get; set; }
        public ZxZ(params int[] nm)
        {
            N = nm.ToArray();
        }

        public ModuloTuple Elt(params int[] m) => ModuloTuple.CreateModuloTuple(N, m);
        public ModuloTuple Canonic(int rank) => ModuloTuple.Canonic(N, rank);

        static void TableGroup(List<ModuloTuple> set)
        {
            if (set.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).ToList();
            Dictionary<char, ModuloTuple> ce = new Dictionary<char, ModuloTuple>();
            Dictionary<ModuloTuple, char> ec = new Dictionary<ModuloTuple, char>();
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

        public static HashSet<ModuloTuple> Group(params ModuloTuple[] mods)
        {
            var hs = new HashSet<ModuloTuple>(mods);
            HashSet<(int, int)> prevOP = new HashSet<(int, int)>();
            int sz = 0;
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

        public static void TableGroup(params ModuloTuple[] mods)
        {
            var set = Group(mods).ToList();
            set.Sort();
            var gr = string.Join(" x ", mods[0].N.Select(n => $"Z/{n}Z"));
            Console.WriteLine("|G| = {0} in {1}", set.Count, gr);
            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void DisplayGroup(params ModuloTuple[] mods)
        {
            var set = Group(mods).ToList();
            set.Sort();
            var gr = string.Join(" x ", mods[0].N.Select(n => $"Z/{n}Z"));
            Console.WriteLine("|G| = {0} in {1}", set.Count, gr);
            set.ForEach(p => p.Display());
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void DetailGroup(params ModuloTuple[] mods)
        {
            var set = Group(mods).ToList();
            set.Sort();
            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).ToList();

            var gr = string.Join(" x ", mods[0].N.Select(n => $"Z/{n}Z"));
            Console.WriteLine("|G| = {0} in {1}", set.Count, gr);
            for (int k = 0; k < set.Count; ++k)
                set[k].Display(word[k].ToString());

            Console.WriteLine();

            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        static ModuloTuple[] CanonicalGen(ZxZ z) => Enumerable.Range(0, z.N.Length).Select(rk => ModuloTuple.Canonic(z.N, rk)).ToArray();
        public static void DisplayGroup(ZxZ z) => DisplayGroup(CanonicalGen(z));
        public static void TableGroup(ZxZ z) => TableGroup(CanonicalGen(z));
        public static void DetailGroup(ZxZ z) => DetailGroup(CanonicalGen(z));

        public static void DisplayGroup(params int[] n) => DisplayGroup(new ZxZ(n));
        public static void TableGroup(params int[] n) => TableGroup(new ZxZ(n));
        public static void DetailGroup(params int[] n) => DetailGroup(new ZxZ(n));
    }
}
