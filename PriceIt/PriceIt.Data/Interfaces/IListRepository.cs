using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriceIt.Data.Models;

namespace PriceIt.Data.Interfaces
{
    public interface IListRepository
    {
        AppUser GetCurrentUser();
        List<UserList> GetUserLists(AppUser user);
        UserList GetUserListById(int id);
        Task AddListAsync(string listname);
        bool DeleteList(int id);
        bool UpdateList(UserList list);
        bool HasAccessList(int id);
        bool Save();
    }
}
