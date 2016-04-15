﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Umbraco.ModelsBuilder.Umbraco
{
    internal static class ModelsGenerationError
    {
        public static void Clear()
        {
            var errFile = GetErrFile();
            if (errFile == null) return;

            // "If the file to be deleted does not exist, no exception is thrown."
            File.Delete(errFile);
        }

        public static void Report(string message, Exception e)
        {
            var errFile = GetErrFile();
            if (errFile == null) return;

            var sb = new StringBuilder();
            sb.Append(message);
            sb.Append("\r\n");
            sb.Append(e.Message);
            sb.Append("\r\n\r\n");
            sb.Append(e.StackTrace);
            sb.Append("\r\n");

            File.WriteAllText(errFile, sb.ToString());
        }

        public static string GetLastError()
        {
            var errFile = GetErrFile();
            if (errFile == null) return null;

            try
            {
                return File.ReadAllText(errFile);
            }
            catch // accepted
            {
                return null;
            }
        }

        private static string GetErrFile()
        {
            var appData = HostingEnvironment.MapPath("~/App_Data");
            if (appData == null)
                return null;

            var modelsDirectory = Path.Combine(appData, "Models");
            if (!Directory.Exists(modelsDirectory))
                return null;

            return Path.Combine(modelsDirectory, "models.err");
        }
    }
}
