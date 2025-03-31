# Use the .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 80


# copy all .csproj files to restore as disctinct layers. Use of the same copy command 
# for every dockerfile in the project to take advantage of docker cache

COPY CheChe.sln CheChe.sln
COPY AuctionService/AuctionService.csproj AuctionService/AuctionService.csproj
COPY SearchService/SearchService.csproj SearchService/SearchService.csproj
COPY IdentityService/IdentityService.csproj IdentityService/IdentityService.csproj
COPY CheChe.Gateway/CheChe.Gateway.csproj CheChe.Gateway/CheChe.Gateway.csproj
COPY Contracts/Contracts.csproj Contracts/Contracts.csproj


# Restore package dependencies for all projects

RUN dotnet restore CheChe.sln

# Copy the App folders to the container

COPY SearchService SearchService
COPY Contracts Contracts
WORKDIR /app/SearchService
RUN dotnet publish -c Release -o /app/out

# Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "SearchService.dll"]