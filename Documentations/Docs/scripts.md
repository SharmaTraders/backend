### Build docker: 
```bash
docker build ./src -t backend_server
cd ..\..\
```

### Run the container: 
```bash
docker compose up
cd ..\..\
```

### Build Ef migrations:
Please replace {migrationName} with the name of the migration
```bash
 dotnet ef --startup-project ../WebApi/ migrations add EmployeeSalary --context WriteDatabaseContext
cd ..\..\src\Persistence\
```

### Scaffold the database:
```bash
dotnet ef dbcontext scaffold "Server=localhost;Database=SharmaTraders;Port=5432;User ID=postgres;Password=postgres;" Npgsql.EntityFrameworkCore.PostgreSQL --force
cd ..\..\src\Query\
```


