using BusinnesLayer.Interface;
using BusinnesLayer.Service;
using DataLayer.Repository;
using DataLayer.Repository.Interface;

namespace Blablacar;

public static class ServicesExtentions
{
    public static void AddDataLayerServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITripRepository, TripRepository>();
    }
    
    public static void AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserService, RegisterUserService>();
        services.AddScoped<ITripService, TripService>();
        services.AddScoped<IUserService, UserService>();
    }
}