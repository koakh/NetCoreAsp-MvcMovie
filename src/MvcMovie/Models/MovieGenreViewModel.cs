using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMovie.Models
{
    public class MovieGenreViewModel
    {
        //List of movies
        public List<Movie> movies;
        //SelectList containing the list of genres.This will allow the user to select a genre from the list.
        public SelectList selectGenre;
        //MovieGenre, which contains the selected genre
        public string selectedGenre { get; set; }
    }
}