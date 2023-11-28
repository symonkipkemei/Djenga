

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Interop;
using Autodesk.Revit.Attributes;
using Djenga.View;

namespace Djenga
{


    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        { 
            // Show setting table
            UserSelection selectionForm = new UserSelection(commandData);

            //  Get the revit handle
            IntPtr revitHandle = commandData.Application.MainWindowHandle;

            //Host the WPF within revit
            WindowInteropHelper helper = new WindowInteropHelper(selectionForm)
            {
                Owner = revitHandle
            };

            // show the WPF dialog

            selectionForm.ShowDialog();
         

            return Result.Succeeded;
        }
    }
}