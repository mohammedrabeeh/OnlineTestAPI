using System;
using System.Collections.Generic;

namespace OnlineRecipeHub.Models
{
    public partial class Recipe
    {
        public Recipe()
        {
            Ingredients = new HashSet<Ingredients>();
            Steps = new HashSet<Steps>();
        }

        public int RecipeId { get; set; }
        public string RecipeTitle { get; set; }
        public DateTime DateAdded { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int LevelId { get; set; }

        public virtual Level Level { get; set; }
        public virtual ICollection<Ingredients> Ingredients { get; set; }
        public virtual ICollection<Steps> Steps { get; set; }
    }
}
