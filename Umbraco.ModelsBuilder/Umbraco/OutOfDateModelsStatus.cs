﻿using System;
using System.IO;
using System.Web.Hosting;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.ModelsBuilder.Configuration;
using Umbraco.Web.Cache;

namespace Umbraco.ModelsBuilder.Umbraco
{
    public sealed class OutOfDateModelsStatus
    {
        internal static void Install()
        {
            // just be sure
            if (UmbracoConfig.For.ModelsBuilder().FlagOutOfDateModels == false)
                return;

            ContentTypeCacheRefresher.CacheUpdated += (sender, args) => Write();
            DataTypeCacheRefresher.CacheUpdated += (sender, args) => Write();
        }

        private static string GetFlagPath()
        {
            var appData = HostingEnvironment.MapPath("~/App_Data");
            if (appData == null) throw new Exception("Panic: appData is null.");
            var modelsDirectory = Path.Combine(appData, "Models");
            if (!Directory.Exists(modelsDirectory))
                Directory.CreateDirectory(modelsDirectory);
            return Path.Combine(modelsDirectory, "ood.flag");
        }

        private static void Write()
        {
            var path = GetFlagPath();
            if (path == null || File.Exists(path)) return;
            File.WriteAllText(path, "THIS FILE INDICATES THAT MODELS ARE OUT-OF-DATE\n\n");
        }

        public static void Clear()
        {
            if (UmbracoConfig.For.ModelsBuilder().FlagOutOfDateModels == false) return;
            var path = GetFlagPath();
            if (path == null || !File.Exists(path)) return;
            File.Delete(path);
        }

        public static bool IsEnabled
        {
            get { return UmbracoConfig.For.ModelsBuilder().FlagOutOfDateModels; }
        }

        public static bool IsOutOfDate
        {
            get
            {
                if (UmbracoConfig.For.ModelsBuilder().FlagOutOfDateModels == false) return false;
                var path = GetFlagPath();
                return path != null && File.Exists(path);
            }
        }
    }
}
