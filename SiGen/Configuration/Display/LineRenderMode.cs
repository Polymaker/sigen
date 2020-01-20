using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SiGen.Configuration.Display
{
    public enum LineRenderMode
    {
        [LocDescription("LineRenderMode_PlainLine")]
        PlainLine,
        [LocDescription("LineRenderMode_RealWidth")]
        RealWidth,
        [LocDescription("LineRenderMode_RealisticLook")]
        RealisticLook
    }
}
