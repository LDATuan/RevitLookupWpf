﻿using System.Windows;
using System.Windows.Interop;
using GalaSoft.MvvmLight.Messaging;
using RevitLookupWpf.ViewModel;

namespace RevitLookupWpf.View
{
    /// <summary>
    /// Interaction logic for LookupWindow.xaml
    /// </summary>
    public partial class LookupWindow : Window
    {
        private LookupWindowViewModel _viewModel = new LookupWindowViewModel();

        public LookupWindow()
        {
            InitializeComponent();

            _viewModel.CloseAction = new Action(() => Close());
            this.Closed += LookupWindow_Closed;
            DataContext = _viewModel;
        }

        public LookupWindow(IntPtr parentHandle)
        {
            InitializeComponent();

            //设置父窗口为Revit
            var windowInteropHelper = new WindowInteropHelper(this);
            windowInteropHelper.Owner = parentHandle;

            _viewModel.CloseAction = new Action(() => Close());
            DataContext = _viewModel;
        }

        public bool SetRvtInstance<TRvtObject>(TRvtObject rvtObject)
        {
            return _viewModel.SetRvtInstance(rvtObject);
        }

        private void LookupWindow_Closed(object sender, EventArgs e)
        {
            this.Closed -= LookupWindow_Closed;

            Messenger.Default.Unregister(_viewModel);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _viewModel.SelectedItemChangedCommand?.Execute(null);
        }
    }
}