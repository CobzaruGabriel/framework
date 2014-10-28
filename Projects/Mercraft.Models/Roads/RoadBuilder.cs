﻿using System.Collections.Generic;
using System.Linq;
using Mercraft.Core.Elevation;
using Mercraft.Core.Scene.World.Roads;
using Mercraft.Infrastructure.Dependencies;
using Mercraft.Models.Geometry.ThickLine;
using Mercraft.Models.Utils;
using UnityEngine;

namespace Mercraft.Models.Roads
{
    /// <summary>
    ///     Provides the way to build road using road model.
    /// </summary>
    public interface IRoadBuilder
    {
        /// <summary>
        ///     Builds road.
        /// </summary>
        /// <param name="heightMap">Height map.</param>
        /// <param name="road">Road.</param>
        /// <param name="style">Style.</param>
        void Build(HeightMap heightMap, Road road, RoadStyle style);
    }

    /// <summary>
    ///     Defaul road builder.
    /// </summary>
    public class RoadBuilder : IRoadBuilder
    {
        private readonly IResourceProvider _resourceProvider;
        private readonly ThickLineBuilder _lineBuilder = new ThickLineBuilder();

        /// <summary>
        ///     Creates RoadBuilder.
        /// </summary>
        /// <param name="resourceProvider">Resource provider.</param>
        [Dependency]
        public RoadBuilder(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        /// <inheritdoc />
        public void Build(HeightMap heightMap, Road road, RoadStyle style)
        {
            var lineElements = road.Elements.Select(e => new LineElement(e.Points, e.Width)).ToList();
            _lineBuilder.Build(heightMap, lineElements, (p, t, u) => CreateMesh(road, style, p, t, u));
        }

        /// <summary>
        ///     Creates unity's mesh.
        /// </summary>
        /// <param name="road">Road.</param>
        /// <param name="style">Style.</param>
        /// <param name="points">Points.</param>
        /// <param name="triangles">Triangles.</param>
        /// <param name="uv">UV.</param>
        protected virtual void CreateMesh(Road road, RoadStyle style,
            List<Vector3> points, List<int> triangles, List<Vector2> uv)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = points.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();

            var gameObject = road.GameObject.GetComponent<GameObject>();
            gameObject.isStatic = true;
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            gameObject.AddComponent<MeshCollider>();
            gameObject.AddComponent<RoadBehavior>().Road = road;
            gameObject.tag = "osm.road";

            var renderer = gameObject.AddComponent<MeshRenderer>();

            renderer.sharedMaterial = _resourceProvider.GetMatertial(style.Path);
        }
    }
}
