using Cryptool;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();

if (args.Length > 0 && args[0] == "--mcp")
{
    var builder = Host.CreateDefaultBuilder()
        .ConfigureServices(services =>
        {
            services.AddMcpServer()
                    .WithStdioServerTransport()
                    .WithTools<CryptoTools>();
        });

    await builder.Build().RunAsync();
    return;
}

try
{
    var command = CLIcipher.Parse(args);
    ExecuteCommand(command, logger);
    Environment.Exit(0);
}
catch (CLIException ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Invalid argument: {ex.Message}");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
    Environment.Exit(2);
}

static void ExecuteCommand(CLICommand command, ILogger logger)
{
    
    logger.LogInformation("Command executed");
    switch (command.Type)
    {
        case CommandType.Help:
            PrintHelp();
            logger.LogInformation("Help command executed");
            break;

        case CommandType.CaesarEncrypt:
            {
                var opts = (CaesarOptions)command.Options!;
                Console.WriteLine(Caesar.Encrypt(opts.Text, opts.Shift));
                logger.LogInformation("Caesar executed");
                break;
            }
        case CommandType.CaesarDecrypt:
            {
                var opts = (CaesarOptions)command.Options!;
                Console.WriteLine(Caesar.Decrypt(opts.Text, opts.Shift));
                logger.LogInformation("Caesar decrypt executed");
                break;
            }

        case CommandType.VigenereEncrypt:
            {
                var opts = (VigenereOptions)command.Options!;
                Console.WriteLine(Vigenere.Encrypt(opts.Text, opts.Key));
                logger.LogInformation("Vigenere executed");
                break;
            }
        case CommandType.VigenereDecrypt:
            {
                var opts = (VigenereOptions)command.Options!;
                Console.WriteLine(Vigenere.Decrypt(opts.Text, opts.Key));
                logger.LogInformation("Vigenere decrypt executed");
                break;
            }
    }
}

static void PrintHelp()
{
    Console.WriteLine(@"
cryptool — text encryption utility
 
USAGE:
  cryptool <command> [options]
 
COMMANDS:
  caesar   --text <text> --shift <n> [--decrypt]
           Example: cryptool caesar --text ""Hello"" --shift 3
           Example: cryptool caesar --text ""Khoor"" --shift 3 --decrypt
 
  vigenere --text <text> --key <word> [--decrypt]
           Example: cryptool vigenere --text ""Hello World"" --key ""KEY""
           Example: cryptool vigenere --text ""Rijvs Uyvjn"" --key ""KEY"" --decrypt
 
  help     Show this message.
 
EXIT CODES:
  0 — success
  1 — invalid arguments
  2 — internal error
");
}
