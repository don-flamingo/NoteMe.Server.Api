using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace NoteMe.Server.Infrastructure.Sql
{
    public static class NoteMeGeometryFactory
    {
        private static readonly GeometryFactory factory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        
        public static Point CreatePoint(double lot, double lat)
        {
            return factory.CreatePoint(new Coordinate(lot, lat));
        }
    }
}