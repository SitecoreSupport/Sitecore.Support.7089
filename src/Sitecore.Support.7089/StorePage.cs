using Sitecore.Links;
using Sitecore.XA.Feature.CreativeExchange.Pipelines.Export.PageProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Sitecore.Support.XA.Feature.CreativeExchange.Pipelines.Export.PageProcessing
{
    public class StorePage: PageProcessingBaseProcessor
    {
        protected virtual string BuildItemPath(PageProcessingArgs args)
        {
            string itemUrl = LinkManager.GetItemUrl(args.PageContext.ExportedPage.Item);
            #region added to fix 7089
            if ((LinkManager.LanguageEmbedding != LanguageEmbedding.Never) && (!itemUrl.StartsWith("/sitecore")))
            {
                int ind = itemUrl.IndexOf("/sitecore");
                itemUrl = itemUrl.Substring(ind);
            }
            #endregion
            string homeFullPath = args.PageContext.HomeFullPath;
            string str3 = (homeFullPath.Length >= 2) ? itemUrl.Substring(homeFullPath.Length - 2) : itemUrl;
            return string.Format("{0}/index.html", str3);
        }

        public override void Process(PageProcessingArgs args)
        {
            char[] trimChars = new char[] { '/' };
            string path = this.BuildItemPath(args).TrimStart(trimChars);
            args.CreativeExchangeExportStorage.Store(path, new MemoryStream(Encoding.UTF8.GetBytes(args.PageHtml)));
        }
    }
}