using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Models;

public class PersonGenreViewModel{
    public List<PersonModel>? PersonModel {get; set;}
    public SelectList? Genres {get; set;}
    public string? PersonGenre {get; set;}
    public string? search {get; set;}
}