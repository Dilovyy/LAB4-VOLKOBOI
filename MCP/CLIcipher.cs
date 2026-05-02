namespace Cryptool;


public static class CLIcipher
{
    public static CLICommand Parse(string[] args)
    {
        if (args.Length == 0)
            return new CLICommand(CommandType.Help, null);

        return args[0].ToLower() switch
        {
            "caesar" => ParseCaesar(args[1..]),
            "vigenere" => ParseVigenere(args[1..]),
            "help" or "--help" or "-h" => new CLICommand(CommandType.Help, null),
            _ => throw new CLIException($"Unknown command: '{args[0]}'. Run 'cryptool help' for usage.")
        };
    }

    private static CLICommand ParseCaesar(string[] args)
    {
        string? text = GetOption(args, "--text");
        string? shiftStr = GetOption(args, "--shift");
        bool decrypt = HasFlag(args, "--decrypt");

        if (text is null)
            throw new CLIException("caesar requires --text <string>");
        if (shiftStr is null)
            throw new CLIException("caesar requires --shift <number>");
        if (!int.TryParse(shiftStr, out int shift))
            throw new CLIException($"--shift must be an integer, got: '{shiftStr}'");

        return new CLICommand(decrypt ? CommandType.CaesarDecrypt : CommandType.CaesarEncrypt,
            new CaesarOptions(text, shift));
    }

    private static CLICommand ParseVigenere(string[] args)
    {
        string? text = GetOption(args, "--text");
        string? key = GetOption(args, "--key");
        bool decrypt = HasFlag(args, "--decrypt");

        if (text is null) throw new CLIException("vigenere requires --text <string>");
        if (key is null) throw new CLIException("vigenere requires --key <word>");

        return new CLICommand(decrypt ? CommandType.VigenereDecrypt : CommandType.VigenereEncrypt,
            new VigenereOptions(text, key));
    }


    private static string? GetOption(string[] args, string name)
    {
        int idx = Array.IndexOf(args, name);
        if (idx < 0 || idx + 1 >= args.Length) return null;
        return args[idx + 1];
    }

    private static bool HasFlag(string[] args, string name) =>
        Array.IndexOf(args, name) >= 0;
}


public enum CommandType
{
    Help,
    CaesarEncrypt, CaesarDecrypt,
    VigenereEncrypt, VigenereDecrypt,
}

public record CLICommand(CommandType Type, object? Options);
public record CaesarOptions(string Text, int Shift);
public record VigenereOptions(string Text, string Key);

public class CLIException(string message) : Exception(message);