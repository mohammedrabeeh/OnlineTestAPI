﻿using System;
using System.Collections.Generic;

namespace OnlineRecipeHub.Models
{
    public partial class Level
    {
        public Level()
        {
            Recipe = new HashSet<Recipe>();
        }

        public int LevelId { get; set; }
        public string LevelName { get; set; }

        public virtual ICollection<Recipe> Recipe { get; set; }
    }
}
