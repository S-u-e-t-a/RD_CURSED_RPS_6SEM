﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlenkaAPI.Data;
using PlenkaAPI.Models;
using PlenkaWpf.Utils;

namespace PlenkaWpf.VM
{
    internal class UserEditVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public UserEditVM(User tempUser)
        {
            TempUser = new User()
            {
                UserId = tempUser.UserId,
                UserName = tempUser.UserName,
                UserPassword = tempUser.UserPassword,
                UserType = tempUser.UserType
            };
            EditingUser = tempUser;
            Db = DbContextSingleton.GetInstance();
            UserTypes = Db.UserTypes.Local.ToObservableCollection();
        }

        #endregion

        #endregion

        #region Properties

        public ObservableCollection<UserType> UserTypes { get; set; }
        public User TempUser { get; set; }
        public User EditingUser { get; set; }

        private MembraneContext Db { get; set; }

        #endregion

        #region Commands

        private RelayCommand _saveUser;
        public RelayCommand SaveUser
        {
            get
            {
                return _saveUser ??= new RelayCommand(o =>
                {
                    EditingUser.UserId = TempUser.UserId;
                    EditingUser.UserName = TempUser.UserName;
                    EditingUser.UserPassword = TempUser.UserPassword;
                    EditingUser.UserType = TempUser.UserType;

                    if (!Db.Users.Contains(EditingUser))
                    {
                        Db.Users.Add(EditingUser);
                    }

                    Db.SaveChanges();
                    OnClosingRequest();
                });
            }
        }

        #endregion
    }
}
