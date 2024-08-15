using FreeSR.Gateserver.Manager.Handlers.Data.Enums.Scene;
using FreeSR.Gateserver.Manager.Handlers.Data;
using Newtonsoft.Json;

namespace FreeSR.Gateserver.Manager.Handlers.Data.Config
{
    public class FloorInfo
    {
        public int FloorID { get; set; }
        public int StartGroupIndex { get; set; }
        public int StartGroupID { get; set; }
        public int StartAnchorID { get; set; }

        public List<FloorGroupInfo> GroupInstanceList { get; set; } = new List<FloorGroupInfo>();

        public List<ExtraDataInfo> SavedValues { get; set; } = new List<ExtraDataInfo>();
        [JsonIgnore]
        public bool Loaded = false;

        [JsonIgnore]
        public List<GroupInfo> groupList { get; set; } = new List<GroupInfo>();

        [JsonIgnore]
        public Dictionary<int, GroupInfo> Groups = new Dictionary<int, GroupInfo>();

        [JsonIgnore]
        public Dictionary<int, PropInfo> CachedTeleports = new Dictionary<int, PropInfo>();

        [JsonIgnore]
        public List<PropInfo> UnlockedCheckpoints = new List<PropInfo>();

        public GroupInfo GetGroupInfoByIndex(int groupIndex)
        {
            return groupList[groupIndex];
        }
        public List<ExtraDataInfo> getExtraDatas()
        {
            if (SavedValues == null)
            {
                SavedValues = new List<ExtraDataInfo>();
            }

            return SavedValues;
        }

        public AnchorInfo getStartAnchorInfo()
        {
            GroupInfo group = this.GetGroupInfoByIndex(StartGroupIndex);
            if (group == null) return null;

            return getAnchorInfo(group, StartAnchorID);
        }

        public AnchorInfo? GetAnchorInfo(int groupId, int anchorId)
        {
            Groups.TryGetValue(groupId, out GroupInfo? group);
            if (group == null) return null;
            return getAnchorInfo(group, anchorId);
        }

        public AnchorInfo? getAnchorInfo(GroupInfo group, int anchorId)
        {
            return group.AnchorList.Find(info => info.ID == anchorId );
        }

        public void OnLoad()
        {
            if (Loaded) return;
            SavedValues = getExtraDatas();
            // Cache anchors
            foreach (var group in Groups.Values)
            {
                foreach (var prop in group.PropList)
                {
                    // Check if prop can be teleported to
                    if (prop.AnchorID > 0)
                    {
                        // Put inside cached teleport list to send to client when they request map info
                        CachedTeleports.TryAdd(prop.MappingInfoID, prop);
                        UnlockedCheckpoints.Add(prop);

                        // Force prop to be in the unlocked state
                        prop.State = PropStateEnum.CheckPointEnable;
                    }
                    else if (!string.IsNullOrEmpty(prop.InitLevelGraph))
                    {
                        string json = prop.InitLevelGraph;

                        // Hacky way to setup prop triggers
                        if (json.Contains("Maze_GroupProp_OpenTreasure_WhenMonsterDie"))
                        {
                            //prop.Trigger = new TriggerOpenTreasureWhenMonsterDie(group.Id);
                        }
                        else if (json.Contains("Common_Console"))
                        {
                            //prop.CommonConsole = true;
                        }

                        // Clear for garbage collection
                        prop.ValueSource = null;
                        prop.InitLevelGraph = null;
                    }
                }
            }

            Loaded = true;
        }

    }
    public class FloorGroupInfo
    {
        public string GroupPath { get; set; } = "";
        public bool IsDelete { get; set; }
        public int ID { get; set; }
    }
    public class ExtraDataInfo
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public int DefaultValue { get; set; }
        public List<int> AllowedValues { get; set; } = new List<int>();
        public int MaxValue { get; set; }
        public int? MinValue { get; set; }
    }
}
