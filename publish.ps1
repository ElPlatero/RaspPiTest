cd App\pidrei-ng-App
ng build --prod
cd ..\..
dotnet build -c Release
dotnet publish -c Release -o P:\NET\RaspPiTest