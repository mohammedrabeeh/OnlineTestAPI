using System;
using System.Collections.Generic;

namespace OnlineRecipeHub.Models
{
    public partial class Steps
    {
        public int stepsId { get; set; }
        public string stepName { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
