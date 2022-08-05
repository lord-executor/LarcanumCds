using LarcanumCds.Server;
using LarcanumCds.Server.Configuration;
using SixLabors.ImageSharp.Web.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
	options.AddPolicy("ContentPolicy", policy =>
	{
		policy.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<SourceSettings>(builder.Configuration.GetSection(SourceSettings.SectionKey));

builder.Services.AddImageSharp()
	.ClearProviders()
	.AddProvider<ContentFileImageProvider>();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("ContentPolicy");
app.UseAuthorization();
app.UseImageSharp();

app.MapControllers();

app.Run();
