Before running, please ensure to follow these steps:

1. **Set Connection String**:
   - Set `MyDbConnection` to the connection string of your Postgres database.

2. **Update Database**:
   - Run the following command from the `./Notepad.Core` directory:
     ```bash
     dotnet ef database update --startup_project="./Notepad"
     ```
3. **Regeneration of `development.jwt`**:
   - On each application startup, `development.jwt` will be regenerated with an admin user JWT.
4. **Integration tests**:
   - Covers only basic scenarios for purpose of this task.

