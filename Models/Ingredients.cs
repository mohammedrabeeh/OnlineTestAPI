using System;
using System.Collections.Generic;

namespace OnlineRecipeHub.Models
{
    public partial class Ingredients
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
