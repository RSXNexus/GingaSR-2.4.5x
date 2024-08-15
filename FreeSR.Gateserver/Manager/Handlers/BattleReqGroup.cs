using FreeSR.Gateserver.Manager.Handlers.Core;
using static FreeSR.Gateserver.Manager.Handlers.LineupReqGroup;
using FreeSR.Gateserver.Network;
using FreeSR.Proto;

namespace FreeSR.Gateserver.Manager.Handlers
{
    internal static class BattleReqGroup
    {
        [Handler(CmdType.CmdSetLineupNameCsReq)]
        public static void OnSetLineupNameCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as SetLineupNameCsReq;
            if(request.Name == "battle")
            {
                
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
                        SpBar = new SpBarInfo { CurAmount = 10000,MaxAmount = 10000},
                        AvatarType = AvatarType.AvatarFormalType,
                        Slot = (uint)lineupInfo.AvatarLists.Count
                    });
                }

                var sceneInfo = new SceneInfo
                {
                    GameModeType = 2,
                    EntryId = 2031301,
                    PlaneId = 20313,
                    FloorId = 20313001
                };

                var calyxPenacony = new SceneEntityGroupInfo
                {
                    State = 0,
                    GroupId = 186
                };

                // flower
                calyxPenacony.EntityLists.Add(new SceneEntityInfo
                {
                    InstId = 300001,
                    EntityId = 328,
                    GroupId = 186,
                    Prop = new ScenePropInfo { CreateTimeMs = 0, PropState = 8, LifeTimeMs = 0, PropId = 808 },
                    Motion = new MotionInfo
                    {
                        Pos = new Vector
                        {
                            X = 31440,
                            Y = 192820,
                            Z = 433790
                        },
                        Rot = new Vector
                        {
                            X = 0,
                            Y = 60000,
                            Z = 0
                        }
                    },
                });

                sceneInfo.EntityGroupLists.Add(calyxPenacony);

                session.Send(CmdType.CmdEnterSceneByServerScNotify, new EnterSceneByServerScNotify
                {
                    Scene = sceneInfo,
                    Lineup = lineupInfo
                });

                session.Send(CmdType.CmdSceneEntityMoveScNotify, new SceneEntityMoveScNotify
                {
                    EntryId = 2031301,
                    EntityId = 0,
                    Motion = new MotionInfo
                    {
                        Pos = new Vector
                        {
                            X = 32342,
                            Y = 192820,
                            Z = 434276
                        },
                        Rot = new Vector
                        {
                            Y = 240000
                        }
                    }
                });
            }

            session.Send(CmdType.CmdSetLineupNameScRsp, new SetLineupNameScRsp
            {
                Retcode = 0,
                Name = request.Name,
                Index = request.Index
            });
        }


        [Handler(CmdType.CmdStartCocoonStageCsReq)]
        public static void OnStartCocoonStageCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as StartCocoonStageCsReq;

            Dictionary<uint, List<uint>> monsterIds = new Dictionary<uint, List<uint>>
            {
                { 1, new List<uint> { 3013010, 3012010, 3013010, 3001010 } },
                { 2, new List<uint> { 8034010 } },
                { 3, new List<uint> { 3014022 } },
            };

            Dictionary<uint, uint> monsterLevels = new Dictionary<uint, uint>
            {
                {1,70},{2,70},{3,60}
            };

            //basic
            var battleInfo = new SceneBattleInfo
            {
                StageId = 201012311,
                LogicRandomSeed = 639771447,
                WorldLevel = 6
            };

            var testRelic = new BattleRelic
            {
                Id = 61011,
                Level = 999,
                MainAffixId = 1,
                SubAffixLists = {new RelicAffix
                {
                    AffixId = 4,
                    Step = 999
                } }
            };

            //avatar
            List<uint> SkillIdEnds = new List<uint> { 1, 2, 3, 4, 7, 101, 102, 103, 201, 202, 203, 204, 205, 206, 207, 208, 209, 210 };
            List<uint> characters = new List<uint> { Avatar1, Avatar2, Avatar3, Avatar4 };
            foreach (uint avatarId in characters)
            {
                var avatarData = new BattleAvatar
                {
                    Id = avatarId,
                    Level = 80,
                    Promotion = 6,
                    Rank = 6,
                    Hp = 10000,
                    AvatarType = AvatarType.AvatarFormalType,
                    WorldLevel = 6,
                    SpBar = new SpBarInfo { CurAmount = 10000, MaxAmount = 10000 },
                    RelicLists = { testRelic },
                    EquipmentLists = {new BattleEquipment
                    {
                        Id = 23006,
                        Level = 80,
                        Rank = 5,
                        Promotion = 6
                    } }
                };

                foreach (uint end in SkillIdEnds)
                {
                    uint level = 1;
                    if (end == 1) level = 6;
                    else if (end < 4 || end == 4) level = 10;
                    if (end > 4) level = 1;
                    avatarData.SkilltreeLists.Add(new AvatarSkillTree
                    {
                        PointId = avatarId * 1000 + end,
                        Level = level
                    });
                }

                battleInfo.BattleAvatarLists.Add(avatarData);
            }

            //monster
            for (uint i = 1; i <= monsterIds.Count; i++)
            {
                SceneMonsterWave monsterInfo = new SceneMonsterWave
                {
                    WaveId = i,
                    WaveParam = new SceneMonsterWaveParam
                    {
                        Level = monsterLevels[i],
                    }
                };

                if (monsterIds.ContainsKey(i))
                {
                    List<uint> monsterIdList = monsterIds[i];

                    foreach (uint monsterId in monsterIdList)
                    {
                        monsterInfo.MonsterLists.Add(new SceneMonster
                        {
                            MonsterId = monsterId
                        });
                    }
                    
                }
                battleInfo.MonsterWaveLists.Add(monsterInfo);
            }

            var response = new StartCocoonStageScRsp
            {
                Retcode = 0,
                CocoonId = request.CocoonId,
                Wave = request.Wave,
                PropEntityId = request.PropEntityId,
                BattleInfo = battleInfo
            };

            session.Send(CmdType.CmdStartCocoonStageScRsp, response);
        }

        [Handler(CmdType.CmdPveBattleResultCsReq)]
        public static void OnPVEBattleResultCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as PVEBattleResultCsReq;
            session.Send(CmdType.CmdPVEBattleResultScRsp, new PVEBattleResultScRsp
            {
                Retcode = 0,
                EndStatus = request.EndStatus
            });
        }
    }
}
