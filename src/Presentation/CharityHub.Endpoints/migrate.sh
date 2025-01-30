#!/bin/bash

# Name of the migration
MIGRATION_NAME=$1

if [ -z "$MIGRATION_NAME" ]; then
    echo "Error: Migration name is required."
    echo "Usage: ./migrate.sh <MigrationName>"
    exit 1
fi

echo "Starting migration process..."

# Add Migration for Command DbContext
echo "Adding migration for Command DbContext..."
dotnet ef migrations add "$MIGRATION_NAME" --context CharityHubCommandDbContext --output-dir Migrations/CommandDb/Data

# Update Database for Command DbContext
echo "Updating database for Command DbContext..."
dotnet ef database update --context CharityHubCommandDbContext


echo "Migration and update process completed successfully!"
