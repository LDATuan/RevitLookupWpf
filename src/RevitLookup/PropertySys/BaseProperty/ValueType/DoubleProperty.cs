﻿namespace RevitLookupWpf.PropertySys.BaseProperty.ValueType
{
    public class DoubleProperty : PropertyBase<double>
    {
        public DoubleProperty(string name, double value) : base(name)
        {
            Value = value;
        }
    }
}