using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace WinUICommunity;
public static partial class SecurityHelper
{
    private static string EncryptStringSymmetricBase(string plainText, string key, SymmetricAlgorithm symmetricAlgorithm, EncodeType encodeType)
    {
        IBuffer keyBuffer = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
        IBuffer dataBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);

        var algoName = symmetricAlgorithm.ToString().Replace("TRIPLE_", "3");
        SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algoName);

        CryptographicKey cryptoKey = aesProvider.CreateSymmetricKey(keyBuffer);

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

        SymmetricKeyAlgorithmProvider aesProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(algoName);

        CryptographicKey cryptoKey = aesProvider.CreateSymmetricKey(keyBuffer);

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
}
