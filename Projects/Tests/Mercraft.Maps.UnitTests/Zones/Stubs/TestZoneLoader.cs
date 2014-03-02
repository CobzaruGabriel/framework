﻿using System.Collections.Generic;
using System.Linq;
using Mercraft.Core;
using Mercraft.Core.Scene;
using Mercraft.Core.Tiles;
using Mercraft.Core.Zones;
using Mercraft.Infrastructure.Dependencies;

namespace Mercraft.Maps.UnitTests.Zones.Stubs
{
    public class TestZoneLoader : ZoneLoader
    {
        [Dependency]
        public TestZoneLoader(TileProvider tileProvider, ITerrainBuilder terrainBuilder,
            IEnumerable<ISceneModelVisitor> sceneModelVisitors) :
                base(tileProvider, terrainBuilder, sceneModelVisitors)
        {
        }

        public override void OnGeoPositionChanged(GeoCoordinate position)
        {
            base.OnGeoPositionChanged(position);
        }

        public override void OnMapPositionChanged(UnityEngine.Vector2 position)
        {
            base.OnMapPositionChanged(position);
        }

        public IList<Zone> ZoneCollection
        {
            get
            {
                return Zones.Values.ToList();
            }
        }
    }
}