# ������� ����� ��� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# �������� ��� .csproj ����� (���������, ��� Dockerfile ������ ApiEasier.Api)
COPY ../ApiEasier.Api/ApiEasier.Api.csproj ApiEasier.Api/
COPY ../ApiEasier.Bll/ApiEasier.Bll.csproj ApiEasier.Bll/
COPY ../ApiEasier.Dal/ApiEasier.Dal.csproj ApiEasier.Dal/
COPY ../ApiEasier.Dm/ApiEasier.Dm.csproj ApiEasier.Dm/
COPY ../ApiEasier.Logger/ApiEasier.Logger.csproj ApiEasier.Logger/

# ��������������� �����������
WORKDIR /source/ApiEasier.Api
RUN dotnet restore

# �������� ���� ��� �������
WORKDIR /source
COPY ../ ./

# ������ � ����������
RUN dotnet publish ApiEasier.Api/ApiEasier.Api.csproj -c Release -o /app --no-restore

# ������� ����� ��� �������
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# �������� ��������� ���
COPY --from=build /app ./

# ������ ����������
ENTRYPOINT ["dotnet", "ApiEasier.Api.dll"]
