using VNK.Omega.Example.Workers;
using VNK.Omega.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOmega(setup =>
{
    //default
    setup.EnableLogin = true;
    setup.Retries = 3;
    setup.MinutesToRetry = 5;
});
builder.Services.AddWorker<WorkerClass1>();
builder.Services.AddWorker<WorkerClass2>();

var app = builder.Build();

app.MapControllers();

app.Run();