using System.Reflection;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Services.Attributes;

namespace Services.Decorators;

internal abstract class BaseEnsureRightsDecorator(IRepositoryManager repoManager)
{
    protected async Task EnsureRights<T>(T decoratee,
        string methodName, 
        Guid requesterId,
        Guid groupId,
        params Type[] restParams)
    {
        // получаем члена группы
        var member = await CheckMember(requesterId, groupId);
        // получаем атрибут метода сервиса
        var attr = decoratee?.GetType().GetMethod(methodName,
                new[] { requesterId.GetType(), groupId.GetType() }
                    .Concat(restParams).ToArray())!
            .GetCustomAttribute<EnsureRequesterRightsAttribute>();
        if (attr is null)
            throw new NoEnsureRightsAttributeOnMethodInternalServerErrorException(methodName);
        // получаем права, необходимые для его вызова
        var rightsToEnsure = attr.RightsNames.Select(r => r.ToUpper()).ToArray();
        // получаем имеющиеся у члена группы права
        await repoManager.Memberships.LoadReference(member, m => m.GroupRole);
        await repoManager.GroupRoles.LoadCollection(member.GroupRole, gr => gr.GroupRights);
        var memberRights = member.GroupRole.GroupRights
            .Select(gr => gr.NormalizedName)
            .ToHashSet();
        // сравниваем
        var missingRights = rightsToEnsure.Where(rte => !memberRights.Contains(rte)).ToList();
        if (missingRights.Count > 0)
            throw new NotEnoughRightsForbiddenException(member.Id, missingRights);
    }
    private async Task<Membership> CheckMember(Guid userId, Guid groupId)
    {
        var member = await repoManager.Memberships.GetByUserIdAndGroupId(userId, groupId);
        if (member is null)
            throw new MemberNotFoundException(userId, groupId);
        return member;
    }
}