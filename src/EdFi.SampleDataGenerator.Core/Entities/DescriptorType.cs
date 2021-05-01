using System;

namespace EdFi.SampleDataGenerator.Core.Entities
{
    public abstract partial class DescriptorType
    {
        //In the SDG, we only care about Namespace and CodeValue when comparing
        //equivalence of Descriptors.  This is a major simplification so we can
        //compare descriptor values via the == operator.
        protected bool Equals(DescriptorType other)
        {
            return string.Equals(CodeValue, other?.CodeValue, StringComparison.OrdinalIgnoreCase) && string.Equals(Namespace, other?.Namespace, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DescriptorType)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((codeValueField?.GetHashCode() ?? 0) * 397) ^ (namespaceField?.GetHashCode() ?? 0);
            }
        }

        public static bool operator == (DescriptorType dt1, DescriptorType dt2)
        {
            if (Equals(dt1, null))
            {
                return Equals(dt2, null);
            }

            return dt1.Equals(dt2);
        }

        public static bool operator != (DescriptorType dt1, DescriptorType dt2)
        {
            return !(dt1 == dt2);
        }
    }
}
