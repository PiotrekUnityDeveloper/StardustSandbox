﻿using PixelDust.Game.Elements.Attributes;
using PixelDust.Game.Elements.Templates.Solid;

namespace PixelDust.Game.Elements.Common.Solid.Immovable
{
    [PElementRegister(11)]
    public sealed class PGlass : PImmovableSolid
    {
        protected override void OnSettings()
        {
            this.Name = "Glass";
            this.Description = string.Empty;
            this.Category = string.Empty;
            this.Texture = this.Game.AssetDatabase.GetTexture("element_1");
            this.IconTexture = this.Game.AssetDatabase.GetTexture("icon_element_12");

            this.DefaultTemperature = 25;
        }
    }
}