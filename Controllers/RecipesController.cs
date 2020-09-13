using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineRecipeHub.Context;
using OnlineRecipeHub.Models;
using OnlineRecipeHub.Models.ViewModel;

namespace OnlineRecipeHub.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly OnlineRecipeHubContext _context;

        public RecipesController(OnlineRecipeHubContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        [HttpGet]
        public IEnumerable<RecipeVM> GetRecipe()
        {
            return _context.Recipe.Include(n => n.Level)
                .Select(r => new RecipeVM()
                {
                    recipeId = r.RecipeId,
                    recipeTitle = r.RecipeTitle,
                    DateAdded = r.DateAdded.ToString("dd-MMM-yyyy hh:mm tt"),
                    image1 = r.Image1,
                    image2 = r.Image2,
                    image3 = r.Image3,
                    levelName = r.Level.LevelName,
                    steps = _context.Steps.Where(s => s.RecipeId == r.RecipeId).Select(s => new Steps
                    { 
                        stepsId = s.stepsId,
                        stepName = s.stepName
                    }).ToList(),
                    ingredients = _context.Ingredients.Where(i => i.RecipeId == r.RecipeId).Select(s => new Ingredients
                    {
                        IngredientId = s.IngredientId,
                        IngredientName = s.IngredientName
                    }).ToList()

                }).ToList();
        }

        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public ActionResult<RecipeVM> GetRecipe(int id)
        {
            var recipe = _context.Recipe.Find(id);

            if (recipe == null)
            {
                return NotFound();
            }

            RecipeVM recipeVM = new RecipeVM();
            recipeVM.recipeId = recipe.RecipeId;
            recipeVM.recipeTitle = recipe.RecipeTitle;
            recipeVM.levelID = recipe.LevelId;
            recipeVM.image1 = recipe.Image1;
            recipeVM.image2 = recipe.Image2;
            recipeVM.image3 = recipe.Image3;


            List<Steps> steps = _context.Steps.Where(s => s.RecipeId == recipe.RecipeId).Select(s => new Steps
            {
                stepsId = s.stepsId,
                stepName = s.stepName
            }).ToList();

            recipeVM.steps = steps;

            List<Ingredients> Ingredients = _context.Ingredients.Where(i => i.RecipeId == recipe.RecipeId).Select(s => new Ingredients
            {
                IngredientId = s.IngredientId,
                IngredientName = s.IngredientName
            }).ToList();

            recipeVM.ingredients = Ingredients;

            return recipeVM;
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Recipes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public ActionResult<Recipe> PostRecipe(PostRecipeVM recipepost)
        {
            Recipe recipe = new Recipe();

            if(recipepost.RecipeID != "")
            {
                recipe.RecipeId = int.Parse(recipepost.RecipeID);
                recipe.DateAdded = DateTime.Now;
            }

            recipe.RecipeTitle = recipepost.RecipeTitle;
            recipe.LevelId = recipepost.LevelID;
            recipe.Image1 = recipepost.Image1;
            recipe.Image2 = recipepost.Image2;
            recipe.Image3 = recipepost.Image3;

            if (recipepost.RecipeID == "")
            {
                _context.Recipe.Add(recipe);
            }
            else
            {
                _context.Entry(recipe).State = EntityState.Modified;
                _context.Steps.RemoveRange(_context.Steps.Where(s => s.RecipeId == recipe.RecipeId));
                _context.Ingredients.RemoveRange(_context.Ingredients.Where(s => s.RecipeId == recipe.RecipeId));
            }

            _context.SaveChanges();

            foreach (Steps stp in recipepost.Steps)
            {
                Steps step = new Steps();
                step.stepName = stp.stepName;
                step.RecipeId = recipe.RecipeId;
                _context.Steps.Add(step);
            }

            foreach (Ingredients ingr in recipepost.Ingredients)
            {
                Ingredients ingredients = new Ingredients();
                ingredients.IngredientName = ingr.IngredientName;
                ingredients.RecipeId = recipe.RecipeId;
                _context.Ingredients.Add(ingredients);
            }
            _context.SaveChanges();

            return CreatedAtAction("GetRecipe", new { id = recipe.RecipeId }, recipe);
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("wwwroot","Uploads");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { fileName });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public ActionResult DeleteRecipe(int id)
        {
            var recipe = _context.Recipe.Find(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Steps.RemoveRange(_context.Steps.Where(s => s.RecipeId == recipe.RecipeId));
            _context.Ingredients.RemoveRange(_context.Ingredients.Where(s => s.RecipeId == recipe.RecipeId));
            _context.Recipe.Remove(recipe);
            //_context.SaveChanges();
            var result = true;
            return Ok(new { result }); 
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.RecipeId == id);
        }
    }
}
