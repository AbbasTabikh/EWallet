using EWallet.Dtos;
using EWallet.Entities;
using EWallet.Enums;
using EWallet.InputModels;

namespace EWallet.Mappings
{
    public static class ExpenseMapping
    {
        public static Expense ToExpense(this CreateExpenseInputModel createExpenseInputModel)
        {
            return new Expense
            {
                BudgetID = createExpenseInputModel.BudgetID,
                Category = GetCategory(createExpenseInputModel.Category),
                Name = createExpenseInputModel.Name,
                Price = createExpenseInputModel.Price,
                CreationDate = DateTime.UtcNow.Date
            };
        }
        public static ExpenseDto ToDto(this Expense expense)
        {
            return new ExpenseDto
            {
                Category = GetCategoryName(expense.Category),
                Name = expense.Name,
                ID = expense.ID,
                Price = expense.Price
            };
        }


        private static Category GetCategory(string  categoryName)
        {
            return categoryName switch
            {
                "Food" => Category.Food,
                "Shopping" => Category.Shopping,
                "Transportation" => Category.Transportation,
                "Entertainment" => Category.Entertainment,
                "Travel" => Category.Travel,
                "Savings" => Category.Savings,
                "Health" => Category.Health,
                "Housing" => Category.Housing,
                "Education" => Category.Education,
                "Utitlies" => Category.Utitlies,
                _ => Category.Other,
            };
        }
        private static string GetCategoryName(Category category)
        {
            return category switch
            {
                Category.Food => "Food",
                Category.Shopping => "Shopping",
                Category.Transportation => "Transportation",
                Category.Entertainment => "Entertainment",
                Category.Travel => "Travel",
                Category.Savings => "Savings",
                Category.Health => "Health",
                Category.Housing => "Housing",
                Category.Education => "Education",
                Category.Utitlies => "Utitlies",
                _ => "Other",
            };
        }
    }
}
