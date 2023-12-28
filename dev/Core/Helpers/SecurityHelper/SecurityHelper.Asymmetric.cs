using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace WinUICommunity;
public static partial class SecurityHelper
{
    private static string CryptoStringAsymmetricBase(CryptoMode cryptoMode, string value, string publicOrPrivateKey, IBuffer publicOrPrivateKeyBuffer, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        var keyBuffer = cryptoMode == CryptoMode.Encrypt
            ? CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8)
            : encodeType == EncodeType.Hex
        ? CryptographicBuffer.DecodeFromHexString(value)
        : CryptographicBuffer.DecodeFromBase64String(value);

        CryptographicKey key;

        if (publicOrPrivateKeyBuffer == null)
        {
            var keyBlob = encodeType == EncodeType.Hex
                ? CryptographicBuffer.DecodeFromHexString(publicOrPrivateKey)
                : CryptographicBuffer.DecodeFromBase64String(publicOrPrivateKey);

            key = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString())
                .ImportPublicKey(keyBlob);
        }
        else
        {
            key = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString())
                .ImportPublicKey(publicOrPrivateKeyBuffer);
        }

        var encryptedData = cryptoMode == CryptoMode.Encrypt
            ? CryptographicEngine.Encrypt(key, keyBuffer, null)
            : CryptographicEngine.Decrypt(key, keyBuffer, null);

        return cryptoMode == CryptoMode.Encrypt
            ? encodeType == EncodeType.Hex
           ? CryptographicBuffer.EncodeToHexString(encryptedData)
           : CryptographicBuffer.EncodeToBase64String(encryptedData)
            : CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, encryptedData);

    }

    public static string EncryptStringAsymmetric(string plainText, string publicKey)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, publicKey, null, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair)
    {
        keyPair = GenerateAsymmetricKeyPair(AsymmetricAlgorithm.RSA_PKCS1);
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, keyPair.ExportPublicKey(), AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, string publicKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, publicKey, null, asymmetricAlgorithm, EncodeType.Hex);
    }
    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        keyPair = GenerateAsymmetricKeyPair(asymmetricAlgorithm);
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, keyPair.ExportPublicKey(), asymmetricAlgorithm, EncodeType.Hex);
    }
    public static string EncryptStringAsymmetric(string plainText, string publicKey, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, publicKey, null, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair, EncodeType encodeType)
    {
        keyPair = GenerateAsymmetricKeyPair(AsymmetricAlgorithm.RSA_PKCS1);
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, keyPair.ExportPublicKey(), AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, string publicKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, publicKey, null, asymmetricAlgorithm, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        keyPair = GenerateAsymmetricKeyPair(asymmetricAlgorithm);
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, keyPair.ExportPublicKey(), asymmetricAlgorithm, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, publicKey, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, publicKey, asymmetricAlgorithm, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, publicKey, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Encrypt, plainText, null, publicKey, asymmetricAlgorithm, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, null, privateKey, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, null, privateKey, asymmetricAlgorithm, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, null, privateKey, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, null, privateKey, asymmetricAlgorithm, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, privateKey, null, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, privateKey, null, asymmetricAlgorithm, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, privateKey, null, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return CryptoStringAsymmetricBase(CryptoMode.Decrypt, encryptedString, privateKey, null, asymmetricAlgorithm, encodeType);
    }

    private static CryptographicKey GenerateAsymmetricKeyPairBase(AsymmetricAlgorithm asymmetricAlgorithm, RSAKeySize keySize)
    {
        AsymmetricKeyAlgorithmProvider algorithm = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString());
        return algorithm.CreateKeyPair((uint)keySize);
    }

    public static CryptographicKey GenerateAsymmetricKeyPair(AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return GenerateAsymmetricKeyPairBase(asymmetricAlgorithm, RSAKeySize.RSA512);
    }

    public static CryptographicKey GenerateAsymmetricKeyPair(RSAKeySize keySize)
    {
        return GenerateAsymmetricKeyPairBase(AsymmetricAlgorithm.RSA_PKCS1, keySize);
    }

    public static CryptographicKey GenerateAsymmetricKeyPair(AsymmetricAlgorithm asymmetricAlgorithm, RSAKeySize keySize)
    {
        return GenerateAsymmetricKeyPairBase(asymmetricAlgorithm, keySize);
    }
}
