public class UserItemDTO{
	public int Id {get;set;}
	public string? Username {get; set;}
	public bool IsActive {get; set;}
	public UserItemDTO(){}
	public UserItemDTO(User userItem) => (Id, Username, IsActive) = (userItem.Id, userItem.Username, userItem.IsActive);
}
