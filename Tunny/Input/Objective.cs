using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

using Python.Runtime;

using Rhino.Geometry;

using Tunny.Component.Params;
using Tunny.Enum;
using Tunny.Type;

namespace Tunny.Input
{
    public class Objective
    {
        public List<IGH_Param> Sources { get; set; }
        public double[] Numbers { get; private set; }
        public Bitmap[] Images { get; private set; }
        public GeometryBase[] Geometries { get; private set; }
        public HumanInTheLoopType HumanInTheLoopType { get; private set; }

        public int Length => Numbers.Length + Images.Length + (Geometries.Length > 0 ? 1 : 0);
        public int NoNumberLength => Images.Length + (Geometries.Length > 0 ? 1 : 0);

        public Objective(List<IGH_Param> sources)
        {
            var numbers = new List<double>();
            var images = new List<Bitmap>();
            var geometries = new List<GeometryBase>();
            Sources = sources;

            SetParamsValue(sources, numbers, images, geometries);

            Numbers = numbers.ToArray();
            Images = images.ToArray();
            Geometries = geometries.ToArray();

            SetHumanInTheLoopType();
        }

        private static void SetParamsValue(List<IGH_Param> sources, List<double> numbers, List<Bitmap> images, List<GeometryBase> geometries)
        {
            foreach (IGH_StructureEnumerator ghEnumerator in sources.Select(objective => objective.VolatileData.AllData(false)))
            {
                foreach (IGH_Goo goo in ghEnumerator)
                {
                    bool result = goo.CastTo(out GeometryBase geometry);
                    if (result)
                    {
                        geometries.Add(geometry);
                        continue;
                    }
                    if (goo is GH_FishPrint fishPrint)
                    {
                        images.Add(fishPrint.Value);
                        continue;
                    }
                    result = goo.CastTo(out double number);
                    if (result)
                    {
                        numbers.Add(number);
                    }
                }
            }
        }

        private void SetHumanInTheLoopType()
        {
            if (NoNumberLength == 0)
            {
                HumanInTheLoopType = HumanInTheLoopType.None;
            }
            else if (NoNumberLength == 1 && Length == 1)
            {
                HumanInTheLoopType = HumanInTheLoopType.Preferential;
            }
            else
            {
                HumanInTheLoopType = HumanInTheLoopType.HumanSliderInput;
            }
        }

        public string[] GetNickNames()
        {
            var nickNames = new List<string>();
            foreach (IGH_Param source in Sources)
            {
                switch (source)
                {
                    case Param_Number param:
                        nickNames.Add(param.NickName);
                        break;
                    case Param_FishPrint param:
                        nickNames.Add("Human-in-the-Loop " + param.NickName);
                        break;
                    default:
                        break;
                }
            }
            return nickNames.ToArray();
        }

        public PyList GetPyListStyleNickname()
        {
            var name = new PyList();
            foreach (string objName in GetNickNames())
            {
                name.Append(new PyString(objName));
            }
            return name;
        }
    }
}
