using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public class Permutation : AElt
    {
        public static Permutation Id(Sn sn)
        {
            int sgn = 1, ord = 1;
            int[] table = Enumerable.Range(0, sn.N + 1).ToArray();
            var p0 = new Permutation(sn, table, sgn, ord);
            p0.Opp = p0;
            return p0;
        }

        public static Permutation Create(Sn sn, int[] table0)
        {
            int sgn = ArrayOps.ComputeSign(table0);
            var (ord, table1) = ArrayOps.ComputeOrder(table0);
            var p0 = new Permutation(sn, table0, sgn, ord);
            var p1 = new Permutation(sn, table1, sgn, ord);
            p0.Opp = p1;
            p1.Opp = p0;
            return p0;
        }

        static Permutation Op(Sn sn, int[] arr1, int[] arr2)
        {
            var arr = new int[arr1.Length];
            ArrayOps.Compose(arr1, arr2, arr);
            return Create(sn, arr);
        }

        private Permutation(Sn sn, int[] arr, int sgn, int ord) : base(sn.N, arr) 
        {
            Group = sn;
            table = arr.ToArray();
            Order = ord;
            Sgn = sgn;
        }

        public override string OrderStr => Sgn == 1 ? $"{Order}+" : $"{Order}-";
        public override string TableStr => string.Join(" ", table.Skip(1).Select(a => $"{a,2}"));

        public override AElt Op(AElt elt)
        {
            if (!Group.Equals(elt.Group))
                return Id((Sn)Group);

            return Op((Sn)Group, table, ((Permutation)elt).table);
        }
    }

    public class Sn : AGroup
    {
        public int N { get; private set; }
        public Sn(int n) : base(n)
        {
            if (n < 1)
                n = 1;

            N = n;
            FmtElt = "({1})[{0}]";
            FmtGroup = "|G| = {0} in " + $"S{n}";
        }

        public Permutation Table(params int[] arr)
        {
            if (arr.Any(c0 => c0 < 0 || c0 > N))
                return Permutation.Id(this);

            if (arr.Distinct().Count() != N)
                return Permutation.Id(this);

            var arr0 = new int[N + 1];
            arr.CopyTo(arr0, 1);
            return Permutation.Create(this, arr0);
        }

        public Permutation Cycle(params int[] cycle)
        {
            if (cycle.Any(c0 => c0 < 0 || c0 > N))
                return Permutation.Id(this);

            if (cycle.Distinct().Count() != cycle.Count())
                return Permutation.Id(this);

            var arr = Enumerable.Range(0, N + 1).ToArray();
            var c = arr[cycle[0]];
            for (int k = 0; k < cycle.Length - 1; ++k)
                arr[cycle[k]] = arr[cycle[k + 1]];

            arr[cycle[cycle.Length - 1]] = c;
            return Permutation.Create(this, arr);
        }

        public Permutation Id => Permutation.Id(this);
        public Permutation Tau(int a) => Cycle(1, a);
        public Permutation Tau(int a, int b) => Cycle(a, b);
        public Permutation RCycle(int start, int count) => Cycle(Enumerable.Range(start, count).ToArray());
        public Permutation PCycle(int count) => RCycle(1, count);

        public List<Permutation> AllTransposition() => Enumerable.Range(1, N - 1).Select(a => Tau(1, a)).ToList();
        public List<Permutation> AllPermutations()
        {
            var arrays = ArrayOps.AllPermutation(N);
            List<Permutation> set = new List<Permutation>();
            foreach (var arr in arrays)
                set.Add(Permutation.Create(this, arr));

            set.Sort();
            return set;
        }

        public static List<Permutation> AllTranspositionSn(int n) => new Sn(n).AllTransposition();
        public static List<Permutation> AllPermSn(int n) => new Sn(n).AllPermutations();

        public static void DisplayGroup(params Permutation[] permutations) => AGroup.DisplayGroup(permutations);
        public static void TableGroup(params Permutation[] permutations) => AGroup.TableGroup(permutations);
        public static void DetailGroup(params Permutation[] permutations) => AGroup.DetailGroup(permutations);

        public static void DisplaySn(Sn sn) => DisplayGroup(sn.AllPermutations().ToArray());
        public static void TableSn(Sn sn) => TableGroup(sn.AllPermutations().ToArray());
        public static void DetailSn(Sn sn) => DetailGroup(sn.AllPermutations().ToArray());

        public static void DisplaySn(int n) => DisplaySn(new Sn(n));
        public static void TableSn(int n) => TableSn(new Sn(n));
        public static void DetailSn(int n) => DetailSn(new Sn(n));

        public static void Dihedral(int n)
        {
            var sn = AllPermSn(n);
            var id = ((Sn)sn[0].Group).Id;
            foreach (var e0 in sn.Where(a => a.Order == 2))
            {
                foreach (var e1 in sn.Where(a => a.Order == n))
                {
                    var e2 = e1.Op(e0);
                    var id0 = e2.Op(e2);
                    if (id0.Equals(id))
                    {
                        Console.WriteLine("(e0 * e1) * (e0 * e1) = id");
                        e0.Display("e0 ");
                        e1.Display("e1 ");
                        Console.WriteLine("(e0 * e1)");
                        e2.Display("   ");
                        Console.WriteLine();
                        DetailGroup(e0, e1);
                        return;
                    }
                }
            }
        }
    }
}
