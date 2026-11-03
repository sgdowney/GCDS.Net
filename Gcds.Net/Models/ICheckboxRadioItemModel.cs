namespace Gcds.Net.Models
{
    public interface ICheckboxRadioItemModel
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string? Hint { get; set; }    
        public string Value { get; set; }
    }
}
