using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace General
{
    public class EnumFlagAttribute : PropertyAttribute
    {
        public string Name;

        public EnumFlagAttribute() { }

        public EnumFlagAttribute(string name)
        {
            Name = name;
        }
    }
}
