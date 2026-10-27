namespace Gcds.Net.Models
{
    public class GcDesignModel : IGcDesignModel
    {
        //public GcDesignModel()
        //{
        //    Breadcrumbs = [];
        //}
        public string CssShortcutVersion { get; set; } = string.Empty;

        public string GcVersion { get; set; } = string.Empty;

        //public bool ShowSearch { get; set; }
        //public bool ShowBreadcrumbs { get; set; }

        //public IList<IBreadcrumbItemModel> Breadcrumbs { get; } = [];

        public IHeaderModel Header { get; } = new HeaderModel();

        public IFooterModel Footer { get; } = new FooterModel();
    }
}
