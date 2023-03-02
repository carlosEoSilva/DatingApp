using Api.DTOs;
using Api.Entities;
using Api.Helpers;
using Api.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper= mapper;
            _context= context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query= _context.Users.AsQueryable();

            //-retornar todos os usuário menos o usuário atual.
            query= query.Where(u => u.UserName != userParams.CurrentUsername);
            //-retornar todos os usuários com 'gender' diferente do usuário atual.
            query= query.Where(u => u.Gender == userParams.Gender);

            //-DateOnly.FromDateTime, extrai apenas a data a partir do 'DateTime' informado.
            //-'DateTime.Today.AddYears', soma ao ano da data de hoje a quantidade informada como parâmetro.
            // var minDob= DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            // var maxDob= DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
            var minDob= DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob= DateTime.Today.AddYears(-userParams.MinAge);
            
            query= query.Where(u => u.DateOfBirth.Year >= minDob.Year && u.DateOfBirth.Year <= maxDob.Year);

            query= userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };
            
            return await PagedList<MemberDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MemberDto>(_mapper.ConfigurationProvider), 
                userParams.PageNumber, 
                userParams.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State= EntityState.Modified;
        }

        public async Task<string> GetUserGender(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .Select(x => x.Gender).FirstOrDefaultAsync();
        }

    }
}