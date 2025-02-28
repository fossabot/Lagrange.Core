using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group File Upload
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x6d6, 0)]
internal class OidbSvcTrpcTcp0x6D6_0
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x6D6_0Upload? File { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x6D6_0Upload
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint AppId { get; set; } // 7
    
    [ProtoMember(3)] public uint BusId { get; set; } // 102
    
    [ProtoMember(4)] public uint Entrance { get; set; } // 6
    
    [ProtoMember(5)] public string TargetDirectory { get; set; }
    
    [ProtoMember(6)] public string FileName { get; set; }
    
    [ProtoMember(7)] public string LocalDirectory { get; set; }
    
    [ProtoMember(8)] public ulong FileSize { get; set; }
    
    [ProtoMember(9)] public byte[] FileSha1 { get; set; }
    
    [ProtoMember(10)] public byte[] FileSha3 { get; set; } // ?
    
    [ProtoMember(11)] public byte[] FileMd5 { get; set; }
}