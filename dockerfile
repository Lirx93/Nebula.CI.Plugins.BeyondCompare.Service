FROM dotnetcore/runtime:3.1

COPY Release /app/Release
WORKDIR /app/Release
