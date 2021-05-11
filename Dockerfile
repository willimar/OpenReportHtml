FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build   
WORKDIR /open-report-html
   
# copy source
COPY . .

RUN dotnet restore  
RUN dotnet build --configuration Release 
RUN dotnet publish -c Release -o out   

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /open-report-html   
COPY --from=build /open-report-html/out ./

# Padrão de container ASP.NET
ENTRYPOINT ["dotnet", "OpenReportHtml.dll"]
# Opção utilizada pelo Heroku
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet OpenReportHtml.dll