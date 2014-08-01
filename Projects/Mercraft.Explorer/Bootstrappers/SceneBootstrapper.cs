﻿using Mercraft.Core.MapCss;
using Mercraft.Core.Scene;
using Mercraft.Explorer.Builders;
using Mercraft.Explorer.Themes;
using Mercraft.Infrastructure.Bootstrap;
using Mercraft.Infrastructure.Config;
using Mercraft.Infrastructure.Dependencies;
using Mercraft.Models.Buildings;
using Mercraft.Models.Buildings.Facades;
using Mercraft.Models.Buildings.Roofs;
using Mercraft.Models.Roads;
using Mercraft.Models.Terrain;

namespace Mercraft.Explorer.Bootstrappers
{
    public class SceneBootstrapper: BootstrapperPlugin
    {
        private const string ThemeKey = "theme";
        private const string StylesheetKey = "mapcss";

        public override string Name { get { return "scene"; } }

        public override bool Run()
        {
            var themeConfigPath = GlobalConfigSection.GetString(ThemeKey);
            var themeConfig = new ConfigSettings(themeConfigPath, PathResolver);
            // register theme provider
            Container.Register(Component
                .For<IThemeProvider>()
                .Use<ThemeProvider>()
                .Singleton()
                .SetConfig(themeConfig.GetRoot()));

            // register stylesheet provider
            Container.Register(Component
                .For<IStylesheetProvider>()
                .Use<StylesheetProvider>()
                .Singleton()
                .SetConfig(GlobalConfigSection.GetSection(StylesheetKey)));

            // register model builders
            Container.Register(Component.For<IModelBuilder>().Use<BuildingModelBuilder>().Named("building").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<SolidModelBuilder>().Named("solid").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<FlatModelBuilder>().Named("flat").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<SphereModelBuilder>().Named("sphere").Singleton());
            Container.Register(Component.For<IModelBuilder>().Use<CylinderModelBuilder>().Named("cylinder").Singleton());
            
            // register core behaviours
            // NOTE no standard behaviours so far

            // buildings
            Container.Register(Component.For<IBuildingStyleProvider>().Use<BuildingStyleProvider>().Singleton());
            Container.Register(Component.For<IBuildingBuilder>().Use<BuildingBuilder>().Singleton());
            Container.Register(Component.For<IFacadeBuilder>().Use<FlatFacadeBuilder>().Named("flat").Singleton());
            Container.Register(Component.For<IRoofBuilder>().Use<FlatRoofBuilder>().Named("flat").Singleton());

            // terrain
            Container.Register(Component.For<ITerrainBuilder>().Use<TerrainBuilder>().Singleton());
            
            // roads
            Container.Register(Component.For<IRoadStyleProvider>().Use<RoadStyleProvider>().Singleton());
            Container.Register(Component.For<IRoadBuilder>().Use<RoadBuilder>().Singleton());

            return true;
        }
    }
}
