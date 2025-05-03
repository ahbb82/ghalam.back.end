using System;
using System.Collections.Generic;

namespace Ghalam.Models;

public partial class LikedStory
{
    public int PkLikedStory { get; set; }

    public int FkUser { get; set; }

    public int FkStory { get; set; }

    public virtual Story FkStoryNavigation { get; set; } = null!;

    public virtual User FkUserNavigation { get; set; } = null!;
}
