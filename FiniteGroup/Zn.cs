using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Modulo : GElt, IComparable<Modulo>
    {
        public Modulo(Zn zn) : base(0)
        {
            table = new int[zn.Dims.Length];
            FSet = zn;
            AddHashGenerated(0);
        }

        public Modulo(Zn zn, int[] arr, int hash) : base(hash)
        {
            FSet = zn;
            table = arr.ToArray();
            AddHashGenerated(zn.Identity.HashCode);
            AddHashGenerated(HashCode);
        }

        string OrderStr => $"{Order,2}";
        string TableStr => string.Join(", ", table.Select(e => $"{e,2}"));
        public override string[] DisplayInfos => new string[] { TableStr, OrderStr };

        public int CompareTo(Modulo other)
        {
            var compOrd = Order.CompareTo(other.Order);
            if (compOrd != 0)
                return compOrd;

            return base.CompareTo(other);
        }

    }

    public class Zn : FGroup<Modulo>
    {
        public int[] Dims { get; }
        public Zn(params int[] dims) : base(dims)
        {
            Dims = dims.ToArray();

            var gr = string.Join(" x ", dims.Select(n => $"Z/{n}Z"));
            Fmt = "|G| = {0} in " + gr;
            FmtElt = "({0})[{1}]";

            CreateCaches(Dims.Length);
            CreateIdentity(new Modulo(this));
        }

        protected override Modulo DefineOp(Modulo a, Modulo b)
        {
            ClearCaches();
            a.CopyTo(cache0);
            b.CopyTo(cache1);
            Helpers.AddMod(Dims, cache0, cache1, cache2);
            int hash = Helpers.GenHash(Dims, cache2);
            if (FSetContains(hash))
                return GetElement<Modulo>(hash);

            return new Modulo(this, cache2, hash);
        }

        public Modulo Elt(params int[] e)
        {
            ClearCaches();
            e.ReCopyTo(cache0);
            Helpers.AddMod(Dims, cache0, cache1, cache2);
            int hash = Helpers.GenHash(Dims, cache2);
            if (FSetContains(hash))
                return GetElement<Modulo>(hash);

            var m = new Modulo(this, cache2, hash);
            FGroupAdd(m);
            return m;
        }

        public Modulo Canonic(int rank) => Elt(Helpers.Canonic(Dims.Length, rank));
        public Modulo[] CanonicBase() => Enumerable.Range(0, Dims.Length).Select(Canonic).ToArray();

        public Modulo Elt(SingleTuple tuple) => Elt(tuple.Table);

        public SubFGroup<Modulo> MonoGenic(SingleTuple tuple) => MonoGenic(Elt(tuple));
        public SubFGroup<Modulo> From(params SingleTuple[] tuples) => LeftCompose(tuples.Select(Elt).ToArray());
        public SubFGroup<Modulo> LeftCompose(SingleTuple tuple, SubFGroup<Modulo> subFGroup, bool amplify = false) => new LeftOp<Modulo>(this, Elt(tuple), subFGroup, amplify);

        public static Zn Dim(params int[] dims) => new Zn(dims);
        public void Display() => LeftCompose(CanonicBase()).DisplayElements();
        public void Details() => LeftCompose(CanonicBase()).Details();
        public static void Details(params int[] dims) => Dim(dims).Details();
        public static void Display(params int[] dims) => Dim(dims).Display();
    }
}
