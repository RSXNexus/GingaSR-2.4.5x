using FreeSR.Gateserver.Manager.Handlers.Data.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FreeSR.Gateserver.Manager.Handlers.Data
{
    public class ResourceManager
    {
        public static Logger Logger { get; private set; } = new Logger("ResourceManager");
        public static void LoadGameData()
        {
            LoadExcel();
            LoadFloorInfo();
        }

        public static void LoadExcel()
        {

            var classes = Assembly.GetExecutingAssembly().GetTypes();  // Get all classes in the assembly
            var resList = new List<ExcelResource>();
            foreach (var cls in classes)
            {
                var attribute = (ResourceEntity)Attribute.GetCustomAttribute(cls, typeof(ResourceEntity))!;

                if (attribute != null)
                {
                    var resource = (ExcelResource)Activator.CreateInstance(cls)!;
                    var count = 0;
                    foreach (var fileName in attribute.FileName)
                    {
                        try
                        {
                            var path = "Resources/ExcelOutput/" + fileName;
                            var file = new FileInfo(path);
                            if (!file.Exists)
                            {
                                Logger.Warn($"File {path} not found");
                                continue;
                            }
                            var json = file.OpenText().ReadToEnd();
                            using (var reader = new JsonTextReader(new StringReader(json)))
                            {
                                reader.Read();
                                if (reader.TokenType == JsonToken.StartArray)
                                {
                                    // array
                                    var jArray = JArray.Parse(json);
                                    foreach (var item in jArray)
                                    {
                                        var res = JsonConvert.DeserializeObject(item.ToString(), cls);
                                        resList.Add((ExcelResource)res!);
                                        ((ExcelResource?)res)?.Loaded();
                                        count++;
                                    }
                                }
                                else if (reader.TokenType == JsonToken.StartObject)
                                {
                                    // dictionary
                                    var jObject = JObject.Parse(json);
                                    foreach (var item in jObject)
                                    {
                                        var id = int.Parse(item.Key);
                                        var obj = item.Value;
                                        var instance = JsonConvert.DeserializeObject(obj!.ToString(), cls);

                                        if (((ExcelResource?)instance)?.GetId() == 0 || ((ExcelResource?)instance) == null)
                                        {
                                            // Deserialize as JObject to handle nested dictionaries
                                            var nestedObject = JsonConvert.DeserializeObject<JObject>(obj.ToString());

                                            foreach (var nestedItem in nestedObject ?? new JObject())
                                            {
                                                var nestedInstance = JsonConvert.DeserializeObject(nestedItem.Value!.ToString(), cls);
                                                resList.Add((ExcelResource)nestedInstance!);
                                                ((ExcelResource?)nestedInstance)?.Loaded();
                                                count++;
                                            }
                                        }
                                        else
                                        {
                                            resList.Add((ExcelResource)instance!);
                                            ((ExcelResource)instance).Loaded();
                                        }
                                        count++;

                                    }
                                }
                            }
                            resource.Finalized();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Error in reading {fileName}", ex);
                        }
                    }
                    Logger.Info($"Loaded {count} {cls.Name}s.");
                }
            }
            foreach (var cls in resList)
            {
                cls.AfterAllDone();
            }
        }

        public static void LoadFloorInfo()
        {
            Logger.Info("Loading floor files...");
            DirectoryInfo directory = new("Resources/Config/LevelOutput/RuntimeFloor/");
            bool missingGroupInfos = false;

            if (!directory.Exists)
            {
                Logger.Warn($"Floor infos are missing, please check your resources folder: Resources/Config/LevelOutput/RuntimeFloor. Teleports and natural world spawns may not work!");
                return;
            }
            // Load floor infos
            foreach (FileInfo file in directory.GetFiles())
            {
                try
                {
                    using var reader = file.OpenRead();
                    using StreamReader reader2 = new(reader);
                    var text = reader2.ReadToEnd();
                    var info = JsonConvert.DeserializeObject<FloorInfo>(text);
                    var name = file.Name[..file.Name.IndexOf('.')];
                    GameData.FloorInfoData.Add(name, info!);
                } catch (Exception ex)
                {
                    Logger.Error("Error in reading" + file.Name, ex);
                }
            }

            foreach (var info in GameData.FloorInfoData.Values)
            {
                foreach (var groupInfo in info.GroupInstanceList)
                {
                    if (groupInfo.IsDelete) { continue; }
                    FileInfo file = new("Resources/" + groupInfo.GroupPath);
                    if (!file.Exists) continue;
                    try
                    {
                        using var reader = file.OpenRead();
                        using StreamReader reader2 = new(reader);
                        var text = reader2.ReadToEnd();
                        GroupInfo? group = JsonConvert.DeserializeObject<GroupInfo>(text);
                        if (group != null)
                        {
                            group.Id = groupInfo.ID;
                            if (!info.Groups.ContainsKey(groupInfo.ID))
                            {
                                info.Groups.Add(groupInfo.ID, group);
                                group.Load();
                            }
                        }
                    } catch (Exception ex)
                    {
                        Logger.Error("Error in reading " + file.Name, ex);
                    }
                    if (info.Groups.Count == 0)
                    {
                        missingGroupInfos = true;
                    }
                }
                info.OnLoad();
            }
            if (missingGroupInfos)
                Logger.Warn($"Group infos are missing, please check your resources folder: Resources/Config/LevelOutput/Group. Teleports, monster battles, and natural world spawns may not work!");

            Logger.Info("Loaded " + GameData.FloorInfoData.Count + " floor infos.");
        }
    }
}
