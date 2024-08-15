using FreeSR.Gateserver.Manager.Handlers.Data.Config;
using FreeSR.Gateserver.Manager.Handlers.Data.Excel;

namespace FreeSR.Gateserver.Manager.Handlers.Data
{
    public static class GameData
    {

        #region Maze
        public static Dictionary<string, FloorInfo> FloorInfoData { get; private set; } = new Dictionary<string, FloorInfo>();
        public static Dictionary<int, MapEntranceExcel> MapEntranceData { get; private set; } = new Dictionary<int, MapEntranceExcel>();

        public static Dictionary<int, MazePlaneExcel> MazePlaneData { get; private set; } = new Dictionary<int, MazePlaneExcel>();



        #endregion

        #region Actions

        public static bool GetFloorInfo(int planeId, int floorId, out FloorInfo floorInfo)
        {
            string key = $"P{planeId}_F{floorId}";
            if (FloorInfoData.TryGetValue(key, out floorInfo))
            {
                return true;
            }
            else
            {
                floorInfo = null;
                return false;
            }
        }

        public static int GetMinPromotionForLevel(int level)
        {
            return Math.Max(Math.Min((int)((level - 11) / 10D), 6), 0);
        }

        #endregion
    }
}
