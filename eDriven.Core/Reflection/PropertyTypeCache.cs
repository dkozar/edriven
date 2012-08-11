using System;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Variable type combination
    /// </summary>
    public class VariableTypeCombo
    {
        public Type Type;
        public string Variable;

        public bool Equals(VariableTypeCombo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Type, Type) && Equals(other.Variable, Variable);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (VariableTypeCombo)) return false;
            return Equals((VariableTypeCombo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Type != null ? Type.GetHashCode() : 0)*397) ^ (Variable != null ? Variable.GetHashCode() : 0);
            }
        }
    }
}