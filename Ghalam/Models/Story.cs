using System;
using System.Collections.Generic;

namespace Ghalam.Models;

public partial class Story
{
    public int PkStory { get; set; }

    public string Title { get; set; } = null!;

    public int FkWriter { get; set; }

    public string? Text { get; set; }

    public virtual User FkWriterNavigation { get; set; } = null!;

    public virtual ICollection<LikedStory> LikedStories { get; set; } = new List<LikedStory>();
}
