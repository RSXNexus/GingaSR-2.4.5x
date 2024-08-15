namespace FreeSR.Dispatch.Handlers
{
    using Ceen;
    using FreeSR.Dispatch.Configuration;
    using FreeSR.Dispatch.Util;
    using FreeSR.Proto;
    using NLog.Fluent;
    using ProtoBuf.WellKnownTypes;
    using System;
    using NLog;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class QueryGatewayHandler : IHttpModule
    {
        private static HotfixConfiguration h_config;
        private static readonly Logger s_log = LogManager.GetCurrentClassLogger();

        public static void Initialize(HotfixConfiguration configuration)
        {
            h_config = configuration;
        }

        public async Task<bool> HandleAsync(IHttpContext context)
        {
            // 获取 Lua 版本号
            string luaVersion = GetLuaVersionFromUrl(h_config.luaUrl);

            if (string.IsNullOrEmpty(luaVersion))
            {
                luaVersion = "0";
            }
            s_log.Info($"query_gateway: luaversion: {luaVersion}");
            context.Response.StatusCode = HttpStatusCode.OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAllAsync(Convert.ToBase64String(ProtobufUtil.Serialize(new Gateserver
            {
                Retcode = 0,
                Msg = "OK",
                Ip = "127.0.0.1",
                Port = 23301,
                RegionName ="GingaSR",
                B1 = true,
                B2 = true,
                B3 = true,
                B6 = true,
                B7 = true,
                B8 = true,
                B9 = true,
                B5 = true,
                AssetBundleUrl = h_config.assetBundleUrl,
                ExResourceUrl = h_config.exResourceUrl,
                IfixVersion = "0",
                LuaUrl = h_config.luaUrl,
                LuaVersion = luaVersion,
            })));
            return true;
        }

        private static string GetLuaVersionFromUrl(string url)
        {
            try
            {
                Regex regex = new Regex(@"output_(\d+)_\w+");
                Match match = regex.Match(url);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting Lua version: {ex.Message}");
            }
            return null;
        }
    }
}
