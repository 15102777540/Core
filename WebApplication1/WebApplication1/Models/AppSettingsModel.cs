using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AppSettingsModel
    {
        public string WeixinToken { get; set; }

        public string WeixinAppID { get; set; }
    }

    public class ApplicationConfiguration {
        public string FileUpPath { get; set; }

        public string IsSingleLogin { get; set; }

        public string AttachExtension { get; set; }

        public string AttachImagesize { get; set; }   
    }

}
