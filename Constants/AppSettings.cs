using System;
using System.Collections.Generic;
using System.Net;

namespace Homo.Core.Constants
{
    public class AppSettings
    {
        public Common Common { get; set; }
    }

    public class Common
    {
        public string ErrorMappingPath { get; set; }
    }
}
