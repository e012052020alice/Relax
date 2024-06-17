using Microsoft.EntityFrameworkCore;
using RelaxService;
using RelaxService.Controllers;
using RelaxService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

builder.Services.AddDbContext<SpotDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SpotsConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//千萬不可加在var app = builder.Build();
string PolicyName = "AllowAny";
builder.Services.AddCors(options => {
    options.AddPolicy(name: PolicyName, policy => {
        policy.WithOrigins("*").WithHeaders("*").WithMethods("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(); //只加管線不套用策略

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
