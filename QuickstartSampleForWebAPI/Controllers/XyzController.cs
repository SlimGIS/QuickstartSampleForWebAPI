using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.WebApi;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace QuickstartSampleForWebAPI.Controllers
{
    [RoutePrefix("xyz")]
    public class XyzController : ApiController
    {
        [HttpGet]
        [Route("{z}/{x}/{y}")]
        public IHttpActionResult GetXyzTile(int z, int x, int y)
        {
            ShapefileLayer countriesLayer = new ShapefileLayer(HttpContext.Current.Server.MapPath("~/App_Data/countries-900913.shp"));
            countriesLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#AAFFDF3E"), GeoColors.White));

            MapModel mapModel = new MapModel(GeoUnit.Meter);
            mapModel.Layers.Add(countriesLayer);

            return new XyzTileResult(mapModel, x, y, z);
        }

        [HttpGet]
        [Route("identify")]
        public IHttpActionResult Identify(double x, double y, int z)
        {
            ShapefileLayer countriesLayer = new ShapefileLayer(HttpContext.Current.Server.MapPath("~/App_Data/countries-900913.shp"));
            Feature feature = countriesLayer.Identify(new GeoCoordinate(x, y), new ScaleLevels()[z].Scale, GeoUnit.Meter).FirstOrDefault();
            if (feature != null)
            {
                return Json(feature);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
