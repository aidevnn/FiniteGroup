using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public abstract class GElt : Elt, IComparable<GElt>
    {
        public List<int> Generated = new List<int>();
        public int Order => Generated.Count;
        public int Sgn { get; set; }

        protected GElt(int[] dims, int[] arr) : base(dims, arr) { }
        protected GElt(int dim, int[] arr) : base(dim, arr) { }
        protected GElt(int hash) : base(hash) { }

        public int CompareTo(GElt other)
        {
            var ord0 = Order;
            var ord1 = other.Order;
            if (ord0 != ord1)
                return ord0.CompareTo(ord1);

            return base.CompareTo(other);
        }
    }

    public abstract class FGroup<T> : Table where T : GElt
    {
        protected FGroup(int[] arr) : base(arr) { }
        protected FGroup(int hash) : base(hash) { }

        protected abstract T DefineOp(T e0, T e1);
        public abstract T Identity { get; }

        public void AddElt(T e)
        {
            SetAdd(e);
            Generate(e);
        }

        public T Invert(T e0) => GetElement<T>(GetInvert(e0.HashCode));

        public T Op(T e0, T e1)
        {
            if (TableOpContains(e0.HashCode, e1.HashCode))
                return GetElement<T>(TableOp(e0.HashCode, e1.HashCode));

            var e2 = DefineOp(e0, e1);
            TableOpAdd(e0.HashCode, e1.HashCode, e2.HashCode);
            AddElt(e2);
            return e2;
        }

        public void Generate(T e0)
        {
            if (e0.Generated.Count == 0)
                e0.Generated.Add(Identity.HashCode);

            var acc = GetElement<T>(e0.Generated.Last());
            var e1 = Op(e0, acc);
            if (e1.Equals(Identity))
                return;

            if (!e0.Generated.Contains(e1.HashCode))
                e0.Generated.Add(e1.HashCode);

            if (!e0.Equals(acc) && !acc.Equals(Identity))
                Generate(acc);

            Generate(e0);
        }

        protected HashSet<T> Group(params T[] elts)
        {
            var hs = new HashSet<T>(new ObjEquality<T>());
            int sz = 0;
            foreach (var e0 in elts)
            {
                hs.Add(e0);
                foreach (var e1 in e0.Generated)
                    hs.Add(GetElement<T>(e1));
            }

            do
            {
                sz = hs.Count;
                var lt = hs.ToHashSet();
                foreach (var e0 in lt)
                    foreach (var e1 in lt)
                    {
                        T e2;
                        if (TableOpContains(e0.HashCode, e1.HashCode))
                            e2 = (T)GetElement(TableOp(e0.HashCode, e1.HashCode));
                        else
                            e2 = Op(e0, e1);

                        hs.Add(e2);
                    }

            } while (hs.Count != sz);

            return hs;
        }

        public void Display()
        {
            var set = Elements.Cast<T>().ToList();
            set.Sort();
            DisplayGroup(this, set);

            foreach (var e0 in set)
            {
                Console.WriteLine("({0}) Invert : ({1})", e0.TableStr, Invert(e0).TableStr);
                foreach (var e1 in set)
                {
                    if (!TableOpContains(e0.HashCode, e1.HashCode))
                        continue;

                    Console.WriteLine("\t{0} {1} -> {2}", e0.HashCode, e1.HashCode, TableOp(e0.HashCode, e1.HashCode));
                }
            }

            Console.WriteLine();
        }

        protected void DisplayGroup(params T[] elts)
        {
            var set = Group(elts).ToList();
            set.Sort();

            DisplayGroup(this, set);
            Console.WriteLine();
        }

        protected void TableGroup(params T[] elts)
        {
            var set = Group(elts).ToList();
            set.Sort();

            TableGroup(this, set);
            Console.WriteLine();
        }

        protected void DetailGroup(params T[] elts)
        {
            var set = Group(elts).ToList();
            set.Sort();

            DisplayGroup(this, set);
            TableGroup(this, set);
            Console.WriteLine();
        }

        static List<string> GenLetters(int n)
        {
            if (n > 50)
                return Enumerable.Range(1, n).Select(a => $"E{a,2:0000}").ToList();

            return "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(n).Select(c => $"{c}").ToList();
        }

        protected static void Display(FGroup<T> gr, params T[] elts)
        {
            if (!elts.All(e => e.FSet.HashCode == gr.HashCode))
                return;

            gr.DisplayGroup(elts);
        }

        protected static void Table(FGroup<T> gr, params T[] elts)
        {
            if (!elts.All(e => e.FSet.HashCode == gr.HashCode))
                return;

            gr.TableGroup(elts);
        }

        protected static void Detail(FGroup<T> gr, params T[] elts)
        {
            if (!elts.All(e => e.FSet.HashCode == gr.HashCode))
                return;

            gr.DetailGroup(elts);
        }

        static void DisplayGroup(FGroup<T> gr, List<T> set)
        {
            Console.WriteLine(gr.Fmt, set.Count);

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

        static void TableGroup(FGroup<T> gr, List<T> set)
        {
            Console.WriteLine(set[0].FSet.Fmt, set.Count);

            if (set.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(set.Count).Select(w => w[0]).ToList();
            Dictionary<char, T> ce = new Dictionary<char, T>();
            Dictionary<T, char> ec = new Dictionary<T, char>(new ObjEquality<T>());

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
                var l0 = set.Select(e1 => ec[gr.Op(e1, e0)]).ToList();
                Console.WriteLine(MyFormat(v0, " ", l0));
            }

            Console.WriteLine();
        }
    }
}
