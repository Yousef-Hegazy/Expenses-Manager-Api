using API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        context.Expenses.RemoveRange(context.Expenses);
        context.Users.RemoveRange(context.Users);
        context.UserClaims.RemoveRange(context.UserClaims);
        context.UserLogins.RemoveRange(context.UserLogins);
        context.UserRoles.RemoveRange(context.UserRoles);
        context.UserTokens.RemoveRange(context.UserTokens);
        
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Title = "Education",
                    Description = "Online courses, school fees, textbooks, etc."
                },
                new Category
                {
                    Title = "Health care",
                    Description = "Doctor visits, medication, health insurance, etc."
                },
                new Category
                {
                    Title = "Social welfare",
                    Description = "Charitable donations, social security, etc."
                },
                new Category
                {
                    Title = "Public safety",
                    Description = "Fire protection, law enforcement, emergency services, etc."
                },
                new Category
                {
                    Title = "Housing",
                    Description = "home repairs and maintenance, etc."
                },
                new Category
                {
                    Title = "Entertainment",
                    Description = "Movies, concerts, hobbies, etc."
                },
                new Category
                {
                    Title = "Clothing",
                    Description = "Clothes and accessories for work and leisure."
                },
                new Category
                {
                    Title = "Bills",
                    Description = "Electricity, Water, Internet bills, etc."
                },
                new Category
                {
                    Title = "Food & Drinks",
                    Description = "Expenses on food, and drinks, and other basic necessities."
                },
                new Category
                {
                    Title = "Other",
                    Description = "Expenses not covered by other categories."
                }
            };
            
            await context.Categories.AddRangeAsync(categories);

            await context.SaveChangesAsync();
        }
    }
}