FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src

# It's important to keep lines from here down to "COPY . ." identical in all Dockerfiles
# to take advantage of Docker's build cache, to speed up local container builds
COPY "Fee.sln" "Fee.sln"

COPY "ApiGateways/Mobile.Bff.Applying/aggregator/Mobile.Applying.HttpAggregator.csproj" "ApiGateways/Mobile.Bff.Applying/aggregator/Mobile.Applying.HttpAggregator.csproj"
COPY "ApiGateways/Web.Bff.Applying/aggregator/Web.Applying.HttpAggregator.csproj" "ApiGateways/Web.Bff.Applying/aggregator/Web.Applying.HttpAggregator.csproj"
COPY "ApiGateways/Mobile.Bff.Scholarship/aggregator/Mobile.Scholarship.HttpAggregator.csproj" "ApiGateways/Mobile.Bff.Scholarship/aggregator/Mobile.Scholarship.HttpAggregator.csproj"
COPY "ApiGateways/Web.Bff.Scholarship/aggregator/Web.Scholarship.HttpAggregator.csproj" "ApiGateways/Web.Bff.Scholarship/aggregator/Web.Scholarship.HttpAggregator.csproj"
COPY "BuildingBlocks/DevspacesSupport/Devspaces.Support/Devspaces.Support.csproj" "BuildingBlocks/DevspacesSupport/Devspaces.Support/Devspaces.Support.csproj"
COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj" "BuildingBlocks/EventBus/EventBusServiceBus/EventBusServiceBus.csproj"
COPY "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj" "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj"
COPY "BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj" "BuildingBlocks/WebHostCustomization/WebHost.Customization/WebHost.Customization.csproj"
COPY "Services/Applying/Applying.API/Applying.API.csproj" "Services/Applying/Applying.API/Applying.API.csproj"
COPY "Services/Applying/Applying.BackgroundTasks/Applying.BackgroundTasks.csproj" "Services/Applying/Applying.BackgroundTasks/Applying.BackgroundTasks.csproj"
COPY "Services/Applying/Applying.Domain/Applying.Domain.csproj" "Services/Applying/Applying.Domain/Applying.Domain.csproj"
COPY "Services/Applying/Applying.Infrastructure/Applying.Infrastructure.csproj" "Services/Applying/Applying.Infrastructure/Applying.Infrastructure.csproj"
COPY "Services/Applying/Applying.SignalrHub/Applying.SignalrHub.csproj" "Services/Applying/Applying.SignalrHub/Applying.SignalrHub.csproj"
COPY "Services/Basket/Applying.Basket.API/Applying.Basket.API.csproj" "Services/Basket/Applying.Basket.API/Applying.Basket.API.csproj"
COPY "Services/Identity/CorporateSponsorIdentity.API/CorporateSponsorIdentity.API.csproj" "Services/Identity/CorporateSponsorIdentity.API/CorporateSponsorIdentity.API.csproj"
COPY "Services/Identity/Sponsor.API/Sponsor.API.csproj" "Services/Identity/Sponsor.API/Sponsor.API.csproj"
COPY "Services/Identity/Student.Identity.API/Student.Identity.API.csproj" "Services/Identity/Student.Identity.API/Student.Identity.API.csproj"
COPY "Services/Payment/Payment.API/Payment.API.csproj" "Services/Payment/Payment.API/Payment.API.csproj"
COPY "Services/Scholarship/Scholarship.API/Scholarship.API.csproj" "Services/Scholarship/Scholarship.API/Scholarship.API.csproj"
COPY "Services/Webhooks/Webhooks.API/Webhooks.API.csproj" "Services/Webhooks/Webhooks.API/Webhooks.API.csproj"
COPY "Web/WebhookClient/WebhookClient.csproj" "Web/WebhookClient/WebhookClient.csproj"
COPY "Web/ScholarshipMVC/ScholarshipMVC.csproj" "Web/ScholarshipMVC/ScholarshipMVC.csproj"
COPY "Web/WebMVC/WebMVC.csproj" "Web/WebMVC/WebMVC.csproj"
COPY "Web/WebSPA/WebSPA.csproj" "Web/WebSPA/WebSPA.csproj"
COPY "Web/WebStatus/WebStatus.csproj" "Web/WebStatus/WebStatus.csproj"

COPY "docker-compose.dcproj" "docker-compose.dcproj"

COPY "NuGet.config" "NuGet.config"

RUN dotnet restore "Fee.sln"

COPY . .
WORKDIR /src/Services/Identity/Sponsor.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Sponsor.API.dll"]
