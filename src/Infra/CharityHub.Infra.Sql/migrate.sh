#!/bin/bash

# Exit immediately if a command exits with a non-zero status
set -e

# Remove Migrations folder if it exists
echo "Removing Migrations folder..."
rm -rf ./Migrations

# Add a new Entity Framework migration named 'Init'
echo "Adding migration..."
dotnet ef migrations add Init --context CharityHubCommandDbContext

# Drop the database forcefully
echo "Dropping database..."
dotnet ef database drop --force --context CharityHubCommandDbContext

# Apply the migration and update the database
echo "Updating database..."
dotnet ef database update --context CharityHubCommandDbContext

echo "Database actions completed successfully!"
