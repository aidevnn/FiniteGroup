using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteGroup
{
    public abstract class AObj
    {
        protected AObj(int[] arr)
        {
            HashCode = Helpers.GenHash(Enumerable.Range(1, arr.Length).ToArray(), arr);
        }

        protected AObj(int[] dims, int[] arr)
        {
            HashCode = Helpers.GenHash(dims, arr);
        }

        protected AObj(int dim, int[] arr)
        {
            HashCode = Helpers.GenHash(dim, arr);
        }

        protected AObj(int hash)
        {
            HashCode = hash;
        }

        public int HashCode { get; }

        public override int GetHashCode() => HashCode;
    }

    public class ObjEquality<T> : EqualityComparer<T> where T : AObj
    {
        public override bool Equals(T x, T y) => x.Equals(y);

        public override int GetHashCode(T obj) => obj.GetHashCode();
    }

    public abstract class FSet : AObj
    {
        protected FSet(int[] arr) : base(arr) { }
        protected FSet(int hash) : base(hash) { }

        protected int[] cache0, cache1, cache2;
        public string FmtElt { get; protected set; }
        public string Fmt { get; protected set; }
        protected void ClearCaches()
        {
            for (int k = 0; k < cache0.Length; ++k)
                cache0[k] = cache1[k] = cache2[k] = 0;
        }

        readonly Dictionary<int, AObj> elts = new Dictionary<int, AObj>();

        protected bool FSetContains(int hash) => elts.ContainsKey(hash);
        protected void FSetAdd(AObj obj) => elts[obj.HashCode] = obj;
        public AObj GetElement(int hash) => elts[hash];

        public U GetElement<U>(int hash) where U : AObj => (U)elts[hash];

        public IEnumerable<U> Elements<U>() where U : AObj => elts.Values.Cast<U>();

        public override int GetHashCode() => base.GetHashCode();
    }

    public abstract class Table : FSet
    {
        readonly Dictionary<(int, int), int> tableOp = new Dictionary<(int, int), int>();
        readonly HashSet<int> generationComplete = new HashSet<int>();
        private int IdHash;

        protected Table(int[] arr) : base(arr) { }
        protected Table(int hash) : base(hash) { }

        protected void TableOpAdd(int h0, int h1, int h2)
        {
            tableOp[(h0, h1)] = h2;

            if (h0 == h2 && h0 == h1)
                IdHash = h0;

            if (h2 == IdHash)
            {
                tableOp[(h0, -1)] = h1;
                tableOp[(h1, -1)] = h0;
            }
        }

        protected void MakeCompleted(int hash) => generationComplete.Add(hash);
        protected bool IsComplete(int hash) => generationComplete.Contains(hash);

        public bool TableOpContains(int h0, int h1) => tableOp.ContainsKey((h0, h1));
        public int GetInvert(int h) => tableOp[(h, -1)];
        public int TableOp(int h0, int h1) => tableOp[(h0, h1)];

        public override int GetHashCode() => base.GetHashCode();
    }

    public abstract class Elt : AObj, IComparable<Elt>
    {
        public FSet FSet { get; set; }
        protected int[] table;

        public void CopyTo(int[] cache) => table.ReCopyTo(cache);

        protected Elt(int[] dims, int[] arr) : base(dims, arr) { }
        protected Elt(int dim, int[] arr) : base(dim, arr) { }
        protected Elt(int hash) : base(hash) { }

        public abstract string[] DisplayInfos { get; }

        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(Elt other)
        {
            for (int k = 0; k < table.Length; ++k)
            {
                var e0 = table[k];
                var e1 = other.table[k];
                if (e0 != e1)
                    return e0.CompareTo(e1);
            }

            return 0;
        }

        public void Display(string name = "")
        {
            var nm = "s";
            if (!string.IsNullOrEmpty(name))
                nm = name;

            Console.WriteLine("{0} = {1}", nm, this);
        }

        public override string ToString() => string.Format(FSet.FmtElt, DisplayInfos);
    }
}
