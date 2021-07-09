using System;
using System.Collections.Generic;
using System.Net;

namespace Homo.Core.Constants
{
    public interface IAppSettings
    {
        Common Common { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public Common Common { get; set; }
    }

    public class Common
    {
        public string ErrorMappingPath { get; set; }
    }
}
