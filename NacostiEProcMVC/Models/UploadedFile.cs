using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NacostiEProcMVC.Models
{
    public class UploadedFile
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}