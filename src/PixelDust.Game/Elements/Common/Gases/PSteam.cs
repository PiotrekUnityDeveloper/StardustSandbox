﻿using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Gases;

namespace PixelDust.Game.Elements.Common.Gases
{
    [PElementRegister(18)]
    public class PSteam : PGas
    {
        protected override void OnSettings()
        {
            this.Name = "Steam";
            this.Description = string.Empty;

            this.Render.AddFrame(new(8, 1));

            this.DefaultTemperature = 100;
        }
    }
}
