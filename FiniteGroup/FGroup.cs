using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public abstract class GElt : Elt, IEquatable<Elt>, IComparable<GElt>
    {
        protected GElt(int[] dims, int[] arr) : base(dims, arr) { }
        protected GElt(int dim, int[] arr) : base(dim, arr) { }
        protected GElt(int hash) : base(hash) { }

        readonly List<int> generated = new List<int>();
        public void AddHashGenerated(int hash) => generated.Add(hash);
        public bool ContainsHash(int hash) => generated.Contains(hash);

        public List<int> Generated => generated.ToList();
        public int LastHash => generated.Last();

        public int Order => generated.Count;

        public override int GetHashCode() => base.GetHashCode();
        public bool Equals(Elt other) => HashCode == other.HashCode;

        public int CompareTo(GElt other)
        {
            if (Order != other.Order)
                return Order.CompareTo(other.Order);

            return base.CompareTo(other); ;
        }

        public void ReDisplay()
        {
            Display();
            Console.WriteLine();
            foreach (var e in Generated)
                FSet.GetElement<GElt>(e).ReReDisplay();
        }

        public void ReReDisplay()
        {
            Display("\t");
            foreach (var e in Generated)
                FSet.GetElement<GElt>(e).Display("\t\t");
        }
    }

    public abstract partial class FGroup<T> : Table where T : GElt
    {
        protected FGroup(int[] arr) : base(arr) { }

        protected FGroup(int hash) : base(hash) { }

        protected void CreateCaches(int size)
        {
            cache0 = new int[size];
            cache1 = new int[size];
            cache2 = new int[size];
        }

        protected void CreateIdentity(T identity)
        {
            Identity = identity;
            FSetAdd(identity);
            TableOpAdd(Identity.HashCode, Identity.HashCode, Identity.HashCode);
            MakeCompleted(identity.HashCode);
        }

        public T Identity { get; private set; }

        public override int GetHashCode() => base.GetHashCode();

        protected abstract T DefineOp(T a, T b);

        public T Op(T a, T b)
        {
            var e = OpInterne(a, b);
            Monogene(e);
            return e;
        }

        public T Invert(T e) => GetElement<T>(GetInvert(e.HashCode));

        T OpInterne(T a, T b)
        {
            if (TableOpContains(a.HashCode, b.HashCode))
                return GetElement<T>(TableOp(a.HashCode, b.HashCode));

            var e = DefineOp(a, b);
            TableOpAdd(a.HashCode, b.HashCode, e.HashCode);
            FSetAdd(e);
            return e;
        }

        protected void FGroupAdd(T e)
        {
            FSetAdd(e);
            Monogene(e);
        }

        void Monogene(T e0)
        {
            if (IsComplete(e0.HashCode))
                return;

            var acc = GetElement<T>(e0.LastHash);
            var e1 = OpInterne(e0, acc);
            if (e1.HashCode == Identity.HashCode)
            {
                MakeCompleted(e0.HashCode);
                var hashes = e0.Generated.Select(GetElement<T>).ToList();
                for (int k = 2; k < hashes.Count; ++k)
                    Monogene(hashes[k]);

                return;
            }

            if (!e0.ContainsHash(e1.HashCode))
                e0.AddHashGenerated(e1.HashCode);

            Monogene(e0);
        }
    }
}
