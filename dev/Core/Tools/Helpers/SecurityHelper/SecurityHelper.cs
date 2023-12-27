using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WinUICommunity;

public static partial class SecurityHelper
{
    public static string GetHash(string value, HashAlgorithm hashAlgorithm, EncodeType encodeType = EncodeType.Hex)
    {
        IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(value, BinaryStringEncoding.Utf8);

        HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(hashAlgorithm.ToString());

        IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

        if (buffHash.Length != objAlgProv.HashLength)
        {
            throw new Exception("There was an error creating the hash");
        }

        return encodeType == EncodeType.Hex
            ? CryptographicBuffer.EncodeToHexString(buffHash).ToUpper()
            : CryptographicBuffer.EncodeToBase64String(buffHash).ToUpper();
    }

    public static async Task<string> GetHashFromFileAsync(string filePath, HashAlgorithm hashAlgorithm, EncodeType encodeType = EncodeType.Hex)
    {
        var file = await FileHelper.GetStorageFile(filePath, PathType.Absolute);
        HashAlgorithmProvider alg = HashAlgorithmProvider.OpenAlgorithm(hashAlgorithm.ToString());
        var stream = await file.OpenStreamForReadAsync();
        var inputStream = stream.AsInputStream();

        uint capacity = 100000000;
        var buffer = new Windows.Storage.Streams.Buffer(capacity);
        var hash = alg.CreateHash();

        while (true)
        {
            IBuffer readBuffer = await inputStream.ReadAsync(buffer, capacity, InputStreamOptions.None);

            if (readBuffer.Length > 0)
                hash.Append(readBuffer);
            else
                break;
        }

        inputStream.Dispose();
        stream.Dispose();

        return encodeType == EncodeType.Hex
                    ? CryptographicBuffer.EncodeToHexString(hash.GetValueAndReset()).ToUpper()
                    : CryptographicBuffer.EncodeToBase64String(hash.GetValueAndReset()).ToUpper();
    }

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
