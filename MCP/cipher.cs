namespace Cryptool;

public static class Caesar
{
    public static string Encrypt(string text, int shift)
    {
        shift = ((shift % 26) + 26) % 26;  // Обробка на впадок від'ємного зміщення
        return new string(text.Select(c => ShiftChar(c, shift)).ToArray());
    }

    public static string Decrypt(string text, int shift) => Encrypt(text, -shift);

    private static char ShiftChar(char c, int shift)
    {
        if (!IsLatinLetter(c)) return c;
        char baseChar = char.IsUpper(c) ? 'A' : 'a';
        return (char)(((c - baseChar + shift) % 26) + baseChar);
    }

    public static bool IsLatinLetter(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    }
}

public static class Vigenere
{
    public static string Encrypt(string text, string key)
    {
        ValidateKey(key);
        key = key.ToUpper();
        int keyIndex = 0;
        return new string(text.Select(c =>
        {
            if (!char.IsLetter(c)) return c;
            int shift = key[keyIndex % key.Length] - 'A'; // Для кожної літере ключа отримуємо зсув
            keyIndex++;
            return Caesar.Encrypt(c.ToString(), shift)[0];// Оскільки шифр цезаря це часний випадок шифра Віженера, можна використати його з використанням знайденного зсуву
        }).ToArray());
    }

    public static string Decrypt(string text, string key)
    {
        ValidateKey(key);
        key = key.ToUpper();
        int keyIndex = 0;
        return new string(text.Select(c =>
        {
            if (!char.IsLetter(c)) return c;
            int shift = key[keyIndex % key.Length] - 'A';
            keyIndex++;
            return Caesar.Decrypt(c.ToString(), shift)[0];
        }).ToArray());
    }

    private static void ValidateKey(string key)
    {
        bool nice = true;
        foreach (char c in key) { 
        if(!IsLatinLetter(c)) nice = false;
        }

        if (string.IsNullOrWhiteSpace(key) || !nice)
            throw new ArgumentException("Vigenere key must contain only latin letters and cannot be empty.");
    }

    public static bool IsLatinLetter(char c)
    {
        return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
    }

}