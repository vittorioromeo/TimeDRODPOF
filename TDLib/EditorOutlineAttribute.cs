#region
using System;

#endregion

namespace TimeDRODPOF.TDLib
{
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class EditorOutlineAttribute : Attribute
    {
        public EditorOutlineAttribute(int mUID) { UID = mUID; }

        public int UID { get; private set; }
    }
}