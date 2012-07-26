#region
using System;
using System.Collections.Generic;

#endregion

namespace TimeDRODPOF.TDEditor
{
    public class TDEEntityParameter
    {
        public TDEEntityParameter(string mName, Type mType)
        {
            Name = mName;
            Type = mType;

            if (Type == typeof (int))
            {
                TypeString = "int";
                Value = 0;
            }
            if (Type == typeof (bool))
            {
                TypeString = "bool";
                Value = false;
            }
            if (Type == typeof (List<int>))
            {
                TypeString = "list<int>";
                Value = new List<int> {-1};
            }
            if (Type == typeof (string))
            {
                TypeString = "string";
                Value = "";
            }
        }

        public string Name { get; set; }
        public Type Type { get; set; }
        public string TypeString { get; set; }
        public object Value { get; set; }

        public TDEEntityParameter Clone()
        {
            var result = new TDEEntityParameter(Name, Type);

            if (Type == typeof (int))
                result.Value = Value;
            if (Type == typeof (bool))
                result.Value = Value;
            if (Type == typeof (List<int>))
                result.Value = new List<int>((List<int>) Value);
            if (Type == typeof (string))
                result.Value = Value;

            return result;
        }

        public override string ToString()
        {
            var valueString = Value.ToString();

            if (TypeString == "list<int>")
            {
                valueString = "";
                for (var x = 0; x < ((List<int>) Value).Count; x++)
                {
                    var i = ((List<int>) Value)[x];
                    valueString += i.ToString();
                    if (x != ((List<int>) Value).Count - 1) valueString += ",";
                }
            }

            return valueString;
        }
    }
}