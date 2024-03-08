using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

internal class GroupsRepository(ApplicationDbContext context) : BaseRepository<Group>(context), IGroupsRepository
{
   
}