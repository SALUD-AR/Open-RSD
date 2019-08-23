FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Msn.InteropDemo.Web/Msn.InteropDemo.Web.csproj", "Msn.InteropDemo.Web/"]
COPY ["Msn.InteropDemo.Integration/Msn.InteropDemo.Integration.csproj", "Msn.InteropDemo.Integration/"]
COPY ["Msn.InteropDemo.ViewModel/Msn.InteropDemo.ViewModel.csproj", "Msn.InteropDemo.ViewModel/"]
COPY ["Msn.InteropDemo.Common/Msn.InteropDemo.Common.csproj", "Msn.InteropDemo.Common/"]
COPY ["Msn.InteropDemo.Data/Msn.InteropDemo.Data.csproj", "Msn.InteropDemo.Data/"]
COPY ["Msn.InteropDemo.Entities/Msn.InteropDemo.Entities.csproj", "Msn.InteropDemo.Entities/"]
COPY ["Msn.InteropDemo.Fhir/Msn.InteropDemo.Fhir.csproj", "Msn.InteropDemo.Fhir/"]
COPY ["Msn.InteropDemo.Fhir.Implementacion/Msn.InteropDemo.Fhir.Implementacion.csproj", "Msn.InteropDemo.Fhir.Implementacion/"]
COPY ["Msn.InteropDemo.AppServices/Msn.InteropDemo.AppServices.csproj", "Msn.InteropDemo.AppServices/"]
COPY ["Msn.InteropDemo.Snowstorm.Implementation/Msn.InteropDemo.Snowstorm.Implementation.csproj", "Msn.InteropDemo.Snowstorm.Implementation/"]
COPY ["Msn.InteropDemo.Snowstorm.Expressions/Msn.InteropDemo.Snowstorm.Expressions.csproj", "Msn.InteropDemo.Snowstorm.Expressions/"]
COPY ["Msn.InteropDemo.Snowstorm/Msn.InteropDemo.Snowstorm.csproj", "Msn.InteropDemo.Snowstorm/"]
COPY ["Msn.InteropDemo.AppServices.Implementation/Msn.InteropDemo.AppServices.Implementation.csproj", "Msn.InteropDemo.AppServices.Implementation/"]
RUN dotnet restore "Msn.InteropDemo.Web/Msn.InteropDemo.Web.csproj"
COPY . .
WORKDIR "/src/Msn.InteropDemo.Web"
RUN dotnet build "Msn.InteropDemo.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Msn.InteropDemo.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Msn.InteropDemo.Web.dll"]