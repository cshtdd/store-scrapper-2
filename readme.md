# Store Scrapper 2  

An application that will leverage a publicly accessible Subway service to read store information  

## Run the tests  

```bash
dotnet test
```

## Build the app  

```bash
PROJECT_FILE=store-scrapper-2/store-scrapper-2.csproj && \
  dotnet clean $PROJECT_FILE && \
  dotnet build $PROJECT_FILE
```

## Run the application in the development machine  

```bash
pushd store-scrapper-2/bin/Debug/netcoreapp2.0 && \
  dotnet store-scrapper-2.dll && \
  popd
```

## Package the app  

[Install dotnet core](https://www.microsoft.com/net/learn/get-started/linuxubuntu)  
[Dotnet Core docker](https://github.com/dotnet/dotnet-docker)  
[Dotnet Core Production Dockerfiles](https://github.com/dotnet/dotnet-docker-samples/tree/master/dotnetapp-prod)  
[Sample Dockerfile](https://github.com/dotnet/dotnet-docker-samples/blob/master/dotnetapp-prod/Dockerfile.arm32)  

```bash
pushd store-scrapper-2 && \
  docker build -t ss2 . && \
  docker build -f Dockerfile-arm -t ss2-arm . && \
  docker system prune -f && \
  popd
```

## Publish the app  

[Configure Docker for Canister](https://canister.freshdesk.com/support/solutions/articles/14000044525-configure-the-docker-cli-for-use-with-canister)

```bash
docker login --username=tddapps cloud.canister.io:5000

docker tag ss2 cloud.canister.io:5000/tddapps/ss2 && \
  docker tag ss2-arm cloud.canister.io:5000/tddapps/ss2-arm && \
  docker push cloud.canister.io:5000/tddapps/ss2 && \
  docker push cloud.canister.io:5000/tddapps/ss2-arm && \
  docker system prune -f
```

## Verify the application logs  

Select the `filebeat-*` index 

```
Unhandled AND Exception
*SingleZipCodeProcessor AND *Processing AND Result*
SingleStorePersistor AND "Saving Store"
```
