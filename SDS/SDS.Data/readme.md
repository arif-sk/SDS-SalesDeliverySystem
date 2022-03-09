#If ef not installed already,
  dotnet tool install --global dotnet-ef 

#Add New Migration
  dotnet ef migrations add <migrationName> --project SDS.Data

#udpate database
  dotnet ef database update --project SDS.Data

#Remove last migration
 dotnet ef migrations remove --project SDS.Data

#Generate Scripts
 The below will create migration script at the root folder of the project
 dotnet ef migrations script 20210112071229_AppointmentSettings --project SDS.Data --output ./migration.sql


 #Add New Migration
	Add-Migration InitialCreate -Context ApplicationContext -Project SDS.Data

#Update Database
	Update-Database -Context ApplicationContext -Project SDS.Data