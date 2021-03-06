using System.Collections.Generic;
using PersonApi.Data.VO;

namespace PersonApi.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);

        List<BookVO> FindAll();

        BookVO FindById(long id);

        BookVO Update(BookVO book);

        void Delete(long id);
    }
}