using Library.Core.IoC;
using Models.DataContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // make sure the cors statement is above AddMvc() method.
            #region CORS policy

            services.AddCors(options =>
            {
                // default policy
                options.AddDefaultPolicy(builder => builder
                                            .AllowAnyOrigin()
                                            .AllowAnyMethod()
                                            .AllowAnyHeader()
                                        );
                // named policy
                options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://localhost").AllowAnyHeader().AllowAnyMethod());
            });

            #endregion
            
            services.AddControllersWithViews();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // AntiforgeryToken CSRF or XSRF
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            #region DbContext

            //var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //var useSqlServer = Convert.ToBoolean(Configuration["BlazorAppServerSide:UseSqlServer"] ?? "false");
            //var dbConnString = useSqlServer ? Configuration.GetConnectionString("DefaultConnection")
            //                                : $"Filename={Configuration.GetConnectionString("SqlLiteConnectionFileName")}";

            //void DbContextOptionsBuilder(DbContextOptionsBuilder builder)
            //{
            //    if (useSqlServer)
            //    {
            //        builder.UseSqlServer(dbConnString, sql => sql.MigrationsAssembly(migrationsAssembly));
            //    }
            //    else if (Convert.ToBoolean(Configuration["BlazorAppServerSide:UsePostgresServer"] ?? "false"))
            //    {
            //        builder.UseNpgsql(Configuration.GetConnectionString("PostgresConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
            //    }
            //    else
            //    {
            //        builder.UseSqlite(dbConnString, sql => sql.MigrationsAssembly(migrationsAssembly));
            //    }
            //}

            services.AddDbContext<EfDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), options => options.MigrationsAssembly("WebApi")));

            //services.AddDbContext<EfDbContext>(DbContextOptionsBuilder);

            #endregion

            //services.AddTransient<IHostedService, ScheduleService>();

            #region data protection service
            //services.AddDataProtection()
            //    .SetDefaultKeyLifetime(TimeSpan.FromDays(30))
            //    .SetApplicationName("BackOfficeAPI")
            //    .DisableAutomaticKeyGeneration()
            //    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
            //    {
            //        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
            //        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
            //    });
            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromMinutes(2));
            #endregion

            #region Identity
            // Policy of IdentityOptions
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireDigit = false;
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireLowercase = false;
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            //    options.Lockout.MaxFailedAccessAttempts = 3;
            //    options.Lockout.AllowedForNewUsers = true;
            //}); 
            #endregion

            RegisterJwtBearer(services);

            //// Configure External Providers authentication
            //services.ConfigureExternalProviders(Configuration);
            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = ".AspNetCoreIdentityCookie";
            //    options.Events.OnRedirectToLogin = context =>
            //    {
            //        context.Response.Headers["Location"] = context.RedirectUri;
            //        context.Response.StatusCode = 401;
            //        return Task.CompletedTask;
            //    };
            //    options.Events.OnRedirectToAccessDenied = context =>
            //    {
            //        context.Response.Headers["Location"] = context.RedirectUri;
            //        context.Response.StatusCode = 403;
            //        return Task.CompletedTask;
            //    };
            //});

            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            #region Application Service Dependency
           
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGenreService, GenreService>();

            #endregion

            #region Swagger
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Auth Demo", Version = "v1" });

            //    var securityScheme = new OpenApiSecurityScheme
            //    {
            //        Name = "JWT Authentication",
            //        Description = "Enter JWT Bearer token **_only_**",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "bearer", // must be lower case
            //        BearerFormat = "JWT",
            //        Reference = new OpenApiReference
            //        {
            //            Id = JwtBearerDefaults.AuthenticationScheme,
            //            Type = ReferenceType.SecurityScheme
            //        }
            //    };
            //    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {securityScheme, new string[] { }}
            //    });
            //}); 
            #endregion

        }
        private void RegisterJwtBearer(IServiceCollection services)
        {
            DependencyContainer.RegisterJwtBearer(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseHttpsRedirection();

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("./swagger/v1/swagger.json", "JWT Auth Demo V1");
            //    c.DocumentTitle = "JWT Auth Demo";
            //    c.RoutePrefix = string.Empty;
            //});


            app.UseCors();
            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
