using PersonApi.Data.VO;
using PersonApi.Models;

namespace PersonApi.Business
{
    public interface ILoginBusiness
    {
        object FindByLogin(UserVO user);
    }
}