docker run --name wifidb -e POSTGRES_USER=mats -e POSTGRES_PASSWORD=mats -e POSTGRES_DB=wifidb -p 5432:5432 -d postgres:16

docker exec -it wifidb psql -U mats -d wifidb

cd .\WifiAPIExam\   
```cmd
dotnet ef migrations add InitialCreate
dotnet ef database update
````
```sql
SELECT * FROM "WifiDatabase";
````