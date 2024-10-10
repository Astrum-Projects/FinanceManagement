using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasData(new List<Category>() {
                new Category { Id = 1, NameUz = "Oziq-ovqat", NameRu = "Продукты", NameEn = "Food", IsIncome = false, IsDeleted = false },
                new Category { Id = 2, NameUz = "Transport", NameRu = "Транспорт", NameEn = "Transport", IsIncome = false, IsDeleted = false },
                new Category { Id = 3, NameUz = "Kommunal xizmatlar", NameRu = "Коммунальные услуги", NameEn = "Utilities", IsIncome = false, IsDeleted = false },
                new Category { Id = 4, NameUz = "Uy-joy ijarasi", NameRu = "Аренда жилья", NameEn = "Rent", IsIncome = false, IsDeleted = false },
                new Category { Id = 5, NameUz = "Sog'liq va tibbiyot", NameRu = "Здоровье и медицина", NameEn = "Health", IsIncome = false, IsDeleted = false },
                new Category { Id = 6, NameUz = "O'yin-kulgi", NameRu = "Развлечения", NameEn = "Entertainment", IsIncome = false, IsDeleted = false },
                new Category { Id = 7, NameUz = "Ta'lim", NameRu = "Образование", NameEn = "Education", IsIncome = false, IsDeleted = false },
                new Category { Id = 8, NameUz = "Kredit to'lovlari", NameRu = "Кредитные выплаты", NameEn = "Loan Payments", IsIncome = false, IsDeleted = false },
                new Category { Id = 9, NameUz = "Kiyim-kechak", NameRu = "Одежда", NameEn = "Clothing", IsIncome = false, IsDeleted = false },
                new Category { Id = 10, NameUz = "Sayohat", NameRu = "Путешествия", NameEn = "Travel", IsIncome = false, IsDeleted = false },
                // Income categories
                new Category { Id = 11, NameUz = "Oylik maosh", NameRu = "Зарплата", NameEn = "Salary", IsIncome = true, IsDeleted = false },
                new Category { Id = 12, NameUz = "Bonus", NameRu = "Бонус", NameEn = "Bonus", IsIncome = true, IsDeleted = false },
                new Category { Id = 13, NameUz = "Qo'shimcha daromad", NameRu = "Дополнительный доход", NameEn = "Additional Income", IsIncome = true, IsDeleted = false },
                new Category { Id = 14, NameUz = "Investitsiya daromadi", NameRu = "Доход от инвестиций", NameEn = "Investment Income", IsIncome = true, IsDeleted = false },
            });
        }
    }
}
