using Lagrange.Core.Common.Entity;

namespace Lagrange.Core.Common.Interface.Api;

public static class GroupExt
{
    public static Task<bool> MuteGroupMember(this BotContext bot, uint groupUin, uint targetUin, uint duration)
        => bot.ContextCollection.Business.OperationLogic.MuteGroupMember(groupUin, targetUin, duration);
    
    public static Task<bool> MuteGroupGlobal(this BotContext bot, uint groupUin, bool isMute)
        => bot.ContextCollection.Business.OperationLogic.MuteGroupGlobal(groupUin, isMute);
    
    public static Task<bool> KickGroupMember(this BotContext bot, uint groupUin, uint targetUin, bool rejectAddRequest)
        => bot.ContextCollection.Business.OperationLogic.KickGroupMember(groupUin, targetUin, rejectAddRequest);
    
    public static Task<bool> SetGroupAdmin(this BotContext bot, uint groupUin, uint targetUin, bool isAdmin)
        => bot.ContextCollection.Business.OperationLogic.SetGroupAdmin(groupUin, targetUin, isAdmin);
    
    public static Task<bool> RenameGroupMember(this BotContext bot, uint groupUin, uint targetUin, string targetName)
        => bot.ContextCollection.Business.OperationLogic.RenameGroupMember(groupUin, targetUin, targetName);
    
    public static Task<bool> RenameGroup(this BotContext bot, uint groupUin, string targetName)
        => bot.ContextCollection.Business.OperationLogic.RenameGroup(groupUin, targetName);
    
    public static Task<bool> RemarkGroup(this BotContext bot, uint groupUin, string targetRemark)
        => bot.ContextCollection.Business.OperationLogic.RemarkGroup(groupUin, targetRemark);

    public static Task<ulong> FetchGroupFSSpace(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSSpace(groupUin);
    
    public static Task<uint> FetchGroupFSCount(this BotContext bot, uint groupUin)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSCount(groupUin);

    public static Task<List<IBotFSEntry>> FetchGroupFSList(this BotContext bot, uint groupUin, string targetDirectory = "/", uint startIndex = 0)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSList(groupUin, targetDirectory, startIndex);

    public static Task<string> FetchGroupFSDownload(this BotContext bot, uint groupUin, string fileId)
        => bot.ContextCollection.Business.OperationLogic.FetchGroupFSDownload(groupUin, fileId);

    public static Task<bool> GroupFSMove(this BotContext bot, uint groupUin, string fileId, string parentDirectory, string targetDirectory)
        => bot.ContextCollection.Business.OperationLogic.GroupFSMove(groupUin, fileId, parentDirectory, targetDirectory);
}