

using Microsoft.Extensions.Configuration;
using Npgsql;



static class Database {

    static private string settings_file = 
#if DEBUG
    "appsettings.Development.json";
#else
    "appsettings.json";
#endif

    static public string? ConnectionString { get; set; }

    public static NpgsqlDataSource GetDataSource() {

        ConnectionString ??= new ConfigurationBuilder()
                .AddJsonFile(settings_file)
                .Build()
                .GetSection("ConnectionStrings")["Default"];

        return NpgsqlDataSource.Create(ConnectionString!);

    }

}