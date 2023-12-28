using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Security.Cryptography;

namespace WinUICommunity;

public static partial class SecurityHelper
{
    public static string GetHash(string value, HashAlgorithm hashAlgorithm, EncodeType encodeType = EncodeType.Hex)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        byte[] result = null;
        switch (hashAlgorithm)
        {
            case HashAlgorithm.MD5:
                result = MD5.HashData(bytes);
                break;
            case HashAlgorithm.SHA1:
                result = SHA1.HashData(bytes);
                break;
            case HashAlgorithm.SHA256:
                result = SHA256.HashData(bytes);
                break;
            case HashAlgorithm.SHA384:
                result = SHA384.HashData(bytes);
                break;
            case HashAlgorithm.SHA512:
                result = SHA512.HashData(bytes);
                break;
        }
        return encodeType == EncodeType.Hex
            ? Convert.ToHexString(result).ToUpper()
            : Convert.ToBase64String(result).ToUpper();
    }

#if NET7_0_OR_GREATER

    public static async Task<string> GetHashFromFileAsync(string filePath, HashAlgorithm hashAlgorithm, EncodeType encodeType = EncodeType.Hex)
    {
        var file = await FileHelper.GetStorageFile(filePath, PathType.Absolute);
        var stream = await file.OpenStreamForReadAsync();

        byte[] result = null;
        switch (hashAlgorithm)
        {
            case HashAlgorithm.MD5:
                result = await MD5.HashDataAsync(stream);
                break;
            case HashAlgorithm.SHA1:
                result = await SHA1.HashDataAsync(stream);
                break;
            case HashAlgorithm.SHA256:
                result = await SHA256.HashDataAsync(stream);
                break;
            case HashAlgorithm.SHA384:
                result = await SHA384.HashDataAsync(stream);
                break;
            case HashAlgorithm.SHA512:
                result = await SHA512.HashDataAsync(stream);
                break;
        }
        return encodeType == EncodeType.Hex
            ? Convert.ToHexString(result).ToUpper()
            : Convert.ToBase64String(result).ToUpper();
    }
#endif

    public static string EncryptBase64(string input)
    {
        var btArray = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(btArray, 0, btArray.Length);
    }

    public static string DecryptBase64(string encryptedString)
    {
        var btArray = Convert.FromBase64String(encryptedString);
        return Encoding.UTF8.GetString(btArray);
    }


    [Obsolete("Use new GenerateHash method")]
    public static string GetMD5Hash(string strMsg)
    {
        string strAlgName = HashAlgorithmNames.Md5;
        IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(strMsg, BinaryStringEncoding.Utf8);

        HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

        IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

        if (buffHash.Length != objAlgProv.HashLength)
        {
            throw new Exception("There was an error creating the hash");
        }

        string hex = CryptographicBuffer.EncodeToHexString(buffHash);

        return hex;
    }

    [Obsolete("Use new GenerateHash method")]
    public static string GetSHA256Hash(string strMsg)
    {
        string strAlgName = HashAlgorithmNames.Sha256;
        IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(strMsg, BinaryStringEncoding.Utf8);

        HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);

        IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

        if (buffHash.Length != objAlgProv.HashLength)
        {
            throw new Exception("There was an error creating the hash");
        }

        string hex = CryptographicBuffer.EncodeToHexString(buffHash);

        return hex;
    }
}
