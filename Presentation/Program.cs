using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigCors();
builder.Services.ConfigDatabase();
builder.Services.AddPersistence();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var provider = app.Services;
provider.AddDataSeeder();

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseUploadStatic();
app.MapControllers();

app.Run();
