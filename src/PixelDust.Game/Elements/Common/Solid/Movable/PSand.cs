﻿using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Movable
{
    [PElementRegister(6)]
    public sealed class PSand : PMovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Sand";
            this.Description = string.Empty;

            this.Render.AddFrame(new(6, 0));

            this.DefaultTemperature = 22;
        }
    }
}
