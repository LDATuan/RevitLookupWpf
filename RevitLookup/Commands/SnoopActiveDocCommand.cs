﻿/*
 * Created By WeiGan 2021.9.9
 * 
 */

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitLookupWpf.Helpers;
using RevitLookupWpf.View;

namespace RevitLookupWpf.Commands
{
    [RvtCommandInfo(Name = "Snoop\nActiveDoc", Image = "search.png")]
    [Transaction(TransactionMode.Manual)]
    public class SnoopActiveDocCommand : RvtCommandBase
    {
        public override Result SnoopClick(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (commandData.Application.ActiveUIDocument == null)
            {
                message = Resource.NoActiveDocument;
                return Result.Cancelled;
            }

            try
            {                
                var lookupWindow = new LookupWindow(commandData);
                lookupWindow.SetRvtInstance(commandData.Application.ActiveUIDocument.Document);
                lookupWindow.Show();
            }
            catch (Exception)
            {
                throw;
            }

            return Result.Succeeded;
        }
    }
}
