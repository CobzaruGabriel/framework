using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mercraft.Infrastructure.Config;
using Mercraft.Infrastructure.Dependencies;
using Mercraft.Infrastructure.IO;

namespace Mercraft.Core.Elevation.Srtm
{
    public class SrtmElevationProvider : IElevationProvider, IConfigurable
	{
        private const string PathKey = "";

        private readonly IFileSystemService _fileSystemService;

        private string _dataDirectory;
	    private List<SrtmDataCell> _dataCells;

        [Dependency]
        public SrtmElevationProvider(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
            _dataCells = new List<SrtmDataCell> ();
        }

        public float GetElevation(double latitude, double longitude)
	    {
            int cellLatitude = (int)Math.Floor(Math.Abs(latitude));
            if (latitude < 0)
                cellLatitude *= -1;

            int cellLongitude = (int)Math.Floor(Math.Abs(longitude));
            if (longitude < 0)
                cellLongitude *= -1;

            for (int i = 0; i < _dataCells.Count; i++)
            {
                if (_dataCells[i].Latitude == cellLatitude && _dataCells[i].Longitude == cellLongitude)
                    return _dataCells[i].GetElevation(latitude, longitude);
            }

            string filename = string.Format("{0}{1:D2}{2}{3:D3}.hgt",
                cellLatitude < 0 ? "S" : "N",
                Math.Abs(cellLatitude),
                cellLongitude < 0 ? "W" : "E",
                Math.Abs(cellLongitude));

            string filePath = Path.Combine(_dataDirectory, filename);

            if (!File.Exists(filePath))
                throw new Exception("SRTM data cell not found: " + filePath);

            var dataCell = new SrtmDataCell(filePath, _fileSystemService);
            _dataCells.Add(dataCell);
            return dataCell.GetElevation(latitude, longitude);
	    }

	    public float GetElevation(GeoCoordinate geoCoordinate)
	    {
	        return GetElevation(geoCoordinate.Latitude, geoCoordinate.Longitude);
	    }

	    public void Configure(IConfigSection configSection)
	    {
            var path = configSection.GetString(PathKey);
	        _dataDirectory = path;
	    }
	}
}

