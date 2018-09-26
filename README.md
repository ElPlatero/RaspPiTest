# RaspPiTest
Samll private smarthub project

Displays several applications on my Raspberry Pi3's display (a 7-inch touch screen which is mounted on a wall), allowing limited control of some smart components at home.

# API
Asp.NET Core REST/JSON-API unifying the external / internal calls. Thermostate control is implemented using DECT thermostates connected to a Fritz!Box, Weather Forecasts are parsed from an external source. Lights control is implemented by communicating with a Phillips Hue Hub.

# App
The app (App\pidrei-ng-app) is an Angular 6 SPA using Angular Material and not much more.
