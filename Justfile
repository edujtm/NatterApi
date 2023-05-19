set dotenv-load := true

MIGRATIONS_ASSEMBLY := "src/Natter.Infrastructure/bin/Debug/net6.0/Natter.Infrastructure.dll"
MIGRATIONS_FOLDER := "src/Natter.Migrations/Migrations"
MIGRATIONS_NAMESPACE := "Natter.Migrations.Migrations"

CURRENT_DATETIME := `date +%Y%m%e%H%M%S`

CONNECTION_STRING := env_var('CONNECTION_STRING')

# this requires the dotnet-t4 cli tool: https://github.com/mono/t4 
create-migration NAME:
  mkdir -p {{MIGRATIONS_FOLDER}}
  cat ./templates/migrations.tt | t4 -o {{MIGRATIONS_FOLDER}}/{{CURRENT_DATETIME}}_{{NAME}}.cs \
    -p:Namespace={{MIGRATIONS_NAMESPACE}} \
    -p:MigrationName={{NAME}} \
    -p:Datetime={{CURRENT_DATETIME}}

run-migrations:
  dotnet run --project src/Natter.Migrations/Natter.Migrations.csproj "{{CONNECTION_STRING}}"