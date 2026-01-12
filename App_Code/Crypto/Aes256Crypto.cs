using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class Aes256Crypto
{
    // Requisito do projeto
    private const string MASTER_KEY = "MLEGATE_PKG_KEY";

    // Parâmetros estáveis (podem ser ajustados, mas mantenha fixo após ir pra produção)
    private const int SALT_SIZE = 16;  // 128-bit
    private const int IV_SIZE = 16;    // 128-bit (AES block size)
    private const int KEY_SIZE = 32;   // 256-bit
    private const int PBKDF2_ITERS = 100000;

    /// <summary>
    /// Criptografa string usando AES-256 (CBC + PKCS7) com chave derivada por PBKDF2.
    /// Retorna: "v1:" + Base64(salt + iv + cipher)
    /// </summary>
    public static string Encrypt(string plainText)
    {
        if (plainText == null) plainText = "";

        byte[] salt = RandomBytes(SALT_SIZE);
        byte[] iv = RandomBytes(IV_SIZE);

        byte[] key = DeriveKey(MASTER_KEY, salt);

        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock();
                }

                byte[] cipher = ms.ToArray();

                // payload = salt + iv + cipher
                byte[] payload = new byte[salt.Length + iv.Length + cipher.Length];
                Buffer.BlockCopy(salt, 0, payload, 0, salt.Length);
                Buffer.BlockCopy(iv, 0, payload, salt.Length, iv.Length);
                Buffer.BlockCopy(cipher, 0, payload, salt.Length + iv.Length, cipher.Length);

                return "v1:" + Convert.ToBase64String(payload);
            }
        }
    }

    /// <summary>
    /// Descriptografa string no formato "v1:Base64(salt+iv+cipher)".
    /// </summary>
    public static string Decrypt(string encrypted)
    {
        if (string.IsNullOrEmpty(encrypted)) return "";

        if (!encrypted.StartsWith("v1:", StringComparison.Ordinal))
            throw new CryptographicException("Formato inválido. Esperado prefixo v1:.");

        string b64 = encrypted.Substring(3);
        byte[] payload = Convert.FromBase64String(b64);

        if (payload.Length < (SALT_SIZE + IV_SIZE + 1))
            throw new CryptographicException("Payload inválido.");

        byte[] salt = new byte[SALT_SIZE];
        byte[] iv = new byte[IV_SIZE];
        byte[] cipher = new byte[payload.Length - SALT_SIZE - IV_SIZE];

        Buffer.BlockCopy(payload, 0, salt, 0, SALT_SIZE);
        Buffer.BlockCopy(payload, SALT_SIZE, iv, 0, IV_SIZE);
        Buffer.BlockCopy(payload, SALT_SIZE + IV_SIZE, cipher, 0, cipher.Length);

        byte[] key = DeriveKey(MASTER_KEY, salt);

        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(cipher, 0, cipher.Length);
                cs.FlushFinalBlock();

                byte[] plainBytes = ms.ToArray();
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }

    /// <summary>
    /// Compatível com legado: se vier sem "v1:", devolve como está (plaintext antigo).
    /// </summary>
    public static string DecryptSmart(string valueFromDb)
    {
        if (string.IsNullOrEmpty(valueFromDb)) return "";
        if (!valueFromDb.StartsWith("v1:", StringComparison.Ordinal)) return valueFromDb; // legado
        return Decrypt(valueFromDb);
    }

    private static byte[] DeriveKey(string passphrase, byte[] salt)
    {
        using (var kdf = new Rfc2898DeriveBytes(passphrase, salt, PBKDF2_ITERS, HashAlgorithmName.SHA256))
            return kdf.GetBytes(KEY_SIZE);
    }

    private static byte[] RandomBytes(int len)
    {
        byte[] b = new byte[len];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(b);
        return b;
    }
}
