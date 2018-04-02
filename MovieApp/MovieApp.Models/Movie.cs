namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string PosterPath { get; set; }
        public string PosterPathBig { get; set; }
        public string ReleaseDate { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public string Genres { get; set; }
    }
}
