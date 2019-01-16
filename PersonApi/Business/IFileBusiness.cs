using PersonApi.Data.VO;

namespace PersonApi.Business
{
    public interface IFileBusiness
    {
        byte[] GetPDFFile();
    }
}