using System.ComponentModel.DataAnnotations;

public class Producer:IEntityBase
{
    [Key]
    public int id {get;set;}    
    public string FullName {get;set;}
    public string ProfilePicUrl {get;set;}

    public string Bio {get;set;}



    public List<Movie> Movies {get;set;}
}
