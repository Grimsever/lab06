using System.Threading.Tasks;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;

namespace Lab06.MVC.BL.Service
{
    public interface ISongServices : IServices<Song>
    {
        Task<EditSongViewModel> GetEditViewModel(int id);

        Task Update(EditSongViewModel model);
    }
}