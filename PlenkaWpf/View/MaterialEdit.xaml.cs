﻿using System.Windows;

using PlenkaAPI.Models;

using PlenkaWpf.VM;


namespace PlenkaWpf.View
{
    /// <summary>
    ///     Логика взаимодействия для MaterialEdit.xaml
    /// </summary>
    public partial class MaterialEdit
    {
        public MaterialEdit(MembraneObject material)
        {
            InitializeComponent();
            var vm = new MaterialEditVm(material);
            DataContext = vm;
            vm.ClosingRequest += (sender, e) => Close();
        }
    }
}
