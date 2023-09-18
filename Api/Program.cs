using Application.Extensions;
using Persistence.Extensions.Persistence;
using Persistence.Extensions.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => {
    config.Enrich.FromLogContext()
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c => c.SwaggerDoc("v1",new OpenApiInfo { Title = "Task Management System", Version = "v1" })
 );
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader(); 
}));
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddRepository(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    x.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("corsapp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
