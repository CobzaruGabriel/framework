﻿using System.Collections.Generic;
using System.Linq;
using Mercraft.Core.Scene;
using Mercraft.Infrastructure.Dependencies;
using Mercraft.Core.Tiles;
using UnityEngine;

namespace Mercraft.Core.Zones
{
    public class ZoneLoader: IPositionListener
    {
        private readonly TileProvider _tileProvider;
        private readonly IFloorBuilder _floorBuilder;
        private readonly IEnumerable<ISceneModelVisitor> _sceneModelVisitors;
        
        private GeoCoordinate _relativeNullPoint;
        private List<Zone> _zones = new List<Zone>();

        [Dependency]
        public ZoneLoader(TileProvider tileProvider, 
            IFloorBuilder floorBuilder,
            IEnumerable<ISceneModelVisitor> sceneModelVisitors)
        {
            _tileProvider = tileProvider;
            _floorBuilder = floorBuilder;
            _sceneModelVisitors = sceneModelVisitors;
        }

        public void OnMapPositionChanged(Vector2 position)
        {
            if(CheckPosition(position)) 
                return;
            
            // Load zone if needed
            var tile = _tileProvider.GetTile(position, _relativeNullPoint);
            var zone = new Zone(tile, _floorBuilder, _sceneModelVisitors);
            zone.Build();
            _zones.Add(zone);           
        }

        public void OnGeoPositionChanged(GeoCoordinate position)
        {
            _relativeNullPoint = position;
            
            // TODO need think about this
            _zones = new List<Zone>();
        }

        /// <summary>
        /// Checks current position whether we need to load new zone
        /// </summary>
        private bool CheckPosition(Vector2 position)
        {
            // TODO
            return _zones.Any();
        }
    }
}
