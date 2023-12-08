namespace WindowUI;

public class DataLink
{
    public DataLink(string title, string uri)
    {
        Title = title;
        Uri = uri;
    }
    public string Title { get; set; }
    public string Uri { get; set; }
}
