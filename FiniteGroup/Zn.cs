using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Modulo : GElt, IComparable<Modulo>
    {
        public Modulo(Zn zn) : base(0)
        {
            FSet = zn;
            table = new int[zn.Dims.Length];
            Sgn = 1;
        }

        public Modulo(Zn zn, int[] arr, int hash) : base(hash)
        {
            FSet = zn;
            table = arr.ToArray();
            Sgn = 1;
        }

        public Zn Zn => (Zn)FSet;

        public override string OrderStr => $"{Order}";
        public override string TableStr => string.Join(", ", table.Select(a => $"{a,2}"));

        public int CompareTo(Modulo other) => base.CompareTo(other);
    }

    public partial class Zn : FGroup<Modulo>
    {
        public int[] Dims { get; private set; }
        private readonly Modulo identity;
        public Zn(params int[] dims) : base(dims)
        {
            Dims = dims.ToArray();
            FmtElt = "({1})[{0}]";
            var gr = string.Join(" x ", Dims.Select(n => $"Z/{n}Z"));
            Fmt = "|G| = {0} in " + gr;
            cache0 = new int[Dims.Length];
            cache1 = new int[Dims.Length];
            cache2 = new int[Dims.Length];
            identity = new Modulo(this);
            TableOpAdd(identity.HashCode, identity.HashCode, identity.HashCode);
            AddElt(identity);
        }

        public override Modulo Identity => identity;
        protected override void Finalized()
        {
            var tuples = Helpers.AllTuples(Dims);
            foreach (var e in tuples) Elt(e);
        }

        protected override Modulo DefineOp(Modulo e0, Modulo e1)
        {
            if (e0.FSet.HashCode != e1.FSet.HashCode)
                return Identity;

            e0.CopyTo(cache0);
            e1.CopyTo(cache1);
            Helpers.AddMod(Dims, cache0, cache1, cache2);
            var hash = Helpers.GenHash(Dims, cache2);
            if (FSetContains(hash))
                return (Modulo)GetElement(hash);

            var m = new Modulo(this, cache2, hash);
            AddElt(m);
            return m;
        }

        public Modulo Canonical(int rank) => Elt(Helpers.Canonic(Dims.Length, rank));
        public Modulo[] CanonicalBase => Enumerable.Range(0, Dims.Length).Select(Canonical).ToArray();

        public Modulo Elt(params int[] arr)
        {
            ClearCaches();
            arr.CopyTo(cache0, 0);
            Helpers.AddMod(Dims, cache0, cache1, cache2);
            var hash = Helpers.GenHash(Dims, cache2);
            if (FSetContains(hash))
                return (Modulo)GetElement(hash);

            var m = new Modulo(this, cache2, hash);
            AddElt(m);
            return m;
        }

    }
}
