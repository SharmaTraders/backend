namespace Data.Entities;


// All the entities are internal, so they can only be accessed from within the same assembly.
internal class Administrator {

    public string Email { get; set; }
    public string Password { get; set; }
}