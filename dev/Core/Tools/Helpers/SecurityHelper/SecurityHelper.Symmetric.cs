using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Security.Cryptography;

namespace WinUICommunity;
public static partial class SecurityHelper
{
    private static string EncryptStringSymmetricBase(string plainText, string key, SymmetricAlgorithm symmetricAlgorithm, EncodeType encodeType)
    {
        IBuffer keyBuffer = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
        IBuffer dataBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);

        var algoName = symmetricAlgorithm.ToString().Replace("TRIPLE_", "3");
        SymmetricKeyAlgorithmProvider symmetricProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algoName);

        CryptographicKey cryptoKey = symmetricProvider.CreateSymmetricKey(keyBuffer);

        IBuffer encryptedBuffer = CryptographicEngine.Encrypt(cryptoKey, dataBuffer, null);

        return encodeType == EncodeType.Hex
            ? CryptographicBuffer.EncodeToHexString(encryptedBuffer)
            : CryptographicBuffer.EncodeToBase64String(encryptedBuffer);
    }

    public static string EncryptStringSymmetric(string plainText, string key)
    {
        return EncryptStringSymmetricBase(plainText, key, SymmetricAlgorithm.AES_CBC_PKCS7, EncodeType.Hex);
    }
    public static string EncryptStringSymmetric(string plainText, string key, SymmetricAlgorithm symmetricAlgorithm)
    {
        return EncryptStringSymmetricBase(plainText, key, symmetricAlgorithm, EncodeType.Hex);
    }
    public static string EncryptStringSymmetric(string plainText, string key, EncodeType encodeType)
    {
        return EncryptStringSymmetricBase(plainText, key, SymmetricAlgorithm.AES_CBC_PKCS7, encodeType);
    }
    public static string EncryptStringSymmetric(string plainText, string key, SymmetricAlgorithm symmetricAlgorithm, EncodeType encodeType)
    {
        return EncryptStringSymmetricBase(plainText, key, symmetricAlgorithm, encodeType);
    }

    private static string DecryptStringSymmetricBase(string encryptedText, string key, SymmetricAlgorithm symmetricAlgorithm, EncodeType encodeType)
    {
        IBuffer keyBuffer = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);

        var encryptedBuffer = encodeType == EncodeType.Hex
            ? CryptographicBuffer.DecodeFromHexString(encryptedText)
            : CryptographicBuffer.DecodeFromBase64String(encryptedText);

        var algoName = symmetricAlgorithm.ToString().Replace("TRIPLE_", "3");

        SymmetricKeyAlgorithmProvider symmetricProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algoName);

        CryptographicKey cryptoKey = symmetricProvider.CreateSymmetricKey(keyBuffer);

        IBuffer decryptedBuffer = CryptographicEngine.Decrypt(cryptoKey, encryptedBuffer, null);

        return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedBuffer);
    }

    public static string DecryptStringSymmetric(string encryptedText, string key)
    {
        return DecryptStringSymmetricBase(encryptedText, key, SymmetricAlgorithm.AES_CBC_PKCS7, EncodeType.Hex);
    }

    public static string DecryptStringSymmetric(string encryptedText, string key, SymmetricAlgorithm symmetricAlgorithm)
    {
        return DecryptStringSymmetricBase(encryptedText, key, symmetricAlgorithm, EncodeType.Hex);
    }

    public static string DecryptStringSymmetric(string encryptedText, string key, EncodeType encodeType)
    {
        return DecryptStringSymmetricBase(encryptedText, key, SymmetricAlgorithm.AES_CBC_PKCS7, encodeType);
    }

    public static string DecryptStringSymmetric(string encryptedText, string key, SymmetricAlgorithm symmetricAlgorithm, EncodeType encodeType)
    {
        return DecryptStringSymmetricBase(encryptedText, key, symmetricAlgorithm, encodeType);
    }

    private static System.Security.Cryptography.SymmetricAlgorithm GetAESSymmetricAlgorithm(string aes_Key, string aes_IV, out string aes_KeyOut, out string aes_IVOut, EncodeType encodeType)
    {
        System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm;
        symmetricAlgorithm = Aes.Create();
        if (string.IsNullOrEmpty(aes_Key) || string.IsNullOrEmpty(aes_IV))
        {
            symmetricAlgorithm.GenerateIV();
            symmetricAlgorithm.GenerateKey();
        }
        else
        {
            byte[] keyBytes;
            byte[] ivBytes;
            if (encodeType == EncodeType.Hex)
            {
                keyBytes = Convert.FromHexString(aes_Key);
                ivBytes = Convert.FromHexString(aes_IV);
            }
            else
            {
                keyBytes = Convert.FromBase64String(aes_Key);
                ivBytes = Convert.FromBase64String(aes_IV);
            }

            symmetricAlgorithm.Key = keyBytes;
            symmetricAlgorithm.IV = ivBytes;
        }

        if (encodeType == EncodeType.Hex)
        {
            aes_KeyOut = Convert.ToHexString(symmetricAlgorithm.Key);
            aes_IVOut = Convert.ToHexString(symmetricAlgorithm.IV);
        }
        else
        {
            aes_KeyOut = Convert.ToBase64String(symmetricAlgorithm.Key);
            aes_IVOut = Convert.ToBase64String(symmetricAlgorithm.IV);
        }


        return symmetricAlgorithm;
    }

    private static void EncryptFileAESBase(Stream inputStream, Stream outputStream, string aes_Key, string aes_IV, out string aes_KeyOut, out string aes_IVOut, EncodeType encodeType)
    {
        var symmetricAlgorithm = GetAESSymmetricAlgorithm(aes_Key, aes_IV, out aes_KeyOut, out aes_IVOut, encodeType);

        using (CryptoStream cryptoStream = new CryptoStream(outputStream, symmetricAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cryptoStream.Write(buffer, 0, bytesRead);
            }

            cryptoStream.FlushFinalBlock();
        }
    }

    private static void EncryptFileAESBase2(string inputFilePath, string outputFilePath, string aes_Key, string aes_IV, out string aes_KeyOut, out string aes_IVOut, EncodeType encodeType)
    {
        using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
        {
            using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                EncryptFileAESBase(inputStream, outputStream, aes_Key, aes_IV, out aes_KeyOut, out aes_IVOut, encodeType);
            }
        }
    }

    public static void EncryptFileAES(string inputFilePath, string outputFilePath, string aes_Key, string aes_IV)
    {
        EncryptFileAESBase2(inputFilePath, outputFilePath, aes_Key, aes_IV, out _, out _, EncodeType.Base64);
    }

    public static void EncryptFileAES(string inputFilePath, string outputFilePath, string aes_Key, string aes_IV, EncodeType encodeType)
    {
        EncryptFileAESBase2(inputFilePath, outputFilePath, aes_Key, aes_IV, out _, out _, encodeType);
    }

    public static void EncryptFileAES(string inputFilePath, string outputFilePath, out string aes_KeyOut, out string aes_IVOut)
    {
        EncryptFileAESBase2(inputFilePath, outputFilePath, null, null, out aes_KeyOut, out aes_IVOut, EncodeType.Base64);
    }

    public static void EncryptFileAES(string inputFilePath, string outputFilePath, out string aes_KeyOut, out string aes_IVOut, EncodeType encodeType)
    {
        EncryptFileAESBase2(inputFilePath, outputFilePath, null, null, out aes_KeyOut, out aes_IVOut, encodeType);
    }

    private static void DecryptFileAESBase(Stream inputStream, Stream outputStream, string aes_Key, string aes_IV, EncodeType encodeType)
    {
        var symmetricAlgorithm = GetAESSymmetricAlgorithm(aes_Key, aes_IV, out _, out _, encodeType);

        using (CryptoStream cryptoStream = new CryptoStream(outputStream, symmetricAlgorithm.CreateDecryptor(), CryptoStreamMode.Write))
        {
            byte[] buffer = new byte[4096];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cryptoStream.Write(buffer, 0, bytesRead);
            }

            cryptoStream.FlushFinalBlock();
        }
    }

    private static void DecryptFileAESBase2(string inputFilePath, string outputFilePath, string aes_Key, string aes_IV, EncodeType encodeType)
    {
        using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
        {
            using (FileStream outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                DecryptFileAESBase(inputStream, outputStream, aes_Key, aes_IV, EncodeType.Base64);
            }
        }
    }

    public static void DecryptFileAES(string inputFilePath, string outputFilePath, string aes_Key, string aes_IV)
    {
        DecryptFileAESBase2(inputFilePath, outputFilePath, aes_Key, aes_IV, EncodeType.Base64);
    }
    public static void DecryptFileAES(string inputFilePath, string outputFilePath, string aes_Key, string aes_IV, EncodeType encodeType)
    {
        DecryptFileAESBase2(inputFilePath, outputFilePath, aes_Key, aes_IV, encodeType);
    }
}
