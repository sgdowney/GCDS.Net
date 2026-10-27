namespace Gcds.Net.Models
{
    public class FooterModel : IFooterModel
    {
        public FooterDisplay Display { get; set; }
        public string? ContextualHeading { get; set; }
        public IDictionary<string, string> ContextualLinks { get; } = new Dictionary<string, string>();
        public IDictionary<string, string> SubLinks { get; } = new Dictionary<string, string>();
        public string? CssClass { get; set; }
    }
}
