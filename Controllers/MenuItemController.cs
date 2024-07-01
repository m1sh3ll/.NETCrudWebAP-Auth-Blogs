﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotNetAPI2.Data;
using DotNetAPI2.Models;
using DotNetAPI2.Dtos;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace DotNetAPI2.Controllers
{
  [Route("api/MenuItem")]
  [ApiController]
  public class MenuItemController : ControllerBase
  {
    private readonly ApplicationDbContext _db;
    private ApiResponse _response;


    public MenuItemController(ApplicationDbContext db)
    {
      _db = db;
      _response = new ApiResponse();
    }




    [HttpGet]
    public async Task<IActionResult> GetMenuItems()
    {
      _response.Result = _db.MenuItems;
      _response.StatusCode = HttpStatusCode.OK;

      return Ok(_response);
    }



    [HttpGet("{id:int}", Name = "GetMenuItem")]
    public async Task<IActionResult> GetMenuItem(int id)
    {
      if (id == 0)
      {
        _response.StatusCode = HttpStatusCode.BadRequest;
        _response.IsSuccess = false;
        return BadRequest(_response);
      }

      MenuItem menuItem = _db.MenuItems.FirstOrDefault(u => u.Id == id);

      if (menuItem == null)
      {
        _response.StatusCode = HttpStatusCode.NotFound;
        _response.IsSuccess = false;
        return NotFound(_response);
      }
      _response.Result = menuItem;
      _response.StatusCode = HttpStatusCode.OK;

      return Ok(_response);
    }




    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateMenuItem([FromForm] MenuItemCreateDTO menuItemCreateDTO)
    {

      try
      {
        if (ModelState.IsValid)
        {
          if (menuItemCreateDTO.Image == null || menuItemCreateDTO.Image.Length == 0)
          {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
          }

          MenuItem menuItemToCreate = new()
          {
            Name = menuItemCreateDTO.Name,
            Price = menuItemCreateDTO.Price,
            Category = menuItemCreateDTO.Category,
            SpecialTag = menuItemCreateDTO.SpecialTag,
            Description = menuItemCreateDTO.Description,
            Image = menuItemCreateDTO.Image
          };
          _db.MenuItems.Add(menuItemToCreate);
          _db.SaveChanges();
          _response.Result = menuItemToCreate;
          _response.StatusCode = HttpStatusCode.Created;
          return CreatedAtRoute("GetMenuItem", new { id = menuItemToCreate.Id }, _response);
        }
        else
        {
          _response.IsSuccess = false;
        }

      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }





    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemUpdateDTO)
    {

      try
      {
        if (ModelState.IsValid)
        {
          if (menuItemUpdateDTO == null || id != menuItemUpdateDTO.Id)
          {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
          }


          MenuItem menuItemFromDb = await _db.MenuItems.FindAsync(id);

          if (menuItemFromDb == null)
          {
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.IsSuccess = false;
            return BadRequest();
          }


          menuItemFromDb.Name = menuItemUpdateDTO.Name;
          menuItemFromDb.Price = menuItemUpdateDTO.Price;
          menuItemFromDb.Category = menuItemUpdateDTO.Category;
          menuItemFromDb.SpecialTag = menuItemUpdateDTO.SpecialTag;
          menuItemFromDb.Description = menuItemUpdateDTO.Description;
          menuItemFromDb.Image = menuItemUpdateDTO.Image;


          _db.MenuItems.Update(menuItemFromDb);
          _db.SaveChanges();
          _response.StatusCode = HttpStatusCode.NoContent;
          return Ok(_response);
        }
        else
        {
          _response.IsSuccess = false;
        }

      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }





    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteMenuItem(int id)
    {

      try
      {

        if (id == 0)
        {
          _response.StatusCode = HttpStatusCode.BadRequest;
          _response.IsSuccess = false;
          return BadRequest();
        }


        MenuItem menuItemFromDb = await _db.MenuItems.FindAsync(id);

        if (menuItemFromDb == null)
        {
          _response.StatusCode = HttpStatusCode.BadRequest;
          _response.IsSuccess = false;
          return BadRequest();
        }

        _db.MenuItems.Remove(menuItemFromDb);
        _db.SaveChanges();
        _response.StatusCode = HttpStatusCode.NoContent;
        return Ok(_response);



      }
      catch (Exception ex)
      {
        _response.IsSuccess = false;
        _response.ErrorMessages = new List<string>() { ex.ToString() };
      }
      return _response;
    }



  }
}
