namespace Gcds.Net.Models
{
    public enum FooterDisplay
    {
        full = 0,
        compact = 1
    }
    public interface IFooterModel
    {
        public FooterDisplay Display { get; set; }
        public string? ContextualHeading { get; set; }
        public IDictionary<string, string> ContextualLinks { get; }
        public IDictionary<string, string> SubLinks { get; }
        public string? CssClass { get; set; }
    }
}
