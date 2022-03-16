using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Modulo : IEquatable<Modulo>, IComparable<Modulo>
    {
        public static Modulo CreateModulo(int n, int m)
        {
            var m0 = new Modulo(n, m);
            var m1 = new Modulo(n, 2 * n + (n - m));
            m0.Opp = m1;
            m1.Opp = m0;
            return m0;
        }

        public int N { get; private set; }
        public int Sgn => 0;
        public string SgnStr => "";
        public int Order { get; private set; }
        public Modulo Opp { get; private set; }

        int M { get; set; }
        private Modulo(int n, int m)
        {
            N = n;
            M = m % N;

            int sum = 0;
            while (true)
            {
                ++Order;
                sum = (sum + M) % N;
                if (sum == 0)
                    break;
            }
        }

        public Modulo Op(Modulo m) => CreateModulo(N, M + m.M);
        public override int GetHashCode() => M;
        public override string ToString() => string.Format("({0})<{1}{2}>", M, Order, SgnStr);
        public void Display(string name = "")
        {
            var nm = "m";
            if (!string.IsNullOrEmpty(name))
                nm = name;

            Console.WriteLine("{0} = {1}", nm, this);
        }

        public bool Equals(Modulo other) => M == other.M;

        public int CompareTo(Modulo other)
        {
            if (Order != other.Order)
                return Order.CompareTo(other.Order);

            if (Sgn != other.Sgn)
                return Sgn.CompareTo(other.Sgn);

            return M.CompareTo(other.M);
        }
    }

    public class Zn
    {
        public int N { get; private set; }
        public Zn(int n)
        {
            if (n < 2)
                n = 2;

            N = n;
        }

        public Modulo Elt(int m) => Modulo.CreateModulo(N, m);

        public static HashSet<Modulo> Group(params Modulo[] mods)
        {
            if (mods.Select(m => m.N).Distinct().Count() != 1)
                return new HashSet<Modulo>() { Modulo.CreateModulo(1, 0) };

            var hs = new HashSet<Modulo>(mods);
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

        public static void DisplayGroup(params Modulo[] mods)
        {
            var set = Group(mods).ToList();
            set.Sort();
            set.ForEach(p => p.Display());
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void TableGroup(params Modulo[] mods)
        {
            var set = Group(mods).ToList();
            set.Sort();
            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public static void DetailGroup(Zn zn) => DetailGroup(zn.Elt(1));

        public static void DetailGroup(params Modulo[] mods)
        {
            var set = Group(mods).ToList();
            set.Sort();
            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).ToList();

            Console.WriteLine("|G| = {0} in Z/{1}Z", set.Count, mods[0].N);
            for (int k = 0; k < set.Count; ++k)
                set[k].Display(word[k].ToString());

            Console.WriteLine();

            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        static void TableGroup(List<Modulo> set)
        {
            if (set.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).ToList();
            Dictionary<char, Modulo> ce = new Dictionary<char, Modulo>();
            Dictionary<Modulo, char> ec = new Dictionary<Modulo, char>();
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
