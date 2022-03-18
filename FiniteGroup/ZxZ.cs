using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class ModuloTuple : AElt
    {
        public static ModuloTuple CreateModuloTuple(ZxZ z, int[] m0)
        {
            var m1 = new int[z.Dims.Length];
            ArrayOps.OppMod(z.Dims, m0, m1);
            var mt0 = new ModuloTuple(z, m0);
            var mt1 = new ModuloTuple(z, m1);
            mt0.Opp = mt1;
            mt1.Opp = mt0;
            return mt0;
        }

        public static ModuloTuple Canonic(ZxZ z, int rank) => CreateModuloTuple(z, ArrayOps.Canonic(z.Dims.Length, rank));

        private ModuloTuple(ZxZ z, int[] m) : base(z.Dims, m)
        {
            Group = z;
            table = new int[z.Dims.Length];
            ArrayOps.AddMod(z.Dims, m, table);

            Order = 0;
            var m1 = new int[m.Length];
            while (true)
            {
                ++Order;
                ArrayOps.AddMod(z.Dims, table, m1);
                if (ArrayOps.IsZero(m1))
                    break;
            }

            Sgn = 1;
        }

        public override string OrderStr => $"{Order}";

        public override string TableStr => string.Join(", ", table);

        public override AElt Op(AElt elt)
        {
            var z = (ZxZ)Group;
            if (!Group.Equals(elt.Group))
                return CreateModuloTuple(z, new int[table.Length]);

            var m0 = table.ToArray();
            ArrayOps.AddMod(z.Dims, ((ModuloTuple) elt).table, m0);
            return CreateModuloTuple(z, m0);
        }
    }

    public class ZxZ : AGroup
    {
        public int[] Dims { get; set; }
        public ZxZ(params int[] dims) : base(dims)
        {
            Dims = dims.ToArray();

            var gr = string.Join(" x ", Dims.Select(n => $"Z/{n}Z"));
            FmtGroup = "|G| = {0} in " + gr;
            FmtElt = "({1})[{0}]";
        }

        public ModuloTuple Elt(params int[] m) => ModuloTuple.CreateModuloTuple(this, m);
        public ModuloTuple Canonic(int rank) => ModuloTuple.Canonic(this, rank);

        public static void DisplayGroup(params ModuloTuple[] modulos) => AGroup.DisplayGroup(modulos);
        public static void TableGroup(params ModuloTuple[] modulos) => AGroup.TableGroup(modulos);
        public static void DetailGroup(params ModuloTuple[] modulos) => AGroup.DetailGroup(modulos);

        static ModuloTuple[] CanonicalGen(ZxZ z) => Enumerable.Range(0, z.Dims.Length).Select(rk => ModuloTuple.Canonic(z, rk)).ToArray();
        public static void DisplayGroup(ZxZ z) => DisplayGroup(CanonicalGen(z));
        public static void TableGroup(ZxZ z) => TableGroup(CanonicalGen(z));
        public static void DetailGroup(ZxZ z) => DetailGroup(CanonicalGen(z));

        public static void DisplayZxZ(params int[] n) => DisplayGroup(new ZxZ(n));
        public static void TableZxZ(params int[] n) => TableGroup(new ZxZ(n));
        public static void DetailZxZ(params int[] n) => DetailGroup(new ZxZ(n));
    }
}
