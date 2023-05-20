MAIN_PROJECT := "src/Natter.Api/Natter.Api.csproj"

MIGRATIONS_ASSEMBLY := "src/Natter.Infrastructure/bin/Debug/net6.0/Natter.Infrastructure.dll"
MIGRATIONS_FOLDER := "src/Natter.Migrations/Migrations"
MIGRATIONS_NAMESPACE := "Natter.Migrations.Migrations"

CURRENT_DATETIME := `date +%Y%m%e%H%M%S`

CONNECTION_STRING := `dotnet user-secrets list --project src/Natter.Api/Natter.Api.csproj --json | tail -n +2 | head -n -1 | jq ".\"Migrations:ConnectionString\""` 

# Runs the application
run:
  dotnet run --project {{MAIN_PROJECT}}

# this requires the dotnet-t4 cli tool: https://github.com/mono/t4 
create-migration NAME:
  mkdir -p {{MIGRATIONS_FOLDER}}
  cat ./templates/migrations.tt | t4 -o {{MIGRATIONS_FOLDER}}/{{CURRENT_DATETIME}}_{{NAME}}.cs \
    -p:Namespace={{MIGRATIONS_NAMESPACE}} \
    -p:MigrationName={{NAME}} \
    -p:Datetime={{CURRENT_DATETIME}}

# Running this requires that the dotnet user-secrets and the jq be installed on the system.
# The "Migrations:ConnectionString" secret must be set on the dotnet user-secrets tool
run-migrations:
  @echo "Running Migrations..."
  @dotnet run --project src/Natter.Migrations/Natter.Migrations.csproj {{CONNECTION_STRING}}