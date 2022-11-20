
// In case we're called from the command line, as part of a DAL utility, we'll 
//   go ahead and create a host, configure it, and run it here.

// Create a basic ASP.NET host builder.
var builder = new HostBuilder().ConfigureServices((context, services) =>
{
    // Wire up the EFCORE data context factory.
    services.AddDbContextFactory<PurpleDbContext>(options =>
    {
        // Get the connection string.
        var connectionString = context.Configuration["DAL:ConnectionString"];

        // Use SQL-Server.
        options.UseSqlServer(connectionString);
    });
});

// Create a basic ASP.NET host.
var host = builder.Build();

// Run the host.
host.RunDelegate((host, token) =>
{
    // For now, just write something to the console.
    Console.WriteLine(
        $"{Assembly.GetExecutingAssembly().ReadProduct()} [{Assembly.GetExecutingAssembly().ReadInformationalVersion()}]"
        );
});

