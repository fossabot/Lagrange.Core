using Lagrange.Core.Common;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Internal.Packets.Service.Oidb.Response;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSListEvent))]
[EventSubscribe(typeof(GroupFSCountEvent))]
[EventSubscribe(typeof(GroupFSSpaceEvent))]
[Service("OidbSvcTrpcTcp.0x6d8_1")]
internal class GroupFSViewService : BaseService<GroupFSViewEvent>
{
    protected override bool Build(GroupFSViewEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        object packet = input switch
        {
            GroupFSListEvent list => new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D8_1>(
                new OidbSvcTrpcTcp0x6D8_1
                {
                    List = new OidbSvcTrpcTcp0x6D8_1List
                    {
                        GroupUin = list.GroupUin,
                        AppId = 7,
                        TargetDirectory = list.TargetDirectory,
                        FileCount = 20,
                        SortBy = 1,
                        StartIndex = list.StartIndex,
                        Field17 = 2,
                        Field18 = 0
                    }
                }, false, true),
            GroupFSCountEvent count => new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D8_2>(
                new OidbSvcTrpcTcp0x6D8_2
                {
                    Count = new OidbSvcTrpcTcp0x6D8_1Count { GroupUin = count.GroupUin, AppId = 7, BusId = 6 }
                }, false, true),
            GroupFSSpaceEvent space => new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D8_3>(
                new OidbSvcTrpcTcp0x6D8_3
                {
                    Space = new OidbSvcTrpcTcp0x6D8_1Space { GroupUin = space.GroupUin, AppId = 7 }
                }, false, true),
            _ => throw new Exception()
        };

        var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out GroupFSViewEvent output, out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<OidbSvcTrpcTcp0x6D8_1Response>>(input.AsSpan());

        extraEvents = null;

        if (packet.Body.List != null)
        {
            var list = packet.Body.List;
            var fileEntries = list.Items.Select(x =>
            {
                var f = x.FileInfo;
                var s = x.FolderInfo;
                
                IBotFSEntry entry = x.Type switch
                {
                    1 => new BotFileEntry(f.FileId, f.FileName, f.ParentDirectory, f.FileSize,
                        DateTime.UnixEpoch.AddSeconds(f.ExpireTime),
                        DateTime.UnixEpoch.AddSeconds(f.ModifiedTime), f.UploaderUin,
                        DateTime.UnixEpoch.AddSeconds(f.UploadedTime), f.DownloadedTimes),
                    2 => new BotFolderEntry(s.FolderId, s.ParentDirectoryId, s.FolderName,
                        DateTime.UnixEpoch.AddSeconds(s.CreateTime),
                        DateTime.UnixEpoch.AddSeconds(s.ModifiedTime), s.CreatorUin, s.TotalFileCount),
                    _ => throw new Exception("Unrecognized FileSystem Entry Found")
                };

                return entry;
            }).ToList();
            output = GroupFSListEvent.Result((int)packet.ErrorCode, fileEntries);
            return true;
        }

        if (packet.Body.Count != null)
        {
            var count = packet.Body.Count;
            output = GroupFSCountEvent.Result((int)packet.ErrorCode, count.FileCount, count.LimitCount, count.IsFull);
            return true;
        }
        if (packet.Body.Space != null)
        {
            var space = packet.Body.Space;
            output = GroupFSSpaceEvent.Result((int)packet.ErrorCode, space.TotalSpace, space.UsedSpace);
            return true;
        }
        
        throw new Exception($"Invalid Packet is given to {nameof(GroupFSViewService)}");
    }
}