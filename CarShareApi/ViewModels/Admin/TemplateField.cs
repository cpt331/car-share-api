﻿//======================================
//
//Name: TemplateField.cs
//Version: 1.0
//Date: 03/12/2017
//Developer: Steven Innes
//Contributor: Shawn Burriss
//
//======================================

namespace CarShareApi.ViewModels.Admin
{
    public class TemplateField
    {
        //The template field holds two Fields including the template name 
        //and the template description. The below are objects hold these details
        public string Name { get; set; }
        public string Description { get; set; }
    }
}