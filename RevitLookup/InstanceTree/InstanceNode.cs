﻿using System.Collections;
using System.Collections.ObjectModel;
using Autodesk.Revit.DB;
using GalaSoft.MvvmLight;
using RevitLookupWpf.Extension;
using RevitLookupWpf.PropertySys;

namespace RevitLookupWpf.InstanceTree
{
    public class InstanceNode<TRvtObject> : InstanceNode
    {
        public InstanceNode(TRvtObject rvtObjcet)
        {
            RvtObject = rvtObjcet;
            Name = rvtObjcet?.GetType().Name;
        }

        public TRvtObject RvtObject { get; set; }

        public override void Snoop()
        {
            if (PropertyList == null && RvtObject != null)
            {
                PropertyList = RvtObject.GetProperties();
            }
        }
    }

    public class ElementIdInstanceNode : InstanceNode<Element>
    {
        public ElementIdInstanceNode(Element rvtObjcet) : base(rvtObjcet)
        {
        }
    }

    public class IEnumerableInstanceNode : InstanceNode<IEnumerable>
    {
        private readonly IEnumerable _rvtObjcet;

        public IEnumerableInstanceNode(IEnumerable rvtObjcet):base(rvtObjcet)
        {
            _rvtObjcet = rvtObjcet;

            Name = rvtObjcet?.GetType().Name;
            GetChild();
        }

        private void GetChild()
        {
            if (Children == null)
            {
                Children = new ObservableCollection<InstanceNode>();
            }
            foreach (var item in _rvtObjcet)
            {
                var node = new InstanceNode<object>(item);
                Children.Add(node);
            }
        }
    }

    public abstract class InstanceNode : ObservableObject
    {
        #region Fields
        private PropertyList _properties;
        private bool _isSelected;
        #endregion

        #region Ctor
        public static InstanceNode Create<TRvtObjcet>(TRvtObjcet obj)
        {
            if (obj == null)
            {
                return null;
            }

            var type = obj.GetType();
            var typeName = type.Name;
            var node = default(InstanceNode);
            
            if (obj is IEnumerable enumble)
            {
                node = new IEnumerableInstanceNode(enumble);
            }
            else
            {
                switch (typeName)
                {
                    case "Document":
                        node = new DocumentInstanceNode(obj as Document);
                        break;
                    default:
                        node = new InstanceNode<object>(obj);
                        break;
                }
            }

            return node;
        }

        #endregion

        #region Properties
        public string Name { get; protected set; }

        public bool IsSelected
        {
            get => _isSelected; set
            {
                Snoop();
                Set(ref _isSelected, value);
            }
        }

        public bool IsExpanded { get; set; }

        public ObservableCollection<InstanceNode> Children { get; set; }

        public PropertyList PropertyList { get => _properties; set => Set(ref _properties, value); }
        #endregion

        public abstract void Snoop();

        #region Public Methods

        public IEnumerable<InstanceNode> RecruChild()
        {
            if (Children == null)
            {
                yield break;
            }

            foreach (var child in Children)
            {
                yield return child;

                foreach (var childChild in child.RecruChild())
                {
                    yield return childChild;
                }
            }
        }
        #endregion
    }
}