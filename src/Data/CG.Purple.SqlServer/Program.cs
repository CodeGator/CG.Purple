
// Visual Studio will generate this error unless we explicitly build a host here:
//   "An error occurred while accessing the Microsoft.Extensions.Hosting services.
//    Continuing without the application service provider. Error: The entry point
//    exited without ever building an IHost."
new HostBuilder().Build();
