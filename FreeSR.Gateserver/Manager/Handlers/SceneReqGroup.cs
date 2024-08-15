namespace FreeSR.Gateserver.Manager.Handlers
{
    using FreeSR.Gateserver.Manager.Handlers.Core;
    using FreeSR.Gateserver.Manager.Handlers.Data.Config;
    using FreeSR.Gateserver.Manager.Handlers.Data.Enums.Scene;
    using FreeSR.Gateserver.Manager.Handlers.Data;
    using FreeSR.Gateserver.Network;
    using FreeSR.Proto;

    internal static class SceneReqGroup
    {
        [Handler(CmdType.CmdGetCurSceneInfoCsReq)]
        public static void OnGetCurSceneInfoCsReq(NetSession session, int cmdId, object data)
        {
            SceneInfo scene = new SceneInfo
            {
                GameModeType = 1,
                EntryId = 2032101,
                PlaneId = 20321,
                FloorId = 20321001,
                //LeaderEntityId = 1,
            };

            session.Send(CmdType.CmdGetCurSceneInfoScRsp, new GetCurSceneInfoScRsp
            {
                Scene = scene,
                Retcode = 0
            });
        }

        [Handler(CmdType.CmdGetSceneMapInfoCsReq)]
        public static void OnGetSceneMapInfoCsReq(NetSession session, int cmdId, object data)
        {
            var req = data as GetSceneMapInfoCsReq;

            uint[] back = new uint[101];
            for (uint i = 0; i <= 100; i++)
            {
                back[i] = i;
            }

            var rsp = new GetSceneMapInfoScRsp();
            foreach (var entry in req.EntryIdLists)
            {
                var mazeMap = new SceneMapInfo()
                {
                    EntryId = entry,
                };
                GameData.MapEntranceData.TryGetValue((int)entry, out var mapData);
                if (mapData == null)
                {
                    rsp.MapLists.Add(mazeMap);
                    continue;
                }

                FloorInfo floorInfo;
                if (!GameData.GetFloorInfo(mapData.PlaneID, mapData.FloorID, out floorInfo))
                {
                    rsp.MapLists.Add(mazeMap);
                    continue;
                }

                mazeMap.UnlockedChestLists.Add(new MazeChest()
                {
                    MapInfoChestType = ChestType.MapInfoChestTypeNormal
                });

                mazeMap.UnlockedChestLists.Add(new MazeChest()
                {
                    MapInfoChestType = ChestType.MapInfoChestTypePuzzle
                });

                mazeMap.UnlockedChestLists.Add(new MazeChest()
                {
                    MapInfoChestType = ChestType.MapInfoChestTypeChallenge
                });

                foreach (GroupInfo groupInfo in floorInfo.Groups.Values)
                {
                    var mazeGroup = new MazeGroup()
                    {
                        GroupId = (uint)groupInfo.Id,
                    };
                    mazeMap.MazeGroupLists.Add(mazeGroup);
                }

                var unlockTeleportList = new List<uint>();
                foreach (var teleport in floorInfo.CachedTeleports.Values)
                {
                    unlockTeleportList.Add((uint)teleport.MappingInfoID);
                }
                mazeMap.UnlockedTeleportLists = unlockTeleportList.ToArray();

                foreach (var prop in floorInfo.UnlockedCheckpoints)
                {
                    var mazeProp = new MazeProp()
                    {
                        GroupId = (uint)prop.AnchorGroupID,
                        ConfigId = (uint)prop.ID,
                        State = (uint)PropStateEnum.CheckPointEnable,
                    };
                    mazeMap.MazePropLists.Add(mazeProp);
                }

                mazeMap.LightenSectionLists = back;
                

                rsp.MapLists.Add(mazeMap);
            }
            //rsp.Mfdibeeclpp = true;
            rsp.Retcode = 0;
            session.Send(CmdType.CmdGetSceneMapInfoScRsp, rsp);
        }

        /*[Handler(CmdType.CmdEnterSceneCsReq)]
        public static void OnEnterSceneCsReq(NetSession session, int cmdId, object data)
        {
            var req = data as EnterSceneCsReq;
            int id1 = (int)req.EntryId;
            int id2 = (int)req.TeleportId;
            LoadScene(session, id1,id2);
            var rsp = new EnterSceneScRsp { Retcode =0};
            session.Send(CmdType.CmdEnterSceneScRsp, rsp);
        }*/

        /*public static void LoadScene(NetSession session, int entryId, int teleportId)
        {
            if (!GameData.MapEntranceData.TryGetValue(entryId, out var entrance))
            {
                Console.WriteLine("Map Entrance Not Found");
                return;
            }

            if (!GameData.GetFloorInfo(entrance.PlaneID, entrance.FloorID, out var floorInfo))
            {
                Console.WriteLine("Map Plane Not Found");
                return;
            }

            int startGroup = entrance.StartGroupID;
            int startAnchor = entrance.StartAnchorID;

            if (teleportId != 0)
            {
                if (floorInfo.CachedTeleports.TryGetValue(teleportId, out var teleport))
                {
                    startGroup = teleport.AnchorGroupID;
                    startAnchor = teleport.AnchorID;
                }
            }
            else if (startAnchor == 0)
            {
                startGroup = floorInfo.StartGroupID;
                startAnchor = floorInfo.StartAnchorID;
            }

            AnchorInfo? anchor = floorInfo.getStartAnchorInfo();
            if (anchor == null)
            {
                Console.WriteLine("Anchor Not Found");
                return;
            }
            Position pos = anchor!.ToPositionProto();
            Position rot = anchor!.ToRotationProto();
            var lineupInfo = new LineupInfo
            {
                ExtraLineupType = ExtraLineupType.LineupNone,
                Name = "Squad 1",
                Mp = 5,
                MaxMp = 5,
                LeaderSlot = 0
            };

            List<uint> characters = new List<uint> { Avatar1, Avatar2, Avatar3, Avatar4 };
            foreach (uint id in characters)
            {
                if (id == 0) continue;
                lineupInfo.AvatarLists.Add(new LineupAvatar
                {
                    Id = id,
                    Hp = 10000,
                    Satiety = 100,
                    SpBar = new SpBarInfo { CurAmount = 10000, MaxAmount = 10000 },
                    AvatarType = AvatarType.AvatarFormalType,
                    Slot = (uint)lineupInfo.AvatarLists.Count
                });
            }

            var sceneInfo = new SceneInfo
            {
                PlaneId = (uint)entrance.PlaneID,
                FloorId = (uint)entrance.FloorID,
                EntryId = (uint)entryId,
            };

            var playerGroup = new SceneGroupInfo
            {
                State = 0,
                GroupId = 0,
            };

            foreach (var avatar in lineupInfo.AvatarLists)
            {
                var playerEntity = new SceneEntityInfo
                {
                    InstId = 0,
                    EntityId = avatar.Slot + 1,
                    Motion = new MotionInfo
                    {
                        Pos = new Proto.Vector
                        {
                            X = pos.X,
                            Y = pos.Y,
                            Z = pos.Z
                        },
                        Rot = new Proto.Vector
                        {
                            X = 0,
                            Y = rot.Y,
                            Z = 0
                        }
                    },
                    Actor = new SceneActorInfo
                    {
                        AvatarType = AvatarType.AvatarFormalType,
                        BaseAvatarId = avatar.Id,
                        MapLayer = 0,
                        Uid = 0
                    }
                };
                playerGroup.EntityLists.Add(playerEntity);
            }

            sceneInfo.EntityGroupLists.Add(playerGroup);

            Console.WriteLine("Sending scene information to client...");

            session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
            {
                Scene = sceneInfo,
                Lineup = lineupInfo
            });

            Console.WriteLine("Sending player position information to client...");

            session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
            {
                EntryId = (uint)entryId,
                EntityId = 0,
                Motion = new MotionInfo
                {
                    Pos = new Proto.Vector
                    {
                        X = pos.X,
                        Y = pos.Y,
                        Z = pos.Z
                    },
                    Rot = new Proto.Vector
                    {
                        X = 0,
                        Y = rot.Y,
                        Z = 0
                    }
                }
            });

            Console.WriteLine("Scene and player information sent successfully.");
        }*/

    }
}
