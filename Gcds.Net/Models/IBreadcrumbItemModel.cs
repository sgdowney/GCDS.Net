namespace Gcds.Net.Models
{
    public interface IBreadcrumbItemModel
    {
        public string? Href { get; set; }
        public string? LinkText { get; set; }
        public string? CssClass { get; set; }
    }
}
