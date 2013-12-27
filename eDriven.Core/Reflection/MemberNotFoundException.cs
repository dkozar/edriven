using System;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string message) : base(message)
        {
        }
    }
}