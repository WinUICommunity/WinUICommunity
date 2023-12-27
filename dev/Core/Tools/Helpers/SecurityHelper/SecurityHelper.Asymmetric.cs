using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Security.Cryptography;

namespace WinUICommunity;
public static partial class SecurityHelper
{
    private static string EncryptStringAsymmetricBase(string plainText, string publicKey, IBuffer publicKeyBuffer, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        IBuffer keyBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf8);

        CryptographicKey key;

        if (publicKeyBuffer == null)
        {
            var keyBlob = encodeType == EncodeType.Hex
                ? CryptographicBuffer.DecodeFromHexString(publicKey)
                : CryptographicBuffer.DecodeFromBase64String(publicKey);

            key = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString())
                .ImportPublicKey(keyBlob);
        }
        else
        {
            key = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString())
                .ImportPublicKey(publicKeyBuffer);
        }

        IBuffer encryptedData = CryptographicEngine.Encrypt(key, keyBuffer, null);

        return encodeType == EncodeType.Hex
            ? CryptographicBuffer.EncodeToHexString(encryptedData)
            : CryptographicBuffer.EncodeToBase64String(encryptedData);
    }

    public static string EncryptStringAsymmetric(string plainText, string publicKey)
    {
        return EncryptStringAsymmetricBase(plainText, publicKey, null, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair)
    {
        keyPair = GenerateAsymmetricKeyPair(AsymmetricAlgorithm.RSA_PKCS1);
        return EncryptStringAsymmetricBase(plainText, null, keyPair.ExportPublicKey(), AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, string publicKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return EncryptStringAsymmetricBase(plainText, publicKey, null, asymmetricAlgorithm, EncodeType.Hex);
    }
    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        keyPair = GenerateAsymmetricKeyPair(asymmetricAlgorithm);
        return EncryptStringAsymmetricBase(plainText, null, keyPair.ExportPublicKey(), asymmetricAlgorithm, EncodeType.Hex);
    }
    public static string EncryptStringAsymmetric(string plainText, string publicKey, EncodeType encodeType)
    {
        return EncryptStringAsymmetricBase(plainText, publicKey, null, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair, EncodeType encodeType)
    {
        keyPair = GenerateAsymmetricKeyPair(AsymmetricAlgorithm.RSA_PKCS1);
        return EncryptStringAsymmetricBase(plainText, null, keyPair.ExportPublicKey(), AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, string publicKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return EncryptStringAsymmetricBase(plainText, publicKey, null, asymmetricAlgorithm, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, out CryptographicKey keyPair, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        keyPair = GenerateAsymmetricKeyPair(asymmetricAlgorithm);
        return EncryptStringAsymmetricBase(plainText, null, keyPair.ExportPublicKey(), asymmetricAlgorithm, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey)
    {
        return EncryptStringAsymmetricBase(plainText, null, publicKey, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return EncryptStringAsymmetricBase(plainText, null, publicKey, asymmetricAlgorithm, EncodeType.Hex);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey, EncodeType encodeType)
    {
        return EncryptStringAsymmetricBase(plainText, null, publicKey, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string EncryptStringAsymmetric(string plainText, IBuffer publicKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return EncryptStringAsymmetricBase(plainText, null, publicKey, asymmetricAlgorithm, encodeType);
    }

    private static string DecryptStringAsymmetricBase(string encryptedString, string privateKey, IBuffer privateKeyBuffer, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        var keyBuffer = encodeType == EncodeType.Hex
            ? CryptographicBuffer.DecodeFromHexString(encryptedString)
            : CryptographicBuffer.DecodeFromBase64String(encryptedString);

        CryptographicKey key;
        if (privateKeyBuffer == null)
        {
            var keyBlob = encodeType == EncodeType.Hex
                ? CryptographicBuffer.DecodeFromHexString(privateKey)
                : CryptographicBuffer.DecodeFromBase64String(privateKey);

            key = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString())
                .ImportKeyPair(keyBlob);
        }
        else
        {
            key = AsymmetricKeyAlgorithmProvider.OpenAlgorithm(asymmetricAlgorithm.ToString())
                .ImportKeyPair(privateKeyBuffer);
        }

        IBuffer decryptedData = CryptographicEngine.Decrypt(key, keyBuffer, null);

        return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, decryptedData);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey)
    {
        return DecryptStringAsymmetricBase(encryptedString, null, privateKey, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return DecryptStringAsymmetricBase(encryptedString, null, privateKey, asymmetricAlgorithm, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey, EncodeType encodeType)
    {
        return DecryptStringAsymmetricBase(encryptedString, null, privateKey, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, IBuffer privateKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return DecryptStringAsymmetricBase(encryptedString, null, privateKey, asymmetricAlgorithm, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey)
    {
        return DecryptStringAsymmetricBase(encryptedString, privateKey, null, AsymmetricAlgorithm.RSA_PKCS1, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey, AsymmetricAlgorithm asymmetricAlgorithm)
    {
        return DecryptStringAsymmetricBase(encryptedString, privateKey, null, asymmetricAlgorithm, EncodeType.Hex);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey, EncodeType encodeType)
    {
        return DecryptStringAsymmetricBase(encryptedString, privateKey, null, AsymmetricAlgorithm.RSA_PKCS1, encodeType);
    }

    public static string DecryptStringAsymmetric(string encryptedString, string privateKey, AsymmetricAlgorithm asymmetricAlgorithm, EncodeType encodeType)
    {
        return DecryptStringAsymmetricBase(encryptedString, privateKey, null, asymmetricAlgorithm, encodeType);
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


    private static System.Security.Cryptography.SymmetricAlgorithm GetSymmetricAlgorithm(string aes_Key, string aes_IV, out string aes_KeyOut, out string aes_IVOut, EncodeType encodeType)
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
        var symmetricAlgorithm = GetSymmetricAlgorithm(aes_Key, aes_IV, out aes_KeyOut, out aes_IVOut, encodeType);

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
        var symmetricAlgorithm = GetSymmetricAlgorithm(aes_Key, aes_IV, out _, out _, encodeType);

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
