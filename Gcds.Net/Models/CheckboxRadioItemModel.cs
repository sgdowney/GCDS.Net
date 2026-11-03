namespace Gcds.Net.Models
{
    public class CheckboxRadioItemModel : ICheckboxRadioItemModel
    {
        public required string Id { get; set; }
        public required string Label { get; set; }
        public string? Hint { get; set; }
        public required string Value { get; set; }
    }
}
