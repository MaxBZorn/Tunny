using System;

using Grasshopper.Kernel.Types;

using Rhino.FileIO;
using Rhino.Geometry;

namespace Tunny.Util
{
    public static class Converter
    {
        public static IGH_GeometricGoo GeometryBaseToGoo(GeometryBase geometryBase)
        {
            switch (geometryBase)
            {
                case Mesh mesh:
                    return new GH_Mesh(mesh);
                case Curve curve:
                    return new GH_Curve(curve);
                case Brep brep:
                    return new GH_Brep(brep);
                case Surface surface:
                    return new GH_Surface(surface);
                case SubD subD:
                    return new GH_SubD(subD);
                default:
                    throw new ArgumentException("Tunny only supports mesh, curve, brep, surface, subd, so convert it and enter it.");
            }
        }

        public static string GooToString(IGH_Goo goo, bool isGeometryBaseToJson)
        {
            var option = new SerializationOptions();
            return GooToString(goo, isGeometryBaseToJson, option);
        }

        private static string GooToString(IGH_Goo goo, bool isGeometryBaseToJson, SerializationOptions option)
        {
            if (isGeometryBaseToJson)
            {
                switch (goo)
                {
                    case GH_Mesh mesh:
                        return mesh.Value.ToJSON(option);
                    case GH_Brep brep:
                        return brep.IsValid ? brep.Value.ToJSON(option) : string.Empty;
                    case GH_Curve curve:
                        return curve.Value.ToJSON(option);
                    case GH_Surface surface:
                        return surface.Value.ToJSON(option);
                    case GH_SubD subd:
                        return subd.Value.ToJSON(option);
                    default:
                        return goo.ToString();
                }
            }
            else
            {
                return goo.ToString();
            }
        }
    }
}
