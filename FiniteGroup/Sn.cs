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
        }

        public Sn Sn => (Sn)FSet;

        public override string OrderStr => Sgn == 1 ? $"{Order}+" : $"{Order}-";
        public override string TableStr => string.Join(" ", table.Skip(1).Select(a => $"{a,2}"));
    }

    public partial class Sn : FGroup<Permutation>
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
                return GetElement<Permutation>(hash);

            var p = new Permutation(this, cache2, hash);
            AddElt(p);
            return p;
        }

        protected override void Finalized()
        {
            var perms = Helpers.AllPermutation(N, false);
            foreach (var arr in perms) Array(arr);
        }

        public Permutation Array(params int[] arr)
        {
            if (arr.Any(c0 => c0 < 1 || c0 > N))
                return Identity;

            if (arr.Distinct().Count() != N)
                return Identity;

            arr.CopyTo(cache0, 1);
            int hash = Helpers.GenHash(N, cache0);
            if (FSetContains(hash))
                return (Permutation)GetElement(hash);

            var p = new Permutation(this, cache0, hash);
            AddElt(p);
            return p;
        }

        public Permutation Cycle(params int[] cycle)
        {
            if (cycle.Any(c0 => c0 < 0 || c0 > N))
                return Identity;

            if (cycle.Distinct().Count() != cycle.Count())
                return Identity;

            Identity.CopyTo(cache0);
            Helpers.ComposeCycle(cache0, cycle);
            var hash = Helpers.GenHash(N, cache0);
            if (FSetContains(hash))
                return (Permutation)GetElement(hash);

            var p = new Permutation(this, cache0, hash);
            AddElt(p);
            return p;
        }

        public Permutation Tau(int a) => Cycle(1, a);
        public Permutation Tau(int a, int b) => Cycle(a, b);
        public Permutation RCycle(int start, int count) => Cycle(Enumerable.Range(start, count).ToArray());
        public Permutation PCycle(int count) => RCycle(1, count);

        public Permutation[] AllTransposition => Enumerable.Range(2, N - 1).Select(a => Tau(1, a)).ToArray();

    }
}
