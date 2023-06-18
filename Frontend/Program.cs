using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Frontend;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IEventoService, ServiceEvento>();
builder.Services.AddScoped<IFeedbackService, ServiceFeedback>();
builder.Services.AddScoped<IInscricaoEventoService, ServiceInscricaoEvento>();
builder.Services.AddScoped<IInscricaoAtividadeService, ServiceInscricaoAtividade>();
builder.Services.AddScoped<IAtividadeService, ServiceAtividade>();


await builder.Build().RunAsync();
