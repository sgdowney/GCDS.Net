namespace Gcds.Net.Models
{
    public interface IGcDesignModel
    {
        public string CssShortcutVersion { get; set; }
        public string GcVersion { get; set; }
        //public bool ShowSearch { get; set; }
        //public bool ShowBreadcrumbs { get; set; }
        //public IList<IBreadcrumbItemModel> Breadcrumbs { get; }
        public IHeaderModel Header { get; }
        public IFooterModel Footer { get; }
    }
}
