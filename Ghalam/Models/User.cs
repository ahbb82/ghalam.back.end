using System;
using System.Collections.Generic;

namespace Ghalam.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsWriter { get; set; }

    public virtual ICollection<LikedStory> LikedStories { get; set; } = new List<LikedStory>();

    public virtual ICollection<Story> Stories { get; set; } = new List<Story>();
}
