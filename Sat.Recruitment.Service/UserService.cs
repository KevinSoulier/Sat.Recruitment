using Sat.Recruitment.Model;
using Sat.Recruitment.Repository;
using Sat.Recruitment.Service.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Xml.Linq;

namespace Sat.Recruitment.Service
{
    public class UserService : IUserService
    {
        RepositoryContext _repository;
        public UserService(RepositoryContext repository)
        {
            _repository = repository;            
        }

        public Result CreateUser(User user)
        {
            Result result = new Result();
            var errors = "";

            if(!ValidateErrors(user, ref errors))
            {
                Debug.WriteLine(errors);
                result.IsSuccess = false;
                result.Message = errors;
                return result;
            }

            if (user.UserType == "Normal")
            {
                if (user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = user.Money * percentage;
                    user.Money = user.Money + gif;
                }
                if (user.Money < 100)
                {
                    if (user.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = user.Money * percentage;
                        user.Money = user.Money + gif;
                    }
                }
            }
            if (user.UserType == "SuperUser")
            {
                if (user.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = user.Money * percentage;
                    user.Money = user.Money + gif;
                }
            }
            if (user.UserType == "Premium")
            {
                if (user.Money > 100)
                {
                    var gif = user.Money * 2;
                    user.Money = user.Money + gif;
                }
            }

            var existingUser =_repository.Users.FirstOrDefault(x => (x.Email.Equals(user.Email) ||  x.Phone.Equals(user.Phone)) || 
                                                    (x.Name.Equals(user.Name) && (x.Address.Equals(user.Address))));
            
            if(existingUser == null)
            {
                _repository.Users.Add(user);
                _repository.SaveChanges();

                Debug.WriteLine("User Created");
                result.IsSuccess = true;
                result.Message = "User Created";
                return result;
            }
            else
            {
                Debug.WriteLine("The user is duplicated");
                result.IsSuccess = false;
                result.Message = "The user is duplicated";
                return result;
            }            
        }

        //Validate errors
        private bool ValidateErrors(User user, ref string errors)
        {
            if (user.Name == null)
            {
                //Validate if Name is null
                errors = "The name is required";
                return false;
            }
            if (user.Email == null)
            {
                //Validate if Email is null
                errors = errors + " The email is required";
                return false;
            }
            if (user.Address == null)
            {
                //Validate if Address is null
                errors = errors + " The address is required";
                return false;
            }
            if (user.Phone == null)
            {
                //Validate if Phone is null
                errors = errors + " The phone is required";
                return false;
            }
            return true;
        }

    }
}
