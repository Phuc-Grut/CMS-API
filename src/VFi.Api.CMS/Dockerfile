#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM harbor.vfico.com/base/dotnet:6.0 AS base
RUN apt-get update && apt-get install -y bash
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM harbor.vfico.com/base/dotnet-sdk:6.0 AS build
WORKDIR /src
COPY ["src/VFi.Api.CMS/VFi.Api.CMS.csproj", "src/VFi.Api.CMS/"]
COPY ["src/VFi.Application.CMS/VFi.Application.CMS.csproj", "src/VFi.Application.CMS/"]
COPY ["src/VFi.Domain.CMS/VFi.Domain.CMS.csproj", "src/VFi.Domain.CMS/"]
COPY ["src/VFi.NetDevPack/VFi.NetDevPack.csproj", "src/VFi.NetDevPack/"]
COPY ["src/VFi.Infra.CMS/VFi.Infra.CMS.csproj", "src/VFi.Infra.CMS/"]
RUN dotnet restore "src/VFi.Api.CMS/VFi.Api.CMS.csproj"
COPY . .
WORKDIR "/src/src/VFi.Api.CMS"
RUN dotnet build "VFi.Api.CMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VFi.Api.CMS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY /src/VFi.Api.CMS/entrypoint.sh .
RUN chmod +x ./entrypoint.sh
ENTRYPOINT ["./entrypoint.sh"]

CMD ["dotnet", "VFi.Api.CMS.dll"]