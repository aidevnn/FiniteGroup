using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public abstract class GElt : Elt, IComparable<GElt>
    {
        public List<int> Generated = new List<int>();
        public int Order => Generated.Count;
        public int Sgn { get; set; }

        protected GElt(int[] dims, int[] arr) : base(dims, arr) { }
        protected GElt(int dim, int[] arr) : base(dim, arr) { }
        protected GElt(int hash) : base(hash) { }

        public int CompareTo(GElt other)
        {
            var ord0 = Order;
            var ord1 = other.Order;
            if (ord0 != ord1)
                return ord0.CompareTo(ord1);

            return base.CompareTo(other);
        }
    }

    public abstract class FGroup<T> : Table where T : GElt
    {
        protected FGroup(int[] arr) : base(arr) { }
        protected FGroup(int hash) : base(hash) { }

        protected abstract T DefineOp(T e0, T e1);
        public abstract T Identity { get; }
        private bool initialized = false, finalized = false;
        private FSubGroup<T> self, identitySubGroup;
        protected abstract void Finalized();
        public FSubGroup<T> IdentitySubGroup
        {
            get
            {
                if (initialized)
                    return identitySubGroup;

                identitySubGroup = new FSubGroup<T>(this);
                initialized = true;
                return identitySubGroup;
            }
        }

        public FSubGroup<T> SelfSubGroup
        {
            get
            {
                if (finalized)
                    return self;

                Finalized();
                finalized = true;
                self = SubGroup(Elements<T>().ToArray());
                return self;
            }
        }

        protected void AddElt(T e)
        {
            SetAdd(e);
            Generate(e);
        }

        public T Invert(T e0) => GetElement<T>(GetInvert(e0.HashCode));

        public T Op(T e0, T e1)
        {
            if (TableOpContains(e0.HashCode, e1.HashCode))
                return GetElement<T>(TableOp(e0.HashCode, e1.HashCode));

            var e2 = DefineOp(e0, e1);
            TableOpAdd(e0.HashCode, e1.HashCode, e2.HashCode);
            AddElt(e2);
            return e2;
        }

        protected void Generate(T e0)
        {
            if (e0.Generated.Count == 0)
                e0.Generated.Add(Identity.HashCode);

            var acc = GetElement<T>(e0.Generated.Last());
            var e1 = Op(e0, acc);
            if (e1.Equals(Identity))
                return;

            if (!e0.Generated.Contains(e1.HashCode))
                e0.Generated.Add(e1.HashCode);

            if (!e0.Equals(acc) && !acc.Equals(Identity))
                Generate(acc);

            Generate(e0);
        }

        public List<T> OfOrder(int ord)
        {
            var set = Elements<T>().Where(a => a.Order == ord).ToList();
            set.Sort();
            return set;
        }

        public FSubGroup<T> SubGroup(params T[] elts) => IdentitySubGroup.GenerateSubGroup(elts);

        public void DisplayHashTable()
        {
            var set = Elements<T>().ToList();
            set.Sort();
            foreach (var e0 in set)
            {
                Console.WriteLine("({0}) Invert : ({1})", e0.TableStr, Invert(e0).TableStr);
                foreach (var e1 in set)
                {
                    if (!TableOpContains(e0.HashCode, e1.HashCode))
                        continue;

                    Console.WriteLine("\t{0} {1} -> {2}", e0.HashCode, e1.HashCode, TableOp(e0.HashCode, e1.HashCode));
                }
            }

            Console.WriteLine();
        }

        protected void DisplaySubGroup(params T[] elts) => SubGroup(elts).DisplayElements();

        protected void TableSubGroup(params T[] elts) => SubGroup(elts).Table();

        protected void DetailSubGroup(params T[] elts) => SubGroup(elts).Details();

    }
}
