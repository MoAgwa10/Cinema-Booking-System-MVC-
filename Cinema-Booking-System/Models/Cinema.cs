using System.ComponentModel.DataAnnotations;

public class Cinema : IEntityBase
{
    [Key]
    public int id {get;set;}    
    public string FullName {get;set;}

    public string LogoUrl{get;set;}
    public string Description {get;set;}
    
    public List<Movie_Cinema> Movies_Cinemas { get; set; }
}