using BeHealthy.Models.Enums;

namespace BeHealthy.Models;

public class FieldDefinition
{

    public required string Name { get; set; }
    public required string DisplayName { get; set; }
    public FieldType Type { get; set; }
    public bool IsRequired { get; set; }
    public IEnumerable<SelectItem> Options { get; set; } = new List<SelectItem>();
}
