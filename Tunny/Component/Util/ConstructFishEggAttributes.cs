using System;
using System.Drawing;
using System.Linq;

using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

namespace Tunny.Component.Util
{
    public partial class ConstructFishEgg
    {
        private sealed class ConstructFishEggAttributes : Tunny_ComponentAttributes
        {
            public ConstructFishEggAttributes(IGH_Component component) : base(component)
            {
            }

            protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
            {
                switch (channel)
                {
                    case GH_CanvasChannel.First:
                        if (Selected)
                        {
                            RenderInputComponentBoxes(graphics);
                        }
                        break;
                    case GH_CanvasChannel.Wires:
                        DrawWires(canvas, graphics);
                        break;
                    default:
                        base.Render(canvas, graphics, channel);
                        break;
                }
            }

            private void RenderInputComponentBoxes(Graphics graphics)
            {
                Brush fill = new SolidBrush(Color.FromArgb(Convert.ToInt32("99FFA500", 16)));
                Pen edge = Pens.Orange;
                foreach (Guid guid in Owner.Params.Input[0].Sources.Select(s => s.InstanceGuid))
                {
                    RenderBox(graphics, fill, edge, guid);
                }
            }

            private void DrawWires(GH_Canvas canvas, Graphics graphics)
            {
                Wire[] wires = Owner.Attributes.Selected
                    ? (new[]
                    {
                        new Wire(3, Color.Orange),
                        new Wire(3, GH_Skin.wire_selected_a, GH_Skin.wire_selected_b),
                        new Wire(3, GH_Skin.wire_selected_a, GH_Skin.wire_selected_b),
                    })
                    : (new[]
                    {
                        new Wire(2, Color.FromArgb(Convert.ToInt32("33FFA500", 16))),
                        new Wire(3, GH_Skin.wire_default),
                        new Wire(3, GH_Skin.wire_default)
                    });

                for (int i = 0; i < 3; i++)
                {
                    DrawPath(canvas, graphics, Owner.Params.Input[i], wires[i]);
                }
            }
        }
    }
}
