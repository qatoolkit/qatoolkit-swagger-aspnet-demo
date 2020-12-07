using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAToolKit.Swagger.AspNet.Demo.Models
{
    public class BicycleImage
    {
        public string FileName { get; set; }
        public IFormFile FileContent { get; set; }
    }
}
