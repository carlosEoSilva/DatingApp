using Api.Interfaces;
using AutoMapper;

namespace Api.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper= mapper;
            _context= context;
        }

        //-não entendi por foi usado o lambda...
        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);
        public ILikesRepository LikesRepository => new LikesRepository(_context);

        public async Task<bool> Complete()
        {
            //-25
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            //-26
            return _context.ChangeTracker.HasChanges();
        }
    }
}