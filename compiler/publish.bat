dotnet publish -c Release --self-contained -r osx-x64 -o ./bin/OSX -p:PublishSingleFile=true
dotnet publish -c Release --self-contained -r linux-x64 -o ./bin/unix -p:PublishSingleFile=true
dotnet publish -c Release --self-contained -r win10-x64 -o ./bin/win10 -p:PublishSingleFile=true