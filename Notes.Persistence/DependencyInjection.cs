using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Notes.Domain;
using Notes.Persistence.Services;

namespace Notes.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, 
            IConfiguration configuration)
        {
            // SQLite подключение - используем правильное имя строки подключения
            services.AddDbContext<NotesDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection") ?? "Data Source=notes.db",
                    b => b.MigrationsAssembly(typeof(NotesDbContext).Assembly.FullName)));
            
            services.AddScoped<INotesDbContext>(provider => provider.GetRequiredService<NotesDbContext>());
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                
                    options.User.RequireUniqueEmail = true;
                
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddEntityFrameworkStores<NotesDbContext>()
                .AddDefaultTokenProviders();
            
            return services;
        }
    }
}