using Fullstack.Domain.entities;
using Fullstack.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fullstack.Application.Services
{
    public class MovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepo)
        {
            _movieRepository= movieRepo;
        }
        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? query)
        {
            return await _movieRepository.SearchMoviesAsync(query);
        }
    }
}
