﻿using Mercraft.Models;

namespace Mercraft.Maps.Osm.Data
{
    /// <summary>
    /// Provides the way to get OSM datasource by geocoordinate
    /// </summary>
    public interface IDataSourceProvider
    {
        /// <summary>
        /// Returns OSM datasource by geocoordinate
        /// </summary>
        IDataSourceReadOnly Get(GeoCoordinate coordinate);
    }

    /// <summary>
    /// Trivial implementation of IDataSourceProvider
    /// TODO: for development purpose only - real implementation should be able 
    /// to return different dataSources by geo coordinates
    /// </summary>
    public class DefaultDataSourceProvider : IDataSourceProvider
    {
        private readonly IDataSourceReadOnly _dataSource;

        public DefaultDataSourceProvider(IDataSourceReadOnly dataSource)
        {
            _dataSource = dataSource;
        }

        public IDataSourceReadOnly Get(GeoCoordinate coordinate)
        {
            return _dataSource;
        }
    }
}