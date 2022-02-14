using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITraining_2022_3._1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementReferenceList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Face, "Выберите элементы");
            var elementList = new List<Element>();

            double SumVolume = 0;

            foreach (var selectedElement in selectedElementReferenceList)
            {
                Element element = doc.GetElement(selectedElement);
                elementList.Add(element);

                if (selectedElement is Wall)
                {
                    Parameter lengthwall1 = selectedElement.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    if (lengthwall1.StorageType == StorageType.Double)
                    {
                        Parameter widththwall1 = selectedElement.get_Parameter(BuiltInParameter.WALL_ATTR_WIDTH_PARAM);
                        if (widththwall1.StorageType == StorageType.Double)
                        {
                            Parameter heighththwall1 = selectedElement.get_Parameter(BuiltInParameter.WALL_ATTR_HEIGHT_PARAM);
                            if (heighththwall1.StorageType == StorageType.Double)
                            {
                                double wallVolume =  lengthwall1.AsDouble() * widththwall1.AsDouble() * heighththwall1.AsDouble();
                                SumVolume += wallVolume;
                            }
                        }
                    }
                }
                
            }

            TaskDialog.Show("Суммарный объем стен", SumVolume.ToString());
            return Result.Succeeded;
        }
    }
}
