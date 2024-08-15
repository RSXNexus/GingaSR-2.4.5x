namespace FreeSR.Gateserver.Network.Factory
{
    using FreeSR.Proto;
    using ProtoBuf;
    using System.Collections.Immutable;

    internal static class ProtoFactory
    {
        private static readonly ImmutableDictionary<CmdType, Type> s_types;

        static ProtoFactory()
        {
            var builder = ImmutableDictionary.CreateBuilder<CmdType, Type>();
            builder.AddRange(new Dictionary<CmdType, Type>()
            {
                {CmdType.CmdPlayerGetTokenCsReq, typeof(PlayerGetTokenCsReq)},
                {CmdType.CmdPlayerLoginCsReq, typeof(PlayerLoginCsReq)},
                {CmdType.CmdGetAvatarDataCsReq, typeof(GetAvatarDataCsReq)},

                {CmdType.CmdGetAllLineupDataCsReq, typeof(GetAllLineupDataCsReq)},
                {CmdType.CmdGetCurLineupDataCsReq, typeof(GetCurLineupDataCsReq)},
                {CmdType.CmdChangeLineupLeaderCsReq, typeof(ChangeLineupLeaderCsReq)},

                {CmdType.CmdGetMissionStatusCsReq, typeof(GetMissionStatusCsReq)},

                {CmdType.CmdGetCurSceneInfoCsReq, typeof(GetCurSceneInfoCsReq)},
                {CmdType.CmdGetSceneMapInfoCsReq, typeof(GetSceneMapInfoCsReq)},

                {CmdType.CmdGetBasicInfoCsReq, typeof(GetBasicInfoCsReq)},
                {CmdType.CmdGetMultiPathAvatarInfoCsReq, typeof(GetMultiPathAvatarInfoCsReq)},
                {CmdType.CmdPlayerHeartBeatCsReq, typeof(PlayerHeartBeatCsReq)},

                //{CmdType.CmdGetGachaInfoCsReq, typeof(GetGachaInfoCsReq)},
                //{CmdType.CmdDoGachaCsReq, typeof(DoGachaCsReq)},

                //{CmdType.CmdGetNpcTakenRewardCsReq, typeof(GetNpcTakenRewardCsReq)},
                //{CmdType.CmdGetFirstTalkByPerformanceNpcCsReq, typeof(GetFirstTalkByPerformanceNpcCsReq)},
                {CmdType.CmdGetLineupAvatarDataCsReq, typeof(GetLineupAvatarDataCsReq)},
                {CmdType.CmdSceneEntityMoveCsReq, typeof(SceneEntityMoveCsReq)},
                {CmdType.CmdReplaceLineupCsReq, typeof(ReplaceLineupCsReq)},
                {CmdType.CmdSetAvatarPathCsReq, typeof(SetAvatarPathCsReq)},
                {CmdType.CmdJoinLineupCsReq, typeof(JoinLineupCsReq)},
                //{CmdType.CmdEnterSceneCsReq, typeof(EnterSceneCsReq)},
                {CmdType.CmdQuitLineupCsReq, typeof(QuitLineupCsReq)},
                {CmdType.CmdSwapLineupCsReq, typeof(SwapLineupCsReq)},

                {CmdType.CmdSetLineupNameCsReq, typeof(SetLineupNameCsReq)},
                {CmdType.CmdStartCocoonStageCsReq, typeof(StartCocoonStageCsReq)},
                {CmdType.CmdPveBattleResultCsReq, typeof(PVEBattleResultCsReq)}
            });

            s_types = builder.ToImmutable();
        }

        public static object Deserialize(int id, byte[] rawData)
        {
            if (s_types.TryGetValue((CmdType)id, out var type))
                return Serializer.Deserialize(type, new MemoryStream(rawData));

            return null;
        }
    }
}
