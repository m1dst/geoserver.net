using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

_ = builder
    .AddContainer("geoserver", "docker.osgeo.org/geoserver", "2.28.2")
    .WithEnvironment("GEOSERVER_ADMIN_USER", "admin")
    .WithEnvironment("GEOSERVER_ADMIN_PASSWORD", "geoserver")
    .WithEndpoint(name: "http", port: 8080, targetPort: 8080, scheme: "http");

builder.Build().Run();
