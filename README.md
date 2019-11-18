# SimpleMap
This is Simple Vector Map Example.

The program reads a Mid/Mif GIS data file and creates a vector map. It supports zoom and pan operations.

![A Simple Vector Map](https://demo-01.s3-ap-southeast-2.amazonaws.com/SimpleMapDemo.jpg)

The "SampleData" folder includes six sample data files:
* layerNatural.mid
* layerNatural.mif
* layerPlaces.mid
* layerPlaces.mif
* layerRoads.mid
* layerRoads.mif

The data source comes from the OpenStreetMap project, it is the area of Bruegge in Belgium. 
https://download.bbbike.org/osm/bbbike/Bruegge/

The data is converted to Mid/Mif files using QGIS https://qgis.org/en/site/ because the Mid/Mif format is a plain text file format, appropriate for an example program.

When compiling the program, please modify the file path to the place where you put the sample data files.

```
mapLayer1 = new SimpleMap();

mapLayer1.loadPolygonData(@"e:\CSharpLocal\SimpleMap\SampleData\layerNatural.mif", "#");
            
mapLayer2 = new SimpleMap();

mapLayer2.loadPolylineData(@"e:\CSharpLocal\SimpleMap\SampleData\layerRoads.mif", "#");

mapLayer3 = new SimpleMap();

mapLayer3.loadPointData(@"e:\CSharpLocal\SimpleMap\SampleData\layerPlaces.mif", @"e:\CSharpLocal\SimpleMap\SampleData\layerPlaces.mid");
```
