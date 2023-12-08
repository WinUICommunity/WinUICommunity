namespace WindowUI;

public class InIHelper
{
    internal string Path { get; set; }
    public InIHelper(string filePath)
    {
        Path = filePath;
    }

    private string GetAssemblyName()
    {
        return Application.Current.GetType().Assembly.GetName().Name;
    }

    /// <summary>
    /// Read Data Value From the Ini File
    /// </summary>
    /// <param name="Key">must be unique</param>
    /// <param name="Section">Optional</param>
    /// <param name="Path">default is: application startup folder location</param>
    /// <returns></returns>
    public string ReadValue(string Key, string Section = null, string Path = null)
    {
        var RetVal = new StringBuilder(255);
        NativeMethods.GetPrivateProfileString(Section ?? GetAssemblyName(), Key, "", RetVal, 255, Path ?? Path);
        return RetVal.ToString();
    }

    /// <summary>
    /// Write Data to the INI File
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    /// <param name="Section">Optional</param>
    /// <param name="Path">default is: application startup folder location</param>
    public void AddValue(string Key, string Value, string Section = null, string Path = null)
    {
        NativeMethods.WritePrivateProfileString(Section ?? GetAssemblyName(), Key, Value, Path ?? Path);
    }

    /// <summary>
    /// Delete Key from INI File
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Section">Optional</param>
    /// <param name="Path"></param>
    public void DeleteKey(string Key, string Section = null, string Path = null)
    {
        AddValue(Key, null, Section ?? GetAssemblyName(), Path ?? Path);
    }

    /// <summary>
    /// Delete Section from INI File
    /// </summary>
    /// <param name="Section"></param>
    /// <param name="Path"></param>
    public void DeleteSection(string Section = null, string Path = null)
    {
        AddValue(null, null, Section ?? GetAssemblyName(), Path ?? Path);
    }

    /// <summary>
    /// Check if Key Exist or Not in INI File
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Section">Optional</param>
    /// <param name="Path"></param>
    /// <returns></returns>
    public bool IsKeyExists(string Key, string Section = null, string Path = null)
    {
        return ReadValue(Key, Section, Path ?? Path).Length > 0;
    }
}
