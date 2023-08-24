using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly NpgsqlConnection _connection;
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>
                ("Select * from Coupon where ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null)
            {
                return new Coupon { Amount = 0, Description = "No Discount Desc", ProductName = "No Discount" };
            }
            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(@"insert into Coupon 
                (productname,description,amount) values (@ProductName,@Description,@Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var affected = await _connection.ExecuteAsync(@"Update Coupon 
                Set productname =@ProductName, description = @Description, amount = @Amount where Id = @Id",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var affected = await _connection.ExecuteAsync(@"Delete From Coupon  Where productname =@ProductName",
                  new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }




    }
}
