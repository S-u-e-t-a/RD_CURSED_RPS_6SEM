﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlenkaAPI.Data;
using PlenkaAPI.Models;
using PlenkaWpf.Utils;
using PlenkaWpf.View;
using MessageBox = HandyControl.Controls.MessageBox;

namespace PlenkaWpf.VM
{
    internal class UserExplorerVM : ViewModelBase

    {
        #region Functions

        #region Constructors

        public UserExplorerVM()
        {
            db = DbContextSingleton.GetInstance();
            Users = db.Users.Local.ToObservableCollection();
            UserTypes = db.UserTypes.Local.ToObservableCollection();
        }

        #endregion

        #endregion

        #region Properties

        private MembraneContext db;
        public User SelectedUser { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<UserType> UserTypes { get; set; }

        #endregion

        #region Commands

        private RelayCommand _addNewUser;

        public RelayCommand AddNewUser
        {
            get { return _addNewUser ??= new RelayCommand(o =>
            {
                ShowChildWindow(new UserEditWindow(new User()));
            }); }
        }

        private RelayCommand _editUser;

        public RelayCommand EditUser
        {
            get { return _editUser ??= new RelayCommand(o =>
            {
                ShowChildWindow(new UserEditWindow(SelectedUser));
            }); }
        }

        private RelayCommand _deleteUser;

        public RelayCommand DeleteUser
        {
            get { return _deleteUser ??= new RelayCommand(o =>
            {
                if (MessageBox.Show($"Вы действительно хотите удалить пользователя {SelectedUser.UserName}?", "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    db.Users.Remove(SelectedUser);
                    db.SaveChanges();
                }
            }); }
        }


        #endregion
    }
}