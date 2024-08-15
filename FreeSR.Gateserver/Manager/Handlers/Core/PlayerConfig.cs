using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeSR.Gateserver.Manager.Handlers.Core
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    public class PlayerConfig
    {
        public class Player
        {
            public uint Uid { get; set; }
            public string Nickname { get; set; }
            public uint Level { get; set; }
            public uint Exp { get; set; }
            public uint Stamina { get; set; }
            public uint Mcoin { get; set; }
            public uint Hcoin { get; set; }
            public uint Scoin { get; set; }
            public uint WorldLevel { get; set; }
            public List<uint> Avatarlist { get; set; }
        }

        public class Position
        {
            public uint x { get; set; }
            public uint y { get; set; }
            public uint z { get; set; }
            public uint rot_y { get; set; }
        }

        public class Scene
        {
            public uint plane_id { get; set; }
            public uint floor_id { get; set; }
            public uint entry_id { get; set; }
        }

        public class RootObject
        {
            public Player Player { get; set; }
            public Position position { get; set; }
            public Scene scene { get; set; }
        }
        public static RootObject ReadPlayerConfig()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlayerConfig.json");
            string jsonString = File.ReadAllText(jsonFilePath);
            return JsonConvert.DeserializeObject<RootObject>(jsonString);
        }
        public static void SavePlayerConfig(RootObject playerData)
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdatedPlayerConfig.json"); // 保存到另一个文件  
            string jsonString = JsonConvert.SerializeObject(playerData, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonString);
        }
    }
}
