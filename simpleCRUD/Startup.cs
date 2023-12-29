using Microsoft.EntityFrameworkCore;
using OnePhp.HRIS.Core.Data;
namespace simpleCRUD
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add your DbContext here
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}



//namespace simpleCRUD
//{
//    public class Startup
//    {
//        public IConfiguration Configuration { get; }
//        public IWebHostEnvironment Env { get; set; }
//        //readonly string BETAOrigins = "_   sbetaOrigins";
//        public Startup(IConfiguration configuration, IWebHostEnvironment env)
//        {
//            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
//            Configuration = configuration;
//            Env = env;
//        }

//        public void ConfigureService(IServiceCollection services)
//        {
//            services.AddScoped<ApplicationDbContext>();
//            services.AddDbContext<ApplicationDbContext>(
//                dbContextOptions => dbContextOptions
//                    .UseMySql(Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection")))
//                    .EnableSensitiveDataLogging()
//                    .EnableDetailedErrors()
//            );

//            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
//            {
//                o.Password.RequireDigit = true;
//                o.Password.RequireLowercase = true;
//                o.Password.RequireUppercase = true;
//                o.Password.RequireNonAlphanumeric = true;
//                o.Password.RequiredLength = 8;
//                o.SignIn.RequireConfirmedAccount = true;
//            })
//            .AddEntityFrameworkStores<ApplicationDbContext>()
//            .AddDefaultTokenProviders();

//            services.AddCors(options =>
//            {
//                options.AddPolicy(name:

//                    )
//            });

//        }

//    }
//}