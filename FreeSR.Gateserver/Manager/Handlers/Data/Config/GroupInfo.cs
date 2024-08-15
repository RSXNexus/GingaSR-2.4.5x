using FreeSR.Gateserver.Manager.Handlers.Data.Enums;
using FreeSR.Gateserver.Manager.Handlers.Data.Enums.Scene;
using FreeSR.Gateserver.Manager.Handlers.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using static System.Formats.Asn1.AsnWriter;

namespace FreeSR.Gateserver.Manager.Handlers.Data.Config
{
    public class GroupInfo
    {
        public int Id;
        [JsonConverter(typeof(StringEnumConverter))]
        public GroupLoadSideEnum LoadSide { get; set; }
        public bool LoadOnInitial { get; set; }
        public string GroupName { get; set; } = "";

        public int OwnerMainMissionID { get; set; }
        public List<AnchorInfo> AnchorList { get; set; } = new List<AnchorInfo>();
        public List<MonsterInfo> MonsterList { get; set; } = new List<MonsterInfo>();
        public List<PropInfo> PropList { get; set; } = new List<PropInfo>();
        public List<NpcInfo> NPCList { get; set; } = new List<NpcInfo>();

        public void Load()
        {
            foreach (var prop in PropList)
            {
                prop.Load(this);
            }
        }
    }
}
