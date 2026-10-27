using Gcds.Net.Models;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gcds.Net.Extensions
{
    public static class ViewDataDictionaryExtension
    {
        public static IGcDesignModel GetIGcDesignModel(this ViewDataDictionary viewData, string key = nameof(GcDesignModel)) => (IGcDesignModel)viewData[key]!;

        public static GcDesignModel GetGcDesignModel(this ViewDataDictionary viewData, string key = nameof(GcDesignModel)) => (GcDesignModel)viewData[key]!;
    }
}
