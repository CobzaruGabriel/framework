﻿using Mercraft.Core.Tiles;

namespace Mercraft.Core.Scene.Models
{
    public class Canvas : Model
    {
        public Tile Tile { get; set; }

        public override bool IsClosed
        {
            get { return true; }
        }
    }
}
