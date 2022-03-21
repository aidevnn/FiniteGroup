using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public partial class Sn
    {
        public Permutation Cycles(ManyTuples tuples)
        {
            tuples = Prepare(tuples);
            if (tuples.Max > N)
                return Identity;

            Identity.CopyTo(cache0);
            foreach (var c in tuples.Tuples)
                Helpers.ComposeCycle(cache0, c.Table);

            var hash = Helpers.GenHash(N, cache0);
            if (FSetContains(hash))
                return (Permutation)GetElement(hash);

            var p = new Permutation(this, cache0, hash);
            AddElt(p);
            return p;
        }

        public static void Dihedral(int n)
        {
            var sn = new Sn(n);
            sn.Finalized();

            foreach (var e0 in sn.OfOrder(2).Where(Convexity))
            {
                foreach (var e1 in sn.OfOrder(n).Where(Convexity))
                {
                    var e2 = sn.Op(e0, e1);
                    var e3 = sn.Op(e2, e2);
                    if (e3.HashCode == sn.Identity.HashCode)
                    {
                        Console.WriteLine("(s * r) * (s * r) = id");
                        e0.Display("s");
                        e1.Display("r");
                        Console.WriteLine("s * r");
                        e2.Display(" ");
                        Console.WriteLine();

                        sn.DetailSubGroup(e0, e1);
                        return;
                    }
                }
            }
        }

        public static SingleTuple RC(int start, int count) => new SingleTuple(Enumerable.Range(start, count).ToArray());
        public static SingleTuple PC(int count) => RC(1, count);

        public static Permutation PermutationFrom(params SingleTuple[] cycles)
        {
            cycles = Prepare(cycles);
            var n = cycles.Max(c => c.Table.Max());
            var sn = new Sn(n);
            sn.Identity.CopyTo(sn.cache0);
            foreach (var c in cycles)
                Helpers.ComposeCycle(sn.cache0, c.Table);

            var hash = Helpers.GenHash(sn.N, sn.cache0);
            if (sn.FSetContains(hash))
                return sn.GetElement<Permutation>(hash);

            var p = new Permutation(sn, sn.cache0, hash);
            sn.AddElt(p);
            return p;
        }

        static SingleTuple Prepare(SingleTuple tuple)
        {
            if (tuple.Table.Any(a => a < 1) || tuple.Table.Distinct().Count() != tuple.Table.Length)
                return new SingleTuple(1);

            return tuple;
        }

        static SingleTuple[] Prepare(SingleTuple[] cycles)=> cycles.Select(Prepare).ToArray();

        static ManyTuples Prepare(ManyTuples tuples)
        {
            return new ManyTuples(Prepare(tuples.Tuples));
        }

        public static FSubGroup<Permutation> From(params SingleTuple[] cycles)
        {
            cycles = Prepare(cycles);
            var n = cycles.Max(c => c.Table.Max());
            var sn = new Sn(n);
            List<Permutation> permutations = new List<Permutation>();
            foreach (var c in cycles)
                permutations.Add(sn.Cycle(c.Table));

            return sn.SubGroup(permutations.ToArray());
        }

        public static FSubGroup<Permutation> FromMany(params ManyTuples[] tuples)
        {
            tuples = tuples.Select(Prepare).ToArray();
            var n = tuples.Max(t => t.Max);
            var sn = new Sn(n);

            List<Permutation> permutations = new List<Permutation>();
            foreach (var cycles in tuples)
                permutations.Add(sn.Cycles(cycles));

            return sn.SubGroup(permutations.ToArray());
        }

        public static bool Convexity(Permutation p0)
        {
            var cache0 = p0.Sn.cache0;
            p0.CopyTo(cache0);
            return Helpers.Convexity(cache0);
        }

        public static void Display(params Permutation[] permutations) => permutations[0].Sn.DisplaySubGroup(permutations);
        public static void Table(params Permutation[] permutations) => permutations[0].Sn.TableSubGroup(permutations);
        public static void Details(params Permutation[] permutations) => permutations[0].Sn.DetailSubGroup(permutations);
    }
}
