using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSR.Dispatch.Configuration
{
    internal class HotfixConfiguration
    {
        public string assetBundleUrl { get; set; }
        public string exResourceUrl { get; set; }
        public string luaUrl { get; set; }
        public string ifixUrl { get; set; }
        public ushort customMdkResVersion { get; set; }
        public ushort customIfixVersion { get; set; }
    }
}
