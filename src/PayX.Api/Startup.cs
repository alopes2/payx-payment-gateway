using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayX.Api.Configurations;
using PayX.Api.Controllers.Resources;
using PayX.Api.Extensions;
using PayX.Api.Metrics;
using PayX.Api.Validators;
using PayX.Bank.Services;
using PayX.Core;
using PayX.Core.Services;
using PayX.Data;
using PayX.Service;
using Prometheus;
using Serilog;

namespace PayX.Api
{
    public class Startup
    {
        private JwtSettings _jwtSettings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration
        {
            get;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services
                .AddControllers();

            // Mapping of appsettings file to an object
            _jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();

            var payxDataAssemblyName = typeof(PayXDbContext).Assembly.GetName().Name;

            services.AddDbContext<PayXDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly(payxDataAssemblyName)));

            services.AddAuth(_jwtSettings);

            services.AddSingleton<IMetricsService, MetricsService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IBankService, BankService>();

            services.AddTransient<IValidator<AuthResource>, AuthResourceValidator>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseGlobalExceptionHandler();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMetrics();

            app.UseAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

            app.UseSwaggerConfiguration();
        }
    }
}