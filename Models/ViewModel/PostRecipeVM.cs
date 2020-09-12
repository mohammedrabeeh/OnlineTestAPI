using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRecipeHub.Models.ViewModel
{
    public class PostRecipeVM
    {

        public string RecipeID { get; set; }
        public string RecipeTitle { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int LevelID { get; set; }

        public Steps[] Steps { get; set; }

        public Ingredients[] Ingredients { get; set; }
    }
}
