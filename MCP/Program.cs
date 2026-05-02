using Cryptool;

try
{
    var command = CLIcipher.Parse(args);
    ExecuteCommand(command);
    return 0;
}
catch (CLIException ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    return 1;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine($"Invalid argument: {ex.Message}");
    return 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
    return 2;
}

static void ExecuteCommand(CLICommand command)
{
    switch (command.Type)
    {
        case CommandType.Help:
            PrintHelp();
            break;

        case CommandType.CaesarEncrypt:
            {
                var opts = (CaesarOptions)command.Options!;
                Console.WriteLine(Caesar.Encrypt(opts.Text, opts.Shift));
                break;
            }
        case CommandType.CaesarDecrypt:
            {
                var opts = (CaesarOptions)command.Options!;
                Console.WriteLine(Caesar.Decrypt(opts.Text, opts.Shift));
                break;
            }

        case CommandType.VigenereEncrypt:
            {
                var opts = (VigenereOptions)command.Options!;
                Console.WriteLine(Vigenere.Encrypt(opts.Text, opts.Key));
                break;
            }
        case CommandType.VigenereDecrypt:
            {
                var opts = (VigenereOptions)command.Options!;
                Console.WriteLine(Vigenere.Decrypt(opts.Text, opts.Key));
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
