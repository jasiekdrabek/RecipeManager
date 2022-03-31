using RecipeManager.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecipeManager.DataAccess
{
    public class UserRepository
    {
        internal DbModel context;
        public UserRepository(DbModel context)
        {
            this.context = context;
        }
        public User GetByName(string name)
        {
            return context.Users.Where(x => x.Login == name).FirstOrDefault();
        }
        public List<User> GetAll()
        {
            return context.Users.ToList();
        }
        public User Get(Guid id)
        {
            return context.Users.Find(id);
        }
        public void AddUser(User user)
        {
            if (user != null)
            {
                context.Users.Add(user);
            }
        }
        public void UpdateUser(User User)
        {
            var UserFind = context.Users.Where(x => x.Id == User.Id).FirstOrDefault();
            UserFind.Password = User.Password;
            context.Users.Attach(UserFind);
            context.Entry(UserFind).State = EntityState.Modified;
        }
        public void RemoveUser(Guid id)
        {
            var UserFind = context.Users.Find(id);
            if (UserFind != null)
            {
                context.Users.Remove(UserFind);
            }
        }
    }
}