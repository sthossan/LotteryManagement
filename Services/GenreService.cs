using Models.DataContext;
using Models.DataModels;
using Services.Repository.Implementation;

namespace Services
{
    public class GenreService : GenericRepository<Genre>, IGenreService
    {
        private readonly EfDbContext _context;
        public GenreService(EfDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
