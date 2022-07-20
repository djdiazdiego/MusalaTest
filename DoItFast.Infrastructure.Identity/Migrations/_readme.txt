
To add migration:

	Add-Migration -Name MigrationName -Project DoItFast.Infrastructure.identity -Context identityContext -StartupProject DoItFast.WebApi -OutputDir Migrations -Verbose

To update database:

	Update-Database -Project DoItFast.Infrastructure.identity -Context identityContext -StartupProject DoItFast.WebApi -Verbose

To remove a migration (not runned):
	Remove-Migration -Project DoItFast.Infrastructure.identity -Context identityContext -StartupProject DoItFast.WebApi -Verbose

To undo an already runned execute migration follow this steps:
	1. Update the Database to the previous migration:

		Update-Database -Migration MigrationName -Project DoItFast.Infrastructure.identity -Context identityContext -StartupProject DoItFast.WebApi -Verbose
	
	2. Remove the latest migrations:
		
		Remove-Migration -Project DoItFast.Infrastructure.identity -Context identityContext -StartupProject DoItFast.WebApi -Verbose