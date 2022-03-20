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
            zn.AddElt(this);
        }

        public Zn Zn => (Zn)FSet;

        public override string OrderStr => $"{Order}";
        public override string TableStr => string.Join(", ", table.Select(a => $"{a,2}"));

        public int CompareTo(Modulo other) => base.CompareTo(other);
    }

    public class Zn : FGroup<Modulo>
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

            return new Modulo(this, cache2, hash);
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

            return new Modulo(this, cache2, hash);
        }

        public static void DisplayZn(params Modulo[] modulos) => modulos[0].Zn.DisplayGroup(modulos);
        public static void TableZn(params Modulo[] modulos) => modulos[0].Zn.TableGroup(modulos);
        public static void DetailZn(params Modulo[] modulos) => modulos[0].Zn.DetailGroup(modulos);

        public static void DisplayZn(Zn zn) => DisplayZn(zn.CanonicalBase);
        public static void TableZn(Zn zn) => TableZn(zn.CanonicalBase);
        public static void DetailZn(Zn zn) => DetailZn(zn.CanonicalBase);

        public static void DisplayZn(params int[] dims) => DisplayZn(new Zn(dims));
        public static void TableZn(params int[] dims) => TableZn(new Zn(dims));
        public static void DetailZn(params int[] dims) => DetailZn(new Zn(dims));
    }
}
