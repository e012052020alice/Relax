using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Relax.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
//
builder.Services.AddDbContext<RelaxDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("RelaxConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
     .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//千萬不可加在var app = builder.Build();
string PolicyName = "AllowAny";
string MyAllowSpecificOrigins = "AllowMy";
builder.Services.AddCors(options => {
    options.AddPolicy(name: PolicyName, policy => {
        policy.WithOrigins("*").WithHeaders("*").WithMethods("*");
    });
});
builder.Services.AddCors(options => {
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy => policy.WithOrigins("http://localhost:7238", "https://relax-trip.azurewebsites.net").WithMethods("*").WithHeaders("*"));
});


builder.Services.Configure<IdentityOptions>(options => {
    // 密碼
    options.Password.RequireDigit = true; // 要求密碼至少包含一個數字
    options.Password.RequireLowercase = true; // 要求密碼至少包含一個小寫字母
    options.Password.RequireNonAlphanumeric = false; // 不要求密碼包含非字母數字字符
    options.Password.RequireUppercase = false; // 不要求密碼包含大寫字母
    options.Password.RequiredLength = 3; // 密碼最少長度為3
    options.Password.RequiredUniqueChars = 1; // 密碼中必須包含至少一個唯一字符

    // 帳戶鎖定
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // 帳戶被鎖定的預設時間為1分鐘
    options.Lockout.MaxFailedAccessAttempts = 3; // 在帳戶被鎖定前允許的最大登錄失敗次數為3
    options.Lockout.AllowedForNewUsers = true; // 新用戶也會受到鎖定政策的影響

    // 用戶名和電子郵件政策
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // 設定了允許用於用戶名的字符
    options.User.RequireUniqueEmail = true; // 要求每個用戶的電子郵件地址必須是唯一的
    options.SignIn.RequireConfirmedEmail = true; // 要求用戶在登錄前必須確認他們的電子郵件地址
});
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(); //只加管線不套用策略

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
