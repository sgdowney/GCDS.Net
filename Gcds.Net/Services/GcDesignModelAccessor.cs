using Gcds.Net.Models;

namespace Gcds.Net.Services
{
    public class GcDesignModelAccessor : IGcDesignModelAccessor
    {
        public GcDesignModelAccessor(IConfiguration configuration)
        {
            GcModel = new GcDesignModel();
            configuration.GetSection("GcDesign").Bind(GcModel);
        }

        public IGcDesignModel GcModel { get; }
    }
}
