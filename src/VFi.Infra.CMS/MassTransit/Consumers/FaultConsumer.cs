using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace VFi.Infra.CMS.MassTransit.Consumers;

public class FaultConsumer<T> : IConsumer<Fault<T>>
{
    private ILogger _logger;

    public FaultConsumer(ILogger<T> logger)
    {

        _logger = logger;
    }

    public async Task Consume(ConsumeContext<Fault<T>> context)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine();
        Console.WriteLine("There was an error {0}:", context.Message.Message);
        Console.ResetColor();
        await Task.CompletedTask;
    }
}
