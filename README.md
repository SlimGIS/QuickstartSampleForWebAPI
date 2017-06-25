# Quickstart Guide for SlimGIS MapKit WebAPI with OpenLayers

SlimGIS MapKit for WebAPI is a .net WebAPI component to help you to easily build up RESTful service based application. It contains full function of SlimGIS MapKit Core which comes with Geometry, GeoFunction, Symbology, Renderer, DataSource, Layer etc. In this guide, we are going to focus on set up the first web application with it. 

*Note: this guide requires some javascript and Leaflet or OpenLayers knowledge. But we won't focus on neither javascript or the 3rd party client JS map library. We will talk more about to setup a RESTful map based web application using SlimGIS MapKit for WebAPI.*

When you are reading this guide, I assume that you have installed SlimGIS Setup on your machine (if not ready, please visit [this page](http://slimgis.com/developers/installation) and make it ready for few steps).

In this guide, I will introduce the following items:

1. Scenario description.
2. Kick-start a WebAPI application project and add assembly reference.
3. Add the first Web Page and client map.
4. Add the first ApiController.
4. Add a Shapefile and set styles.
5. Identify a clicked feature.

All right, I think we can stop here. Not too much content. It is only parts of this WebAPI component. View [this page](#) for the full feature list.

## Scenario description.
What a basic map contains? I'm sure everyone has a different vision of it. Let's open the web browser and visit [maps.google.com](http://www.google.com/maps). It is the most popular map currently.  
![quickstart-guide-google](http://p1.bqimg.com/567571/baf5b2a702cd22b2.png)
A basic map application usually contains a map of course; a serial buttons to controll the map viewport (In the screenshot above, it adorns on the bottom right corner, see the little "+" and "-" button). A scale bar belows the buttons on the right of the very bottom. We can add more controls to make it convenient to use. Like displaying current mouse coordinate. This is what all for a common maps have. That's not all for this guide. You know everyone loves Google Maps, but compare with a component, we can do more as we like. In the following part, we will load our own Shapefile, set a nice style and put a lable on it; then interact with it. Isn't it cool? Let's get start.

## Kick-start a WebAPI application project and add assembly reference.
To start the demo, every developers know how to create a project with Visual Studio. In this guide, I will use Visual Studio 2015 Community version. Here are few steps, I will try to make it simple. Open VS2015 -> Go to Installed project templates/Visual C#/ASP.NET Web Application (.NET Framework) and name it `QuickstartSampleForWebAPI` -> Next -> Select the Empty ASP.NET project and check `Web API` for adding folders and core references for -> Press `OK` button to continue.

![quickstart-guide-webapi-project-option](http://i1.piimg.com/567571/2d69bf30100c1350.png)

WebAPI project is different from working with a desktop app, we don't need to drag controls around; what we are going to do next is to add the required SlimGIS assemblies to the project. Let's open your windows file explorer and navigate to *C:\Program Files (x86)\SlimGIS\SDK\3.0.0\WebAPI*. *SGMapKit.Core.dll* and *SGMapKit.WebApi.dll* are what we are going to add the assemblies into the project reference list. At this step, VisualStudio has already created the folders and core references for you. Now we are ready to code.

![quickstart-guide-webapi-project-folders](http://p1.bqimg.com/567571/0b549d2846d4e531.png)

## Add the first Web Page and client map.
In this section, we are going to do some simple things that we familiar. Add a client map. But first, a HTML page has to be create. Let's do it right now. Right click on the project -> select `Add` -> `New Item...` -> select `HTML Page` and name it *index.html* -> press `Add` button to confirm. A HTML page named *index.html* file will be created into the project root folder.

*Note: This guide is taking OpenLayers as the 3rd party client map library for example. If you want to see the Leaflet version, [click here](#).*

To add the OpenLayers library, we could use the CDN for the js and css, it is convenient for the sample right? Like I said in the top, I want to make it simple.

JS-OpenLayers: [https://cdnjs.cloudflare.com/ajax/libs/ol3/3.5.0/ol.js](https://cdnjs.cloudflare.com/ajax/libs/ol3/3.5.0/ol.js)

JS-JQuery: [http://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.1.0.min.js](http://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.1.0.min.js)

CSS: [https://cdnjs.cloudflare.com/ajax/libs/ol3/3.5.0/ol.css](https://cdnjs.cloudflare.com/ajax/libs/ol3/3.5.0/ol.css)

```
<!DOCTYPE html>
<html>
<head>
    <title>Quickstart Guide</title>
    <meta charset="utf-8" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/ol3/3.5.0/ol.css" rel="stylesheet" />
    <style>
        #map {
            width: 400px;
            height: 400px;
        }
    </style>
</head>
<body>
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.1.0.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/ol3/3.5.0/ol.js" type="text/javascript"></script>
    <div id="map" />
    <script type="text/javascript">
        let map = new ol.Map({
            layers: [new ol.layer.Tile({ source: new ol.source.OSM() })],
            target: 'map',
            view: new ol.View({ center: [0, 0], zoom: 2 })
        });
    </script>
</body>
</html>
```

![quickstart-guide-webapi-ol-map-only](http://p1.bpimg.com/567571/6c1aa45be795a34a.png)

Pretty cool? Wait, it's not what we wanna show you :) Let's keep going.

## Add the first ApiController.
Let's back to the VisualStudio Solution Explorer on the right side of the IDE. The project has a *Controller* folder, add a new controller by right click on the folder name -> choose `Add` -> `Controller...` -> `Web API 2 Controller -  Empty` and name it *XyzContrller*. The reason I call it `XyzController` is that, I'm going to create our first XYZ RESTful map service right away. The IDE creates an empty class as following:

```csharp
[RoutePrefix("xyz")]
public class XyzController : ApiController
{
}
```

We will add a `GET` method calls `GetXyzTile` like following. It takes three parameters that are name as `z`, `x`, `y`. 

1. `z` stands for `zoom level index` 
2. `x` means the column index at the specified zoom level 
3. `y` means the row index at the specified zoom level.

```
[HttpGet]
[Route("{z}/{x}/{y}")]
public IHttpActionResult GetXyzTile(int z, int x, int y)
{ }
```

The implementation of this method is the important part. Let's get into the next section.

## Add a Shapefile and set styles.
In our sample project, please look for an *App_Data* folder and copy all the files into your projects' `App_Data` folder. Then fill the implementation like this:
```csharp
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
}
```
I guess our first RESTful service is already here. Butterfly in my stomach. Something was missing, we have to set the port number, just make sure we are in the same URI. Let's set the port number to `64664` and press `F5`. Ooh, where is my countries map? Please follow me. Let's do a simple test on the web browser. Type this address: [http://localhost:64664/xyz/0/0/0](http://localhost:64664/xyz/0/0/0) We will get this:

![quickstart-guide-webapi-ol-map-countries](http://p1.bpimg.com/567571/0b1187300fd894ce.png). 

*Note: I'm useing the random style, so the color must be different from mine.*

Right, what we did just now?  I created a `ShapefileLayer` with the data we copied to the *App_Data* folder. Then we set a fill style. Then we created a `MapModel` instance that mantains the countries layer. We could set more things on this instance, we will talk about it in the further guide. Then return an `XyzTileResult` that holds the `mapModel` instance and the specified x, y, z parameters. The `XyzTileResult` will converts the map model into the image we requested in the sample URI.

The next thing is to integrate it into the map. Get back to the *index.html*. Add the layer here.

```
let map = new ol.Map({
    layers: [
        new ol.layer.Tile({ source: new ol.source.OSM() }),
        new ol.layer.Tile({
            source: new ol.source.XYZ({ url: 'http://localhost:64664/xyz/{z}/{x}/{y}' })
        })
    ],
    target: 'map',
    view: new ol.View({ center: [0, 0], zoom: 2 })
});

```

We will see the map becomes this. The yellow countries map covers on the OpenStreet Map.

![quickstart-guide-webapi-ol-map-osm-countries](http://p1.bpimg.com/567571/20210147ce952e71.png)

## Identify a clicked feature.
In this, we will do something useful: Identify a clicked feature. It means we are going to click on the map, sends the clicked location back to a RESTful API, the service looks for the location on the countries layer and find the exact country that we clicked, then send it back to client. When the client gets the callback feature, we will display a popup with the country name where we clicked.

To prepare the popup, we have to following [OpenLayers' sample guide](http://openlayers.org/en/latest/examples/popup.html) to do the following things.

Add this div below the map `div`.
```
<div id="popup" class="ol-popup">
    <div id="popup-content"></div>
</div>
```

Replace the following javascript block to the script block we have.
```
let popup = new ol.Overlay({ element: $('#popup') });

let map = new ol.Map({
    layers: [
        new ol.layer.Tile({ source: new ol.source.OSM() }),
        new ol.layer.Tile({
            source: new ol.source.XYZ({ url: 'http://localhost:64664/xyz/{z}/{x}/{y}' })
        })
    ],
    overlays: [popup],
    target: 'map',
    view: new ol.View({ center: [0, 0], zoom: 2 })
});

map.on('click', function (e) {
    let uri = 'http://localhost:64664/xyz/identify?x=' + e.coordinate[0] + '&y=' + e.coordinate[1] + '&z=' + e.map.getView().getZoom();
    $.get(uri, null, function (data) {
        let name = data.FieldValues['CNTRY_NAME'];
        $('#popup-content').text(name);
        popup.setPosition(e.coordinate);
    }).fail(function () {
        popup.setPosition(undefined);
    });
});
```

Pretty straight forward right. Let's get back to the server part.

```
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
```

Now press `F5` to display the map, and click a country on the map.

![quickstart-guide-webapi-ol-map-countries-popup](http://i1.piimg.com/567571/cc35bcbda21f45de.png)

That'a all for the scenario now. I'm sure you have more ideas for this guide. Please feel free to create a pull request, we are glad to take suggestions from you. Also, let us know how you think by dev@slimgis.com.
