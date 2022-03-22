using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Permutation : GElt, IComparable<Permutation>
    {
        public Permutation(Sn sn) : base(Helpers.SnIdHash(sn.N))
        {
            table = Helpers.SnIdentity(sn.N);
            FSet = Sn = sn;
            Sgn = 1;
            AddHashGenerated(HashCode);
        }

        public Permutation(Sn sn, int[] arr, int hash) : base(hash)
        {
            table = arr.ToArray();
            FSet = Sn = sn;
            Sgn = Helpers.ComputeSign(arr);
            AddHashGenerated(sn.Identity.HashCode);
            AddHashGenerated(HashCode);
        }

        public Sn Sn { get; }

        public int Sgn { get; }
        string SgnStr => Sgn == 1 ? "+" : "-";
        string OrderStr => $"{Order,2}{SgnStr}";
        string TableStr => string.Join(" ", table.Skip(1).Select(e => $"{e,2}"));
        public override string[] DisplayInfos => new string[] { TableStr, OrderStr };

        public int CompareTo(Permutation other)
        {
            var compOrd = Order.CompareTo(other.Order);
            if (compOrd != 0)
                return compOrd;

            var compSgn = Sgn.CompareTo(other.Sgn);
            if (compSgn != 0)
                return compSgn;

            return base.CompareTo(other);
        }
    }

    public class Sn : FGroup<Permutation>
    {
        public int N { get; }
        public Sn(int n) : base(n)
        {
            N = Math.Max(2, n);

            var gr = $"S{N}";
            Fmt = "|G| = {0} in " + gr;
            FmtElt = "({0})[{1}]";

            CreateCaches(N + 1);
            CreateIdentity(new Permutation(this));
        }

        private Permutation TableCache2()
        {
            int hash = Helpers.GenHash(N, cache2);
            if (FSetContains(hash))
                return GetElement<Permutation>(hash);

            var p = new Permutation(this, cache2, hash);
            FGroupAdd(p);
            return p;
        }

        protected override Permutation DefineOp(Permutation a, Permutation b)
        {
            ClearCaches();
            a.CopyTo(cache0);
            b.CopyTo(cache1);
            Helpers.ComposePermutation(cache0, cache1, cache2);

            return TableCache2();
        }

        public Permutation Cycle(params int[] cycle)
        {
            if (!Helpers.CheckArray(N, cycle))
                return Identity;

            ClearCaches();
            Helpers.SnIdentity(N).ReCopyTo(cache2);
            Helpers.ComposeCycle(cache2, cycle);
            return TableCache2();
        }

        public Permutation Cycle(SingleTuple tuple) => Cycle(tuple.Table);

        public Permutation Cycles(ManyTuples tuples)
        {
            if (tuples.Tuples.Any(c => !Helpers.CheckArray(N, c.Table)))
                return Identity;

            ClearCaches();
            Helpers.SnIdentity(N).ReCopyTo(cache2);
            foreach (var c in tuples.Tuples)
                Helpers.ComposeCycle(cache2, c.Table);

            return TableCache2();
        }

        SubFGroup<Permutation> MonoGenic(SingleTuple tuple) => MonoGenic(Cycle(tuple));
        SubFGroup<Permutation> LeftCompose(params SingleTuple[] tuples) => LeftCompose(tuples.Select(Cycle).ToArray());
        SubFGroup<Permutation> LeftCompose(params ManyTuples[] tuples) => LeftCompose(tuples.Select(Cycles).ToArray());
        SubFGroup<Permutation> LeftCompose(SingleTuple tuple, SubFGroup<Permutation> subFGroup, bool amplify = false) => new LeftOp<Permutation>(this, Cycle(tuple), subFGroup, amplify);

        public static SingleTuple RC(int start, int count) => new SingleTuple(Enumerable.Range(start, count).ToArray());
        public static SingleTuple PC(int n) => RC(1, n);
        public static Sn Dim(int n) => new Sn(n);

        public static void Dihedral(int n)
        {
            var sn = new Sn(n);
            List<Permutation> permutations = new List<Permutation>();
            var perms = Helpers.AllPermutation(n);
            foreach (var p in perms)
            {
                if (!Helpers.Convexity(p))
                    continue;

                p.ReCopyTo(sn.cache2);
                var pm = sn.TableCache2();
                permutations.Add(pm);
            }

            permutations.Sort((a, b) => ((Elt)a).CompareTo(b));

            foreach (var s in permutations.Where(a => a.Order == 2))
            {
                foreach (var r in permutations.Where(a => a.Order == n))
                {
                    var e0 = sn.Op(s, r);
                    var e1 = sn.Op(e0, e0);
                    if (e1.HashCode == sn.Identity.HashCode)
                    {
                        Console.WriteLine("(s * r) * (s * r) = id");
                        s.Display("s");
                        r.Display("r");
                        Console.WriteLine("s * r");
                        e0.Display(" ");
                        Console.WriteLine();
                        sn.LeftCompose(s, r).Details();
                        return;
                    }
                }
            }
        }

        public static SubFGroup<Permutation> From(params SingleTuple[] tuples)
        {
            var n = tuples.SelectMany(t => t.Table).Max();
            var sn = new Sn(n);
            return sn.LeftCompose(tuples);
        }

        public static SubFGroup<Permutation> From(params ManyTuples[] tuples)
        {
            var n = tuples.SelectMany(t => t.Tuples).SelectMany(u => u.Table).Max();
            var sn = new Sn(n);
            return sn.LeftCompose(tuples);
        }
    }
}
