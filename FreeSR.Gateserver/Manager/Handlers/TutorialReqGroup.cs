﻿namespace FreeSR.Gateserver.Manager.Handlers
{
    using FreeSR.Gateserver.Manager.Handlers.Core;
    using FreeSR.Gateserver.Network;
    using FreeSR.Proto;

    internal static class TutorialReqGroup
    {
        [Handler(CmdType.CmdGetTutorialGuideCsReq)]
        public static void OnGetTutorialGuideCsReq(NetSession session, int cmdId, object _)
        {
            var response = new GetTutorialGuideScRsp
            {
                Retcode = 0
            };

            uint[] guides = new uint[]
            {
                1101, 1102, 1103, 1104, 1105, 1108, 1109, 1116, 1117, 1118, 2006, 2007, 2008, 2105, 2106, 2107,
    2200, 2201, 2202, 2203, 2204, 2205, 2206, 2207, 2208, 2209, 2210, 2211, 2212, 2213, 2214, 2215,
    2216, 2217, 2218, 2219, 2220, 2221, 2222, 3007, 3105, 3106, 3107, 3108, 3109, 3201, 3202, 3203,
    3204, 3205, 3206, 3207, 3208, 3209, 3210, 3211, 3212, 3213, 3214, 3215, 4001, 4101, 4102, 4103,
    4104, 4105, 4106, 4107, 4109, 4110, 5101, 5102, 5103, 5104, 5105, 5106, 6001, 6002, 6003, 6004,
    6008, 6009, 6010, 6011, 6012, 6014, 6015, 6018, 6020, 6021, 6023, 6024, 6025, 6027, 6028, 6029,
    6030, 6031, 6032, 6033, 6034, 6035, 6036, 6037, 6038, 6039, 6040, 6041, 6042, 6043, 6044, 6045,
    6046, 6047, 6048, 6049, 6050, 6051, 6052, 6053, 6054, 6055, 6056, 6057, 6058, 6059, 6060, 6061,
    6062, 6063, 6064, 6065, 6066, 6067, 6068, 6069, 6070, 6071, 6072, 6073, 6074, 6075, 6076, 6077,
    6078, 6079, 6080, 6081, 6082, 6083, 6085, 6086, 6087, 6088, 6089, 6090, 6091, 6092, 6093, 6094,
    6095, 6096, 6097, 6098, 6099, 6100, 6101, 6102, 6103, 6104, 6105, 6106, 6107, 6108, 6109, 6110,
    6111, 7090, 7091, 7092, 7501, 7502, 7503, 7504, 7506, 7507, 7508, 7509, 7511, 7514, 7515, 8001,
    8002, 8003, 8004, 8006, 8007, 8008, 8010, 8011, 8012, 8013, 8014, 8015, 8016, 8017, 8018, 8019,
    8020, 8021, 8022, 8023, 8024, 8025, 8026, 8027, 8028, 8038, 8039, 8047, 8050, 8051, 8052, 8055,
    8056, 8057, 8058, 8059, 8061, 8062, 8063, 8064, 8065, 8066, 8067, 8069, 8070, 8072, 8073, 8074,
    8075, 8076, 8078, 8079, 8080, 8090, 8091, 8092, 8093, 8094, 8095, 8096, 8100, 8101, 8102, 8103,
    8104, 8105, 8106, 8107, 8108, 8109, 8110, 8111, 8112, 8113, 8122, 8123, 8124, 8140, 8141, 8142,
    8143, 8144, 8145, 8146, 9101, 9102, 9103, 9104, 9105, 9107, 9108, 9109, 9110, 9111, 9112, 9113,
    9114, 9115, 9116, 9117, 9118, 9119, 9120, 9201, 9202, 9203, 9204, 9205, 9206, 9207, 9208, 9209,
    9210, 9211, 9212, 9301, 9303, 9304, 9305, 9601, 9602, 9603, 9604, 9605, 9701, 9702, 9703,9704,31001,
    31102, 31103, 31105, 31106, 31109, 31204, 31206,
            };
            
            foreach (uint id in guides)
            {
                response.TutorialGuideLists.Add(new TutorialGuide
                {
                    Id = id,
                    Status = TutorialStatus.TutorialFinish
                });
            }

            session.Send(CmdType.CmdGetTutorialGuideScRsp, response);
        }

        [Handler(CmdType.CmdGetTutorialCsReq)]
        public static void OnGetTutorialCsReq(NetSession session, int cmdId, object _)
        {
            uint[] completedTutorials = new uint[]
            {
                1001, 1002, 1003, 1004, 1005, 1007, 1008, 1010, 1011,
                2001, 2002, 2003, 2004, 2005, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015,
                3001, 3002, 3003, 3004, 3005, 3006,
                4002, 4003, 4004, 4005, 4006, 4007, 4008, 4009,
                5001, 5002, 5003, 5004, 5005, 5006, 5007, 5008, 5009, 5010, 5011, 5012,
                7001,
                9001, 9002, 9003, 9004, 9005, 9006
            };

            var response = new GetTutorialScRsp
            {
                Retcode = 0,
            };

            foreach (uint id in completedTutorials)
            {
                response.TutorialLists.Add(new Tutorial
                {
                    Id = id,
                    Status = TutorialStatus.TutorialFinish
                });
            }

            session.Send(CmdType.CmdGetTutorialScRsp, response);
        }
    }
}
