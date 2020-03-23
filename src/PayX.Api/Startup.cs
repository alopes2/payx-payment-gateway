using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayX.Api.Configurations;
using PayX.Api.Extensions;
using PayX.Api.Metrics;
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

            services.AddControllers();

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

            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PayX - Payment Gateway",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT containing userid claim",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                OpenApiSecurityRequirement security =
                              new OpenApiSecurityRequirement
                              {
                                      {
                                          new OpenApiSecurityScheme
                                          {
                                            Reference = new OpenApiReference
                                            {
                                                Id = "Bearer",
                                                Type = ReferenceType.SecurityScheme
                                            },
                                            UnresolvedReference = true
                                          }, new List<string>()
                                      }
                              };

                c.AddSecurityRequirement(security);
            });
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PayX V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}