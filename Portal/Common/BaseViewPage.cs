
using System.Web.Mvc;
using Portal.Identity.Models;


namespace Portal.Common
{
    public abstract class BaseViewPage : WebViewPage
    {
        public virtual new ICloudPrincipal User
        {
            get { return (ICloudPrincipal)base.User; }
        }
    }
    public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public virtual new ICloudPrincipal User
        {
            get { return (ICloudPrincipal)base.User; }
        }
    }
}