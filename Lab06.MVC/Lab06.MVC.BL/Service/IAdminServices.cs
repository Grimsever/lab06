using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;

namespace Lab06.MVC.BL.Service
{
    public interface IAdminServices
    {
        Task<List<UserViewModel>> GetAll();
        Task<User> Get(string id);

        Task<EditUserViewModel> GetEditViewUser(string id);

        void Update(string id, EditUserViewModel model);

        void Remove(string id);
    }
}