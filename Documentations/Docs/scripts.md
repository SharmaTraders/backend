### Build docker: 
```bash
cd ..\..\
docker build ./src -t backend_server
```

### Run the container: 
```bash
cd ..\..\
docker compose up
```

### Build Ef migrations:
Please replace {migrationName} with the name of the migration
```bash
cd ..\..\src\Persistence\
 dotnet ef --startup-project ../WebApi/ migrations add {migrationName} --context ReadDatabaseContext
```

### Scaffold the database:
```bash
cd ..\..\src\Query\
dotnet ef dbcontext scaffold "Server=localhost;Database=SharmaTraders;Port=5432;User ID=postgres;Password=postgres;" Npgsql.EntityFrameworkCore.PostgreSQL --force
```


