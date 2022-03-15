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

        int[] table;
        public int Order => table.Length - 1;
        public int Pow { get; private set; }
        public string Name { get; private set; }

        public Perm(int order)
        {
            table = Enumerable.Range(0, order + 1).ToArray();
            Pow = 1;
            Name = "-";
        }

        public Perm(int[] t)
        {
            table = t.ToArray();
            Cpow();
        }

        public int this[int k]
        {
            get => table[k];
        }

        private void Cpow()
        {
            int[] cache1 = Enumerable.Range(0, table.Length).ToArray();
            int[] cache0 = new int[table.Length];
            int pow = 0;
            for (int k = 0; k < table.Length; ++k)
            {
                ++pow;
                Compose(table, cache1, cache0);
                cache0.CopyTo(cache1, 0);
                if (IsIdentity(cache1))
                    break;
            }

            Name = $"{pow}";
            Pow = pow;
        }

        public Perm Op(Perm p)
        {
            var r = new Perm(Order);
            Compose(table, p.table, r.table);
            r.Cpow();
            return r;
        }

        public bool Equals(Perm other) => table.Length == other.table.Length && table.SequenceEqual(other.table);
        public override int GetHashCode() => 1;

        public int CompareTo(Perm other)
        {
            if (other.Order != Order)
                throw new Exception();

            if (Pow != other.Pow)
                return Pow.CompareTo(other.Pow);

            for (int k = 1; k <= Order; ++k)
            {
                var t0 = this[k];
                var t1 = other[k];
                if (t0 != t1)
                    return t0.CompareTo(t1);
            }

            return 0;
        }

        public override string ToString() => string.Format("[{1}]({0})", Name, string.Join(" ", table.Skip(1).Select(e => $"{e,2}")));

        public void Display(string name = "")
        {
            var id = new Perm(table.Length - 1);
            if (!string.IsNullOrEmpty(name))
                id.Name = name;

            Console.WriteLine(id);
            Console.WriteLine(this);
            Console.WriteLine();
        }
    }

    public class Sigma
    {
        public int Order { get; private set; }
        public Sigma(int order)
        {
            Order = order;
        }

        public Perm Cycle(params int[] cycle)
        {
            var arr = Enumerable.Range(0, Order + 1).ToArray();
            var c = arr[cycle[0]];
            for (int k = 0; k < cycle.Length - 1; ++k)
                arr[cycle[k]] = arr[cycle[k + 1]];

            arr[cycle[cycle.Length - 1]] = c;

            return new Perm(arr);
        }

        public Perm Tau(int a) => Cycle(1, a);
        public Perm Tau(int a, int b) => Cycle(a, b);
        public Perm RCycle(int start, int count) => Cycle(Enumerable.Range(start, count).ToArray());
        public Perm PCycle(int count) => RCycle(1, count);

        public HashSet<Perm> Group(params Perm[] perms)
        {
            var hs = new HashSet<Perm>(perms);
            int sz = 0;
            do
            {
                sz = hs.Count;
                var lt = hs.ToHashSet();
                foreach (var e0 in lt)
                    foreach (var e1 in lt)
                        hs.Add(e0.Op(e1));


            } while (hs.Count != sz);

            return hs;
        }

        public void DisplayGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();
            set.ForEach(p => p.Display());
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public void TableGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();
            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

        public void DetailGroup(params Perm[] perms)
        {
            var set = Group(perms).ToList();
            set.Sort();
            var word = "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(set.Count).ToList();

            for (int k = 0; k < set.Count; ++k)
                set[k].Display(word[k].ToString());

            TableGroup(set);
            Console.WriteLine("#########");
            Console.WriteLine();
        }

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