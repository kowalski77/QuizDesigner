#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Standalone/QuizDesigner.OutboxSender/QuizDesigner.OutboxSender.csproj", "Standalone/QuizDesigner.OutboxSender/"]
COPY ["Source/QuizDesigner.Outbox/QuizDesigner.Outbox.csproj", "Source/QuizDesigner.Outbox/"]
COPY ["QuizDesigner.Events/QuizDesigner.Events.csproj", "QuizDesigner.Events/"]
RUN dotnet restore "Standalone/QuizDesigner.OutboxSender/QuizDesigner.OutboxSender.csproj"
COPY . .
WORKDIR "/src/Standalone/QuizDesigner.OutboxSender"
RUN dotnet build "QuizDesigner.OutboxSender.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuizDesigner.OutboxSender.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuizDesigner.OutboxSender.dll"]