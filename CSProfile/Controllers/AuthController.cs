﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSProfile.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CSProfile.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{    // GET api/values
		[HttpPost, Route("login")]
		public IActionResult Login([FromBody]LoginModel user)
		{
			if (user == null)
			{
				return BadRequest("Invalid client request");
			}

			if (user.UserName == "admin" && user.Password == "1234")
			{
                //https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string key = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

                //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

				var tokeOptions = new JwtSecurityToken(
					issuer: "https://localhost:44305",
					audience: "https://localhost:44305",
					claims: new List<Claim>(),
					expires: DateTime.Now.AddMinutes(5),
					signingCredentials: signinCredentials
				);

				var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
				return Ok(new { Token = tokenString });
			}
			else
			{
				return Unauthorized();
			}
		}


	}
}
