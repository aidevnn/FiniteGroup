using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Permutation : GElt, IComparable<Permutation>
    {
        static int HashId(int n) => Helpers.GenHash(n, Enumerable.Range(0, n + 1).ToArray());

        public int CompareTo(Permutation other) => base.CompareTo(other);

        public Permutation(Sn sn) : base(HashId(sn.N))
        {
            FSet = sn;
            table = Enumerable.Range(0, sn.N + 1).ToArray();
            Sgn = 1;
        }

        public Permutation(Sn sn, int[] arr, int hash) : base(hash)
        {
            FSet = sn;
            table = arr.ToArray();
            Sgn = Helpers.ComputeSign(table);
            sn.AddElt(this);
        }

        public Sn Sn => (Sn)FSet;

        public override string OrderStr => Sgn == 1 ? $"{Order}+" : $"{Order}-";
        public override string TableStr => string.Join(" ", table.Skip(1).Select(a => $"{a,2}"));
    }

    public class Sn : FGroup<Permutation>
    {
        public int N { get; private set; }
        private readonly Permutation identity;

        public Sn(int n) : base(n)
        {
            if (n < 1)
                n = 1;

            N = n;
            FmtElt = "({1})[{0}]";
            Fmt = "|G| = {0} in " + $"S{n}";
            cache0 = new int[N + 1];
            cache1 = new int[N + 1];
            cache2 = new int[N + 1];
            identity = new Permutation(this);
            TableOpAdd(identity.HashCode, identity.HashCode, identity.HashCode);
            AddElt(identity);
        }

        public override Permutation Identity => identity;
        protected override Permutation DefineOp(Permutation e0, Permutation e1)
        {
            if (e0.FSet.HashCode != e1.FSet.HashCode)
                return Identity;

            e0.CopyTo(cache0);
            e1.CopyTo(cache1);
            Helpers.ComposePermutation(cache0, cache1, cache2);
            var hash = Helpers.GenHash(N, cache2);
            if (FSetContains(hash))
                return (Permutation)GetElement(hash);

            return new Permutation(this, cache2, hash);
        }

        public Permutation Array(params int[] arr)
        {
            if (arr.Any(c0 => c0 < 0 || c0 > N))
                return Identity;

            if (arr.Distinct().Count() != N)
                return Identity;

            arr.CopyTo(cache0, 1);
            int hash = Helpers.GenHash(N, cache0);
            if (FSetContains(hash))
                return (Permutation)GetElement(hash);

            return new Permutation(this, cache0, hash);
        }

        public Permutation Cycle(params int[] cycle)
        {
            if (cycle.Any(c0 => c0 < 0 || c0 > N))
                return Identity;

            if (cycle.Distinct().Count() != cycle.Count())
                return Identity;

            Identity.CopyTo(cache0);
            var c = cache0[cycle[0]];
            for (int k = 0; k < cycle.Length - 1; ++k)
                cache0[cycle[k]] = cache0[cycle[k + 1]];

            cache0[cycle[cycle.Length - 1]] = c;
            var hash = Helpers.GenHash(N, cache0);
            if (FSetContains(hash))
                return (Permutation)GetElement(hash);

            return new Permutation(this, cache0, hash);
        }

        public Permutation Tau(int a) => Cycle(1, a);
        public Permutation Tau(int a, int b) => Cycle(a, b);
        public Permutation RCycle(int start, int count) => Cycle(Enumerable.Range(start, count).ToArray());
        public Permutation PCycle(int count) => RCycle(1, count);

        public Permutation[] AllTransposition => Enumerable.Range(2, N - 1).Select(a => Tau(1, a)).ToArray();

        public static void DisplaySn(params Permutation[] permutations) => permutations[0].Sn.DisplayGroup(permutations);
        public static void TableSn(params Permutation[] permutations) => permutations[0].Sn.TableGroup(permutations);
        public static void DetailSn(params Permutation[] permutations) => permutations[0].Sn.DetailGroup(permutations);

        public static void Dihedral(int n)
        {
            var sn = new Sn(n);
            var gr = sn.Group(sn.AllTransposition);
            foreach (var e0 in gr.Where(a => a.Order == 2))
            {
                foreach (var e1 in gr.Where(a => a.Order == n))
                {
                    var e2 = sn.Op(e0, e1);
                    var e3 = sn.Op(e2, e2);
                    if (e3.HashCode == sn.Identity.HashCode)
                    {
                        Console.WriteLine("(e0 * e1) * (e0 * e1) = id");
                        e0.Display("e0");
                        e1.Display("e1");
                        Console.WriteLine("e0 * e1");
                        e2.Display("  ");
                        Console.WriteLine();

                        sn.DetailGroup(e0, e1);
                        return;
                    }
                }
            }
        }

    }
}
