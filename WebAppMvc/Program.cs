using Gcds.Net.Services;

using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Options;

using WebAppMvc.AppValidationAttributeAdapterProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-CA", "fr-CA" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.UseGcDesignServices();
builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapterProvider>();
//builder.Services.Configure<CustomValidationAttributeAdapterProviderOptions>(options => options.UseWebExperienceToolkit = false);
//builder.Services.AddSingleton<CustomValidationAttributeAdapterProvider>();
//builder.Services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAdapter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions!.Value);

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
