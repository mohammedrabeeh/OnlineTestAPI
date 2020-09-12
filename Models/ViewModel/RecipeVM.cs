using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRecipeHub.Models.ViewModel
{
    public class RecipeVM
    {

        public int recipeId { get; set; }
        public string recipeTitle { get; set; }
        public string DateAdded { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public string levelName { get; set; }
        public int levelID { get; set; }

        public List<Steps> steps { get; set; }

        public List<Ingredients> ingredients { get; set; }
    }
}
