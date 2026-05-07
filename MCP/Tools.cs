using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Cryptool;

[McpServerToolType]
public class CryptoTools
{
    [McpServerTool, Description("Encrypt or decrypt text using Caesar cipher")]
    public static string CaesarCipher(
        [Description("Text to process")] string text,
        [Description("Shift amount (integer)")] int shift,
        [Description("Set true to decrypt")] bool decrypt = false)
    {
        Console.Error.WriteLine($"[MCP] Caesar called: shift={shift} decrypt={decrypt}");
        return decrypt
            ? Caesar.Decrypt(text, shift)
            : Caesar.Encrypt(text, shift);
    }

    [McpServerTool, Description("Encrypt or decrypt text using Vigenere cipher")]
    public static string VigenereCipher(
        [Description("Text to process")] string text,
        [Description("Keyword (latin letters only)")] string key,
        [Description("Set true to decrypt")] bool decrypt = false)
    {
        Console.Error.WriteLine($"[MCP] Vigenere called: key={key} decrypt={decrypt}");
        return decrypt
            ? Vigenere.Decrypt(text, key)
            : Vigenere.Encrypt(text, key);
    }
}