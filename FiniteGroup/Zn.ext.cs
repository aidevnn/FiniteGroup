using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public partial class Zn
    {
        public Modulo Elt(SingleTuple tuple) => Elt((Prepare(this, tuple)).Table);

        public FSubGroup<Modulo> SubGroupGeneratedBy(params SingleTuple[] tuples)
        {
            tuples = Prepare(this, tuples);
            return IdentitySubGroup.GenerateSubGroup(tuples.Select(Elt).ToArray());
        }

        static void CopyFromTo(int[] arrFrom, int[] arrTo)
        {
            for (int k = 0; k < Math.Min(arrFrom.Length, arrTo.Length); ++k)
                arrTo[k] = arrFrom[k];
        }

        static SingleTuple Prepare(Zn zn, SingleTuple tuple)
        {
            zn.ClearCaches(); 
            for (int k = 0; k < Math.Min(tuple.Table.Length, zn.cache0.Length); ++k)
                zn.cache0[k] = tuple.Table[k];

            return new SingleTuple(zn.cache0);
        }

        static SingleTuple[] Prepare(Zn zn, SingleTuple[] tuples)
        {
            tuples = tuples.Select(t => Prepare(zn, t)).ToArray();
            return tuples;
        }

        public static Zn OfDim(params int[] dims) => new Zn(dims);

        public static void Display(params Modulo[] modulos) => modulos[0].Zn.DisplaySubGroup(modulos);
        public static void Table(params Modulo[] modulos) => modulos[0].Zn.TableSubGroup(modulos);
        public static void DetailZn(params Modulo[] modulos) => modulos[0].Zn.DetailSubGroup(modulos);

        public static void Display(Zn zn) => Display(zn.CanonicalBase);
        public static void Table(Zn zn) => Table(zn.CanonicalBase);
        public static void Details(Zn zn) => DetailZn(zn.CanonicalBase);

        public static void Display(params int[] dims) => Display(new Zn(dims));
        public static void Table(params int[] dims) => Table(new Zn(dims));
        public static void Details(params int[] dims) => Details(new Zn(dims));
    }
}
