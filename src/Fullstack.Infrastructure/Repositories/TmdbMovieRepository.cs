using Fullstack.Domain.entities;
using Fullstack.Domain.Interfaces;
using Fullstack.Infrastructure.Options;
using Microsoft.Ajax.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Fullstack.Infrastructure.Repositories
{
    public class TmdbMovieRepository : IMovieRepository
    {
        private readonly HttpClient _httpClient;
        private readonly TmdbOptions _settings;
        private const string BaseUrl = "https://api.themoviedb.org/3";

        public TmdbMovieRepository(HttpClient httpClient, IOptions<TmdbOptions> options)
        {
            _httpClient = httpClient;
            _settings=options.Value;
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", _settings.ApiKey);
        }
        public async Task<IEnumerable<Movie>> SearchMoviesAsync(string? query)
        {
            string endpoint = string.IsNullOrWhiteSpace(query)
               ? $"{BaseUrl}/discover/movie?sort_by=popularity.desc"
               : $"{BaseUrl}/search/movie?query={Uri.EscapeDataString(query)}";

            var response = await _httpClient.GetAsync(endpoint);
            var json = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            var tmdbResponse = JsonSerializer.Deserialize<TmdbResponse>(json,
              new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return tmdbResponse?.Results?.Select(r =>
                new Movie(r.Id, r.Title, r.VoteAverage, r.PosterPath, r.ReleaseDate, r.OriginalLanguage)
            ) ?? Enumerable.Empty<Movie>();
        }
        private class TmdbResponse
        {
            public List<TmdbMovie> Results { get; set; } = new();
        }

        private class TmdbMovie
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("vote_average")]
            public double VoteAverage { get; set; }

            [JsonPropertyName("poster_path")]
            public string PosterPath { get; set; }

            [JsonPropertyName("release_date")]
            public string ReleaseDate { get; set; }

            [JsonPropertyName("original_language")]
            public string OriginalLanguage { get; set; }
        }

    }



}
