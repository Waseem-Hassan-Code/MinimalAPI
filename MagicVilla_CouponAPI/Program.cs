using MagicVilla_CouponAPI;
using MagicVilla_CouponAPI.Data;
using MagicVilla_CouponAPI.Model;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAutoMapper(typeof(MappingConfiguration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> _logger) =>
{
    _logger.Log(LogLevel.Information, "Getting all coupons");
    return Results.Ok(CouponStore.CouponList);
}).WithName("GetAllCoupons");

app.MapGet("/api/coupon/{id:int}",(int id) =>
{
    var coupon=CouponStore.CouponList.FirstOrDefault(x => x.Id == id);
    return Results.Ok(coupon);
}).WithName("GetCoupon");

app.MapPost("/api/coupon", ([FromBody] Coupon coupon) =>
{
    if (coupon.Id != 0 || string.IsNullOrEmpty(coupon.Name))
        return Results.BadRequest("Invalid Coupon Name or Coupon ID.");

    if (CouponStore.CouponList.FirstOrDefault(u => u.Name.ToLower() == coupon.Name.ToLower()) != null)
        return Results.BadRequest("Coupon with this name is already available.");

    coupon.Id = CouponStore.CouponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
    CouponStore.CouponList.Add(coupon);

    //return Results.Created($"/api/coupon/{coupon.Id:int}", coupon);
    return Results.CreatedAtRoute("GetCoupon", new { id = coupon.Id }, coupon);
}).WithName("CreateCoupon");

app.MapPut("/api/coupon", (int id, [FromBody] Coupon updatedCoupon) =>
{
    var existingCoupon = CouponStore.CouponList.FirstOrDefault(u => u.Id == id);
    if (existingCoupon == null)
        return Results.NotFound("Coupon not found.");

    if (string.IsNullOrEmpty(updatedCoupon.Name))
        return Results.BadRequest("Invalid Coupon Name.");

    var couponWithSameName = CouponStore.CouponList.FirstOrDefault(u => u.Name.ToLower() == updatedCoupon.Name.ToLower() && u.Id != id);
    if (couponWithSameName != null)
        return Results.BadRequest("Coupon with this name is already available.");

    existingCoupon.Name = updatedCoupon.Name;
    existingCoupon.Percent = updatedCoupon.Percent;
    existingCoupon.IsActive= updatedCoupon.IsActive;
    existingCoupon.LastUpdated = updatedCoupon.LastUpdated;

    return Results.Ok(existingCoupon);
}).WithName("UpdateCoupon");

app.MapDelete("/api/coupon/{id:int}", (int id) =>
{
    if(CouponStore.CouponList.FirstOrDefault(u=>u.Id == id) == null)
        return Results.BadRequest("You entered the wrong Id.");
    
    var coupon =  CouponStore.CouponList.FirstOrDefault(u=>u.Id == id);
    CouponStore.CouponList.Remove(coupon);
    return Results.Ok(CouponStore.CouponList);
}).WithName("DeleteCoupon");

app.UseHttpsRedirection();

app.Run();

