﻿using Mercraft.Core;
using Mercraft.Core.Algorithms;
using Mercraft.Core.MapCss.Domain;
using Mercraft.Core.Scene;
using Mercraft.Core.Scene.Models;
using Mercraft.Core.Unity;
using Mercraft.Explorer.Helpers;
using Mercraft.Infrastructure.Dependencies;

namespace Mercraft.Maps.UnitTests.Zones.Stubs
{
    public class TestCylinderModelBuilder : ModelBuilder
    {
        public override string Name
        {
            get { return "cylinder"; }
        }

        [Dependency]
        public TestCylinderModelBuilder(IGameObjectFactory gameObjectFactory) : base(gameObjectFactory)
        {
        }

        public override IGameObject BuildArea(GeoCoordinate center, Rule rule, Area area)
        {
            base.BuildArea(center, rule, area);
            return BuildCylinder(center, area.Points, rule);
        }

        public override IGameObject BuildWay(GeoCoordinate center, Rule rule, Way way)
        {
            base.BuildWay(center, rule, way);
            return BuildCylinder(center, way.Points, rule);
        }

        private IGameObject BuildCylinder(GeoCoordinate center, GeoCoordinate[] points, Rule rule)
        {
            var circle = CircleHelper.GetCircle(center, points);
            var diameter = circle.Item1;
            var cylinderCenter = circle.Item2;

            var height = rule.GetHeight();
            var minHeight = rule.GetMinHeight();

            var actualHeight = (height - minHeight)/2;
            return GameObjectFactory.CreatePrimitive("", UnityPrimitiveType.Cylinder);
        }
    }
}