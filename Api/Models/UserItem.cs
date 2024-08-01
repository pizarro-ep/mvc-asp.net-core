namespace Api.Models;

public class UserItem{
    public long Id {get;set;}
    public string? Username {get;set;}
    public bool IsActive{get;set;}

    public string? Password{get;set;}
}