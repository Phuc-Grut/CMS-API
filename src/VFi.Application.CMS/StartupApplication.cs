using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VFi.NetDevPack;
using VFi.NetDevPack;

namespace VFi.Application.CMS;

public class StartupApplication : IStartupApplication
{
    public int Priority => 101;
    public bool BeforeConfigure => true;


    public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
    {
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {

    }
}
