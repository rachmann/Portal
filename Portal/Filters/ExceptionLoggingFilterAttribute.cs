using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Portal.Common;


namespace Portal.Filters
{
    public class ExceptionLoggingFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
            base.OnException(filterContext);
        }

        public void LogError(ExceptionContext filterContext)
        {
            // You could use any logging approach here

            var builder = new StringBuilder();
            builder
                .AppendLine(new String('=', 79))
                .AppendLine(DateTime.Now.ToString(ApplicationConstants.format_DateTime))
                .AppendFormat("Source:\t{0}", filterContext.Exception.GetBaseException().Source)
                .AppendLine()
                .AppendFormat("Target:\t{0}", filterContext.Exception.GetBaseException().TargetSite)
                .AppendLine()
                .AppendFormat("Type:\t{0}", filterContext.Exception.GetBaseException().GetType().Name)
                .AppendLine()
                .AppendFormat("Message:\t{0}", filterContext.Exception.GetBaseException().Message)
                .AppendLine()
                .AppendFormat("Stack:\t{0}", filterContext.Exception.GetBaseException().StackTrace)
                .AppendLine();

            var filePath = filterContext.HttpContext.Server.MapPath(string.Format("~/Errors/Error-{0}-{1}.log", DateTime.Now.Year, DateTime.Now.Month));

            using (var writer = File.AppendText(filePath))
            {
                writer.Write(builder.ToString());
                writer.Flush();
            }

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult { ViewName = "Error" };
            filterContext.Controller.ViewBag.Message = "An error has occured. IT Support should be notified. ";
        }

        public static void LogMessage(string message, LogLevel logging)
        {
            // You could use any logging approach here
            string configLevel = ConfigurationManager.AppSettings["LoggingLevel"] ?? "Normal" ;

            LogLevel configLogLevel = LogLevel.Normal;
            switch (configLevel.ToLower())
            {
                case "off":
                    configLogLevel = LogLevel.Off;
                    break;
                case "debug":
                    configLogLevel = LogLevel.Debug;
                    break;
                case "verbose":
                    configLogLevel = LogLevel.Verbose;
                    break;
            }
            if (logging != LogLevel.Off && logging <= configLogLevel)
            {
                var builder = new StringBuilder();
                builder
                    .AppendLine(new String('=', 79))
                    .AppendLine(DateTime.Now.ToString(ApplicationConstants.format_DateTime))
                    .AppendLine()
                    .AppendFormat("Message:\t{0}", message)
                    .AppendLine();

                var filePath = HostingEnvironment.MapPath(string.Format("~/Errors/Error-{0}-{1}.log", DateTime.Now.Year, DateTime.Now.Month));

                using (var writer = File.AppendText(filePath))
                {
                    writer.Write(builder.ToString());
                    writer.Flush();
                }
            }
        }

    }
}
