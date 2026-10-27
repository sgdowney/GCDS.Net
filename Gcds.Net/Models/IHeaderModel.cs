namespace Gcds.Net.Models
{
    public interface IHeaderModel
    {
        public bool ShowSearch { get; set; }
        public bool ShowBreadcrumbs { get; set; }
        public string? LanguageHref { get; set; }
        public string? SkipToHref { get; set; }
        public string? CssClass { get; set; }
        public IList<IBreadcrumbItemModel> Breadcrumbs { get; }
        public string? PageTitle { get; set; }
    }
}
