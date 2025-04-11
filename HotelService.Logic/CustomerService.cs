using HotelService.Db;
using HotelService.Db.DTOs;
using HotelService.Db.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HotelService.Logic;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> ChangeData(CustomerDto customerDto)
    {
        var customerToChange =
            await _context.Customers.FirstOrDefaultAsync(c => 
                c.UserId == customerDto.UserId);
        var updatedCustomer = new Customer
        {
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            PhoneNumber = customerDto.PhoneNumber,
            DateOfBirth = customerDto.DateOfBirth

        };
        await _context.Customers.AddAsync(updatedCustomer);
        await _context.SaveChangesAsync();
        return updatedCustomer;
    }
    public async Task<Customer> AddNewCustomerAsync(CustomerDto customerDto)
    {
        var newCustomer = new Customer
        {
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            PhoneNumber = customerDto.PhoneNumber,
            DateOfBirth = customerDto.DateOfBirth,
            UserId = customerDto.UserId

        };
         await _context.Customers.AddAsync(newCustomer);
         await _context.SaveChangesAsync();
         return newCustomer;
    }
}