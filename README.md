# Live View MVP

## Running the solution

### Configure hosts to resolve services by name

```sh
sudo sh service-host-entries.sh add -i <YOUR-IP-HERE>
```

- After network change, first run remove and then add again with new IP.

### Build & Run the services as docker containers

```sh
sudo docker compose up --build
```

- Navigate to <http://webspa:4200/>

**Notes:**

- Provide the value `<YOUR-IP-HERE>` in `mediamtx.yml`.
- In non-Intel systems, undefine INTEL by commenting out in `FaceDetector.cs` before running the above command.
- In non-Linux systems, comment out mediamtx block in `docker-compose.yml` and `docker-compose.override.yml` before running the above command.
- In non-Linux systems, run mediamtx locally using release binaries.

### ⚠️ Remember to apply migrations as below

- (Might need to remove the docker volume used with database service before running the command above.)

```sh
dotnet ef database update -c appdbcontext -p ./src/Infrastructure -s ./src/PublicAPI
```
