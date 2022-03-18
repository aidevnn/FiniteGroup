using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public abstract class AObj
    {
        protected AObj(int[] arr)
        {
            HashCode = ArrayOps.GenHash(Enumerable.Range(1, arr.Length).ToArray(), arr);
        }

        protected AObj(int[] dims, int[] arr)
        {
            HashCode = ArrayOps.GenHash(dims, arr);
        }

        protected AObj(int dim, int[] arr)
        {
            HashCode = ArrayOps.GenHash(dim, arr);
        }

        protected AObj(int hash)
        {
            HashCode = hash;
        }

        public int HashCode { get; }
        public override int GetHashCode() => HashCode;
    }

    public abstract class AElt : AObj, IEquatable<AElt>, IComparable<AElt>
    {
        public int Order { get; set; }
        public AElt Opp { get; set; }
        public int Sgn { get; set; }
        public AGroup Group { get; set; }
        protected int[] table;

        protected AElt(int[] dims, int[] arr) : base(dims, arr) { }
        protected AElt(int dim, int[] arr) : base(dim, arr) { }
        protected AElt(int hash) : base(hash) { }

        public abstract string OrderStr { get; }
        public abstract string TableStr { get; }
        public abstract AElt Op(AElt elt);

        public bool Equals(AElt other) => Group.Equals(other.Group) && HashCode.Equals(other.HashCode);

        public int CompareTo(AElt other)
        {
            if (Order != other.Order)
                return Order.CompareTo(other.Order);

            if (Sgn != other.Sgn)
                return Sgn.CompareTo(other.Sgn);

            for (int k = 0; k < table.Length; ++k)
            {
                var t0 = table[k];
                var t1 = other.table[k];
                if (t0 != t1)
                    return t0.CompareTo(t1);
            }

            return 0;
        }

        public void Display(string name = "")
        {
            var nm = "s";
            if (!string.IsNullOrEmpty(name))
                nm = name;

            Console.WriteLine("{0} = {1}", nm, this);
        }

        public override string ToString() => string.Format(Group.FmtElt, OrderStr, TableStr);
    }

    public abstract class AGroup : AObj, IEquatable<AGroup>
    {
        protected AGroup(int[] arr) : base(arr) { }
        protected AGroup(int hash) : base(hash) { }

        public string FmtElt { get; set; }
        public string FmtGroup { get; set; }

        public bool Equals(AGroup other) => HashCode.Equals(other.HashCode);

        protected static HashSet<AElt> Group(params AElt[] elts)
        {
            var hs = new HashSet<AElt>(elts);
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

        protected static void DisplayGroup(params AElt[] elts)
        {
            var set = Group(elts).ToList();
            set.Sort();

            DisplayGroup(set);
            Console.WriteLine();
        }

        protected static void TableGroup(params AElt[] elts)
        {
            var set = Group(elts).ToList();
            set.Sort();

            TableGroup(set);
            Console.WriteLine();
        }

        protected static void DetailGroup(params AElt[] elts)
        {
            var set = Group(elts).ToList();
            set.Sort();

            DisplayGroup(set);
            TableGroup(set);
            Console.WriteLine();
        }

        static List<string> GenLetters(int n)
        {
            if (n > 50)
                return Enumerable.Range(1, n).Select(a => $"E{a,2:0000}").ToList();

            return "@abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".Take(n).Select(c => $"{c}").ToList();
        }

        static void DisplayGroup(List<AElt> set)
        {
            Console.WriteLine(set[0].Group.FmtGroup, set.Count);

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

        static void TableGroup(List<AElt> set)
        {
            Console.WriteLine(set[0].Group.FmtGroup, set.Count);

            if (set.Count > 50)
            {
                Console.WriteLine("TOO BIG");
                return;
            }

            var word = GenLetters(set.Count).Select(w => w[0]).ToList();
            Dictionary<char, AElt> ce = new Dictionary<char, AElt>();
            Dictionary<AElt, char> ec = new Dictionary<AElt, char>();
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
