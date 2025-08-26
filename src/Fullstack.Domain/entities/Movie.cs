using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fullstack.Domain.entities
{
    public class Movie
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

        public Movie(int Id,string title, double voteAverage, string posterPath, string releaseDate, string originalLanguage)
        {
            this.Id = Id;
            Title = title;
            VoteAverage = voteAverage;
            PosterPath = posterPath;
            ReleaseDate = releaseDate;
            OriginalLanguage = originalLanguage;
        }
    }
}
