namespace BeHealthy.Models;

public class TabItem<TTabKey>
{
    public string Title { get; set; } = string.Empty;
    public TTabKey Key { get; set; } = default!;
}
