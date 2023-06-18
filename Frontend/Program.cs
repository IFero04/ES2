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
builder.Services.AddScoped<IFeedbackService, ServiceFeedback>();
builder.Services.AddScoped<IInscricaoService, ServiceInscricaoEvento>();
builder.Services.AddScoped<IAtividadeService, ServiceAtividade>();


await builder.Build().RunAsync();

// teste