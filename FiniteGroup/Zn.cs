using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup 
{
    public class Modulo : AElt
    {
        public static Modulo CreateModulo(Zn zn, int m)
        {
            var m0 = new Modulo(zn, m);
            var m1 = new Modulo(zn, 2 *zn.N - m);
            m0.Opp = m1;
            m1.Opp = m0;
            return m0;
        }

        private Modulo(Zn zn, int m) : base(m % zn.N)
        {
            Group = zn;
            Sgn = 1;
            table = new int[] { m % zn.N };

            int sum = 0;
            while (true)
            {
                ++Order;
                sum = (sum + m) % zn.N;
                if (sum == 0)
                    break;
            }
        }

        public override string OrderStr => $"{Order}";

        public override string TableStr => $"{table[0],2}";

        public override AElt Op(AElt elt)
        {
            if (!Group.Equals(elt.Group))
                return CreateModulo((Zn)Group, 0);

            var m0 = table[0];
            var m1 = ((Modulo)elt).table[0];
            return CreateModulo((Zn)Group, m0 + m1);
        }
    }

    public class Zn : AGroup
    {
        public int N { get; private set; }
        public Zn(int n) : base(n)
        {
            if (n < 2)
                n = 2;

            N = n;
            FmtElt = "({1})[{0}]";
            FmtGroup = "|G| = {0} in " + $"Z/{n}Z";
        }

        public Modulo Elt(int m) => Modulo.CreateModulo(this, m);
        public Modulo Zero => Modulo.CreateModulo(this, 0);
        public Modulo One => Modulo.CreateModulo(this, 1);
        public List<Modulo> AllClasses() => Group(One).Cast<Modulo>().ToList();

        public static void DisplayGroup(params Modulo[] modulos) => AGroup.DisplayGroup(modulos);
        public static void TableGroup(params Modulo[] modulos) => AGroup.TableGroup(modulos);
        public static void DetailGroup(params Modulo[] modulos) => AGroup.DetailGroup(modulos);

        public static void DisplayZn(Zn zn) => DisplayGroup(zn.One);
        public static void TableZn(Zn zn) => TableGroup(zn.One);
        public static void DetailZn(Zn zn) => DetailGroup(zn.One);

        public static void DisplayZn(int n) => DisplayZn(new Zn(n));
        public static void TableZn(int n) => TableZn(new Zn(n));
        public static void DetailZn(int n) => DetailZn(new Zn(n));
    }
}
