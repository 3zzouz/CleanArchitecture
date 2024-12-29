namespace Core.Domain.IRepositories
{
    public interface GRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByGenreId(int Id);
        IEnumerable<T> GetAllOrdered();
    }
}