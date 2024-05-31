using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.DTO
{
    internal class PackageDTO
    {
        public bool Result {  get; set; }
        public string Message {  get; set; }
        public object Data {  get; set; }

        public PackageDTO() { }
    }
}
