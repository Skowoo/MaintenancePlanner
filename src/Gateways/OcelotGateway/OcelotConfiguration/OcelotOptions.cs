using MMLib.SwaggerForOcelot.Configuration;

namespace OcelotGateway.OcelotConfiguration
{
    public static class OcelotOptions
    {
        public static void ConfigureOcelotWithSwaggerOptions(OcelotWithSwaggerOptions options)
        {
            options.Folder = "OcelotConfiguration";
        }
    }
}
