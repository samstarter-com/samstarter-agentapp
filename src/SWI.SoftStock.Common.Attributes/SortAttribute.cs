using System;

namespace SWI.SoftStock.Common.Attributes
{
    public class SortAttribute : Attribute
    {
        public SortAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}