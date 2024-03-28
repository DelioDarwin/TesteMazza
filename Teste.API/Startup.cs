using TesteMazza.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TesteMazza.Api
{
    public class Startup
    {
        //Local
        public const string stringConexao = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TesteMazza;Integrated Security=True";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)        
        {
            //Para uso do DataContext
            services.AddDbContext<TesteDataContext>(options => options.UseSqlServer(stringConexao));

            //services.AddDbContext<AspDbContext>(options =>    options.UseSqlServer(config.GetConnectionString("optimumDB")));
            services.AddScoped<TesteDataContext, TesteDataContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/google-login";
            })
            .AddGoogle(options =>
            {
                options.ClientId = "xxx";
                options.ClientSecret = "xxx";
            });


            services.AddControllers();

            services.Configure<IISOptions>(o =>
            {
                o.ForwardClientCertificate = false;
            });

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
