﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using PlenkaAPI.Data;
using PlenkaAPI.Models;
using PlenkaWpf.Utils;
using PlenkaWpf.View;
using MessageBox = HandyControl.Controls.MessageBox;

namespace PlenkaWpf.VM;

public class MaterialExplorerVM : ViewModelBase
{
    #region Functions

    #region Constructors

    public MaterialExplorerVM()
    {
        db.SavedChanges += (sender, args) => { OnPropertyChanged(nameof(Materials)); };
       
       
        //var db = DbContextSingleton.GetInstance();
        Materials = db.MembraneObjects.Local.ToObservableCollection();
    }

    #endregion

    #endregion

    #region Properties

    private MembraneContext db = DbContextSingleton.GetInstance();
    public ObservableCollection<MembraneObject> Materials { get; set; }

    public MembraneObject SelectedMemObject { get; set; }

    #endregion


    #region Commands

    private RelayCommand _addNewMemObject;

    public RelayCommand AddNewMemObject
    {
        get { return _addNewMemObject ??= new RelayCommand(o => { ShowChildWindow(new CreateMaterialWindow()); }); }
    }

    private RelayCommand _editMemObject;

    public RelayCommand EditMemObject
    {
        get
        {
            return _editMemObject ??= new RelayCommand(o =>
                {
                    ShowChildWindow(new MaterialEdit(SelectedMemObject));
                },
                c => SelectedMemObject != null
            );
        }
    }

    private RelayCommand _deleteMemObject;

    public RelayCommand DeleteMemObject
    {
        get { return _deleteMemObject ??= new RelayCommand(o =>
        {
            if (MessageBox.Show($"Вы действительно хотите удалить объект {SelectedMemObject.ObName}?", "Удаление объекта", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                foreach (var value in SelectedMemObject.Values)
                {
                    db.Values.Remove(value);
                }
                db.MembraneObjects.Remove(SelectedMemObject);
                db.SaveChanges();
            }
        },c=> SelectedMemObject != null); }
    }


    #endregion
}