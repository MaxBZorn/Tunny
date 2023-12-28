using System.Collections.Generic;
using System.Drawing;

using Tunny.PreProcess;

namespace Tunny.PostProcess
{
    public class TrialGrasshopperItems
    {
        public double[] ObjectiveValues { get; set; }
        public string[] GeometryJson { get; set; }
        public Dictionary<string, List<string>> Attribute { get; set; }
        public Bitmap[] ObjectiveImages { get; set; }
        public Artifact Artifacts { get; set; }
    }
}
