dotnet-ef dbcontext scaffold "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=postgres;" "Npgsql.EntityFrameworkCore.PostgreSQL" -p ./Infra -s ./Api --no-pluralize -o ../Domain/Commons/Entities -n Domain.Commons.Entities -c PgContext --context-dir Context --context-namespace Infra.Contexts --schema cad -f