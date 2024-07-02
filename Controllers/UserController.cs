using DotNetAPI2.Data;
using DotNetAPI2.Models;
using DotNetAPI2.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace DotnetAPI2.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase
  {

    private readonly ApplicationDbContext _db;

    public UserController(ApplicationDbContext context)
    {
      _db = context;
    }



    
    [HttpGet("GetUsers")]
    [Authorize]
    public IEnumerable<User> GetUsers()
    {
      IEnumerable<User> users = _db.Users.ToList<User>();
      return users;
    }




    [HttpGet("GetSingleUser/{userId}")]
    [Authorize]
    public User GetSingleUser(int userId)
    {

      User? user = _db.Users.Where(u => u.UserId == userId).FirstOrDefault<User>();
      if (user != null)
      { return user; }
      throw new Exception("Failed to get userToAddDto");
    }


    [HttpGet("UserSalary/{userId}")]
    [Authorize]
    public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
    {
      return _db.UserSalary
          .Where(u => u.UserId == userId)
          .ToList();
    }

    [HttpPost("UserSalary")]
    [Authorize]
    public IActionResult PostUserSalaryEf(UserSalary userForInsert)
    {
      _db.UserSalary.Add(userForInsert);
      if (_db.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Adding UserSalary failed on save");
    }


    [HttpPut("UserSalary")]
    [Authorize]
    public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
    {
      UserSalary? userToUpdate = _db.UserSalary
          .Where(u => u.UserId == userForUpdate.UserId)
          .FirstOrDefault();

      if (userToUpdate != null)
      {
    
        if (_db.SaveChanges() > 0)
        {
          return Ok();
        }
        throw new Exception("Updating UserSalary failed on save");
      }
      throw new Exception("Failed to find UserSalary to Update");
    }


    [HttpDelete("UserSalary/{userId}")]
    [Authorize]
    public IActionResult DeleteUserSalaryEf(int userId)
    {
      UserSalary? userToDelete = _db.UserSalary
          .Where(u => u.UserId == userId)
          .FirstOrDefault();

      if (userToDelete != null)
      {
        _db.UserSalary.Remove(userToDelete);
        if (_db.SaveChanges() > 0)
        {
          return Ok();
        }
        throw new Exception("Deleting UserSalary failed on save");
      }
      throw new Exception("Failed to find UserSalary to delete");
    }


    [HttpGet("UserJobInfo/{userId}")]
    [Authorize]
    public IEnumerable<UserJobInfo> GetUserJobInfoEF(int userId)
    {
      return _db.UserJobInfo
          .Where(u => u.UserId == userId)
          .ToList();
    }

    [HttpPost("UserJobInfo")]
    [Authorize]
    public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
    {
      _db.UserJobInfo.Add(userForInsert);
      if (_db.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Adding UserJobInfo failed on save");
    }


    [HttpPut("UserJobInfo")]
    [Authorize]
    public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
    {
      UserJobInfo? userToUpdate = _db.UserJobInfo
          .Where(u => u.UserId == userForUpdate.UserId)
          .FirstOrDefault();

      if (userToUpdate != null)
      {
        
        if (_db.SaveChanges() > 0)
        {
          return Ok();
        }
        throw new Exception("Updating UserJobInfo failed on save");
      }
      throw new Exception("Failed to find UserJobInfo to Update");
    }


    [HttpDelete("UserJobInfo/{userId}")]
    [Authorize]
    public IActionResult DeleteUserJobInfoEf(int userId)
    {
      UserJobInfo? userToDelete = _db.UserJobInfo
          .Where(u => u.UserId == userId)
          .FirstOrDefault();

      if (userToDelete != null)
      {
        _db.UserJobInfo.Remove(userToDelete);
        if (_db.SaveChanges() > 0)
        {
          return Ok();
        }
        throw new Exception("Deleting UserJobInfo failed on save");
      }
      throw new Exception("Failed to find UserJobInfo to delete");
    }




    [HttpPut("EditUser")]
    [Authorize]
    public IActionResult EditUser(User user)
    {
      User? userDb = _db.Users
      .Where(u => u.UserId == user.UserId)
      .FirstOrDefault<User>();

      if (userDb != null)
      {
        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        if (_db.SaveChanges() > 0)
        {
          return Ok();
        }
        throw new Exception("Failed to update userToAddDto");
      }
      throw new Exception("Failed to get userToAddDto");

    }



    [HttpPut("AddUser")]
    [Authorize]
    public IActionResult AddUser(UserToAddDto userToAddDto)
    {
      User userDb = new User();

      userDb.Active = userToAddDto.Active;
      userDb.FirstName = userToAddDto.FirstName;
      userDb.LastName = userToAddDto.LastName;
      userDb.Email = userToAddDto.Email;
      userDb.Gender = userToAddDto.Gender;

      _db.Add(userDb);
      if (_db.SaveChanges() > 0)
      {
        return Ok();
      }
      throw new Exception("Failed to add userToAddDto");
    }



    [HttpPut("DeleteUser/{userId}")]
    [Authorize]
    public IActionResult DeleteUser(int userId)
    {
      User? userDb = _db.Users
      .Where(u => u.UserId == userId)
      .FirstOrDefault<User>();

      if (userDb != null)
      {
        _db.Users.Remove(userDb);
        if (_db.SaveChanges() > 0)
        {
          return Ok();
        }
        throw new Exception("Failed to delete userToAddDto");
      }
      throw new Exception("Failed to get userToAddDto");

    }



  }

}

