namespace FreeSR.Gateserver.Manager.Handlers
{
    using FreeSR.Gateserver.Manager.Handlers.Core;
    using FreeSR.Gateserver.Network;
    using FreeSR.Proto;
    using NLog;

    using System;

    internal static class PlayerReqGroup
    {
        [Handler(CmdType.CmdPlayerHeartBeatCsReq)]
        public static void OnPlayerHeartBeatCsReq(NetSession session, int cmdId, object data)
        {
            var heartbeatReq = data as PlayerHeartBeatCsReq;

            session.Send(CmdType.CmdPlayerHeartBeatScRsp, new PlayerHeartBeatScRsp
            {
                Retcode = 0,

                DownloadData = new ClientDownloadData
                {
                    Version = 51,
                    Time = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Data = Convert.FromBase64String("bG9jYWwgZnVuY3Rpb24gYmV0YV90ZXh0KG9iaikKICBsb2NhbCBnYW1lT2JqZWN0ID0gQ1MuVW5pdHlFbmdpbmUuR2FtZU9iamVjdC5GaW5kKCJVSVJvb3QvQWJvdmVEaWFsb2cvQmV0YUhpbnREaWFsb2coQ2xvbmUpIikKCiAgaWYgZ2FtZU9iamVjdCB0aGVuCiAgICAgIGxvY2FsIHRleHRDb21wb25lbnQgPSBnYW1lT2JqZWN0OkdldENvbXBvbmVudEluQ2hpbGRyZW4odHlwZW9mKENTLlJQRy5DbGllbnQuTG9jYWxpemVkVGV4dCkpCgogICAgICBpZiB0ZXh0Q29tcG9uZW50IHRoZW4KICAgICAgICAgIHRleHRDb21wb25lbnQudGV4dCA9ICI8Y29sb3I9IzAwRkZGRj7ml6DorrrkvaDmnInlpJrlvLrvvIzpk7bmsrPpg73lnKjkvaDkuYvkuIrvvIE8L2NvbG9yPiIKICAgICAgZWxzZQogICAgICAgICAgLS0gbG9nOndyaXRlKCJObyBUZXh0IGNvbXBvbmVudCBmb3VuZCBvbiB0aGUgZ2FtZSBvYmplY3QiKQogICAgICBlbmQKICBlbHNlCiAgICAgIC0tIGxvZzp3cml0ZSgiR2FtZSBvYmplY3Qgbm90IGZvdW5kIikKICBlbmQKZW5kCgpsb2NhbCBmdW5jdGlvbiB2ZXJzaW9uX3RleHQob2JqKQogIGxvY2FsIGdhbWVPYmplY3QgPSBDUy5Vbml0eUVuZ2luZS5HYW1lT2JqZWN0LkZpbmQoIlZlcnNpb25UZXh0IikKCiAgaWYgZ2FtZU9iamVjdCB0aGVuCiAgICAgIGxvY2FsIHRleHRDb21wb25lbnQgPSBnYW1lT2JqZWN0OkdldENvbXBvbmVudEluQ2hpbGRyZW4odHlwZW9mKENTLlJQRy5DbGllbnQuTG9jYWxpemVkVGV4dCkpCgogICAgICBpZiB0ZXh0Q29tcG9uZW50IHRoZW4KICAgICAgICAgIHRleHRDb21wb25lbnQudGV4dCA9ICIiCiAgICAgIGVsc2UKICAgICAgICAgIC0tIGxvZzp3cml0ZSgiTm8gVGV4dCBjb21wb25lbnQgZm91bmQgb24gdGhlIGdhbWUgb2JqZWN0IikKICAgICAgZW5kCiAgZWxzZQogICAgICAtLSBsb2c6d3JpdGUoIkdhbWUgb2JqZWN0IG5vdCBmb3VuZCIpCiAgZW5kCmVuZAoKbG9jYWwgZnVuY3Rpb24gbWh5X3RleHQob2JqKQogIGxvY2FsIGdhbWVPYmplY3QgPSBDUy5Vbml0eUVuZ2luZS5HYW1lT2JqZWN0LkZpbmQoIklETUFQMSIpCgogIGlmIGdhbWVPYmplY3QgdGhlbgogICAgICBsb2NhbCB0ZXh0Q29tcG9uZW50ID0gZ2FtZU9iamVjdDpHZXRDb21wb25lbnRJbkNoaWxkcmVuKHR5cGVvZihDUy5SUEcuQ2xpZW50Lk1lc3NhZ2VCb3hEaWFsb2dVdGlsKSkKCiAgICAgIGlmIHRleHRDb21wb25lbnQgdGhlbgogICAgICAgICAgdGV4dENvbXBvbmVudC5TaG93QWJvdmVEaWFsb2dUZXh0ID0gZmFsc2UKICAgICAgZWxzZQogICAgICAgICAgLS0gbG9nOndyaXRlKCJObyBUZXh0IGNvbXBvbmVudCBmb3VuZCBvbiB0aGUgZ2FtZSBvYmplY3QiKQogICAgICBlbmQKICBlbHNlCiAgICAgIC0tIGxvZzp3cml0ZSgiR2FtZSBvYmplY3Qgbm90IGZvdW5kIikKICBlbmQKZW5kCgpsb2NhbCBvbl9lcnJvciA9IGZ1bmN0aW9uKGVycm9yKQogIENTLlVuaXR5RW5naW5lLkFwcGxpY2F0aW9uLnRhcmdldEZyYW1lUmF0ZSA9IDEyMAogIENTLlVuaXR5RW5naW5lLlF1YWxpdHlTZXR0aW5ncy52U3luY0NvdW50ID0gMAogIGxvY2FsIGZpbGVzID0gaW8ub3BlbigiLi9lcnJvci50eHQiLCAidyIpCiAgZmlsZXM6d3JpdGUoZXJyb3IpCiAgZmlsZXM6Y2xvc2UoKQplbmQKeHBjYWxsKG1haW5fZnVuY3Rpb24sIG9uX2Vycm9yKQoKdmVyc2lvbl90ZXh0KCkKYmV0YV90ZXh0KCkKbWh5X3RleHQoKQ==")
                },
                ClientTimeMs = heartbeatReq.ClientTimeMs,
                ServerTimeMs = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds()
            });
        }

        [Handler(CmdType.CmdGetMultiPathAvatarInfoCsReq)]
        public static void OnGetMultiPathAvatarInfoCsReq(NetSession session, int cmdId, object _)
        {
            uint[] back = { 8001,8003,8005,1001,1224};
            GetMultiPathAvatarInfoScRsp response = new GetMultiPathAvatarInfoScRsp
            {
                Retcode = 0,
                AvatarTypeInfoLists ={
                    new MultiPathAvatarTypeInfo
                    {
                        AvatarId = MultiPathAvatarType.BoyWarriorType,
                    },
                    new MultiPathAvatarTypeInfo
                    {
                        AvatarId = MultiPathAvatarType.BoyKnightType,
                    },
                    new MultiPathAvatarTypeInfo
                    {
                        AvatarId = MultiPathAvatarType.BoyShamanType,
                    },
                    new MultiPathAvatarTypeInfo
                    {
                        AvatarId = MultiPathAvatarType.Mar7thKnightType,
                    },
                    new MultiPathAvatarTypeInfo
                    {
                        AvatarId = MultiPathAvatarType.Mar7thRogueType,
                    },
                },
                MultiAvatarTypeIdLists = back,
            };
            response.CurrentMultiAvatarIds[8001] = MultiPathAvatarType.BoyWarriorType;
            response.CurrentMultiAvatarIds[8003] = MultiPathAvatarType.BoyKnightType;
            response.CurrentMultiAvatarIds[8005] = MultiPathAvatarType.BoyShamanType;
            response.CurrentMultiAvatarIds[1001] = MultiPathAvatarType.Mar7thKnightType;
            response.CurrentMultiAvatarIds[1224] = MultiPathAvatarType.Mar7thRogueType;
            session.Send(CmdType.CmdGetMultiPathAvatarInfoScRsp, response);
        }

        [Handler(CmdType.CmdGetBasicInfoCsReq)]
        public static void OnGetBasicInfoCsReq(NetSession session, int cmdId, object _)
        {
            session.Send(CmdType.CmdGetBasicInfoScRsp, new GetBasicInfoScRsp
            {
                CurDay = 1,
                ExchangeTimes = 0,
                Retcode = 0,
                NextRecoverTime = 2281337,
                WeekCocoonFinishedCount = 0
            });
        }

        [Handler(CmdType.CmdPlayerLoginCsReq)]
        public static void OnPlayerLoginCsReq(NetSession session, int cmdId, object data)
        {
            var request = data as PlayerLoginCsReq;
            session.Send(CmdType.CmdPlayerLoginScRsp, new PlayerLoginScRsp
            {
                Retcode = 0,
                LoginRandom = request.LoginRandom,
                Stamina = 240,
                ServerTimestampMs = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds() * 1000,
                BasicInfo = new PlayerBasicInfo
                {
                    Nickname = "银河",
                    Level = 70,
                    Exp = 0,
                    Stamina = 100,
                    Mcoin = 100,
                    Hcoin = 100,
                    Scoin = 100,
                    WorldLevel = 6
                }
            });
        }

        [Handler(CmdType.CmdPlayerGetTokenCsReq)]
        public static void OnPlayerGetTokenCsReq(NetSession session, int cmdId, object data)
        {
            session.Send(CmdType.CmdPlayerGetTokenScRsp, new PlayerGetTokenScRsp
            {
                Retcode = 0,
                Uid = 100001,
                Msg = "OK",
                SecretKeySeed = 0
            });
        }
    }
}
