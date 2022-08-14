﻿using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.Input;
using RevitLookupWpf.View;

namespace RevitLookupWpf.PropertySys.BaseProperty.ReferenceType
{
    public class DefaultObjectProperty : ObjectProperty<object>
    {
        private RelayCommand _selectedCommand;

        public DefaultObjectProperty(string name, string fullName, object value) : base(name, fullName)
        {
            if (value != null)
            {
                Value = value;
                ValueType = value.GetType()?.Name;
            }
        }

        public string ValueType { get; set; }

        public ICommand SelectedCommand => _selectedCommand ??= new RelayCommand(Selected);

        private void Selected()
        {
            if (SnoopOption.WindowOrNavi)
            {
                var lookupWindow = new LookupWindow();
                lookupWindow.SetRvtInstance(Value);
                lookupWindow.ShowDialog();
            }
            else
            {
                //使用面包屑导航
                NaviRvtObj(Value);
            }
        }
    }
}
