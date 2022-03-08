using WelcomeWeb.Data;
using WelcomeWeb.Models;

namespace WelcomeWeb.Master
{
    public class CategoryDA
    {
        private readonly ApplicationDbContext Database;
        public CategoryDA(ApplicationDbContext Context)
        {
            Database = Context;
        }

        public List<Category> GetAll()
        {
            return Database.Categories.ToList();
        }
        public Category GetbyId(int Id)
        {
            return Database.Categories.FirstOrDefault(x => x.CategoryId == Id);
        }
        public string Remove(int Id)
        {
            string Result = string.Empty;
            var cat = Database.Categories.FirstOrDefault(c => c.CategoryId == Id);
            if(cat != null)
            {
                Database.Categories.Remove(cat);
                Database.SaveChanges();
                Result = "Pass";
            }
            return Result;
        }
        public string Save(Category tblCategory)
        {
            string Result = string.Empty;
            var cat = Database.Categories.FirstOrDefault(c => c.CategoryId == tblCategory.CategoryId);
            if (cat != null)
            {
                cat.CategoryName = tblCategory.CategoryName;
                cat.OrderPlace = tblCategory.OrderPlace;
                cat.CreateDate = tblCategory.CreateDate;
                Database.SaveChanges();
                Result = "Pass";
            }
            else
            {
                Category c = new Category
                {
                    CategoryName = tblCategory.CategoryName,
                    OrderPlace = tblCategory.OrderPlace,
                    CreateDate = tblCategory.CreateDate
                };
                Database.Categories.Add(c);
                Database.SaveChanges();
                Result = "Pass";
            }
            return Result;
        }
    }
}
